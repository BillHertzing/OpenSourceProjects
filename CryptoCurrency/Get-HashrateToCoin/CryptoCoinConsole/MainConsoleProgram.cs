using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ATAP.CryptoCurrency;
using ATAP.WebGet;
using Money;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Globalization;
using System.Threading.Tasks.Dataflow;

namespace CryptoCoinConsole {
    using Money = Money<decimal>;
    class MainConsoleProgram {
        public static async Task<int> Main(string[] args)
        {
             async Task<int> AsyncConsoleWork() {

                string response;
                int saferesponse;
                bool exitnow = false;
                

                // Attach event handlers
                AppDomain.CurrentDomain.ProcessExit += (s, ev) => { Debug.WriteLine("process exit");
                    exitnow = true; };

                Console.CancelKeyPress += (s, ev) => { Debug.WriteLine("Ctrl+C pressed");
                    exitnow = true;
                    ev.Cancel = true; };

                // create a variable holding the WebGet 
                WebGet webGet = WebGet.Instance;

                // create a variable to hold the currentUI CultureInfo, regionInfo, UIISOCurrencySymbol, and UIISOOCurrencyGlyph
                var currentUICulture = System.Threading.Thread.CurrentThread.CurrentUICulture;
                var currentUIRegionInfo = new RegionInfo(currentUICulture.LCID);
                // this is the 3-letter code from ISO 4217
                var currentUIISOCurrencySymbol = currentUIRegionInfo.ISOCurrencySymbol;
                // this is the glyph used when displaying a currency
                var currentUICurrencySymbol = currentUICulture.NumberFormat.CurrencySymbol;
                
                // create a var to hold the currency in which profitability will be calculated, baseCurrency, and initialize it to the current UI culture's ISOCurrencySymbol
                var profitabilityISOCurrencySymbol = currentUIISOCurrencySymbol;
                var profitabilityCurrencySymbol = currentUICurrencySymbol;
                // create a var to hold the interval over which profitability will be calculated
                var profitabilityTimeSpan = new TimeSpan(1, 0, 0);


                // declare an initially empty dictionary, keyed by CoinE, to hold a CryptoCoinNetworkInfo object 
                ConcurrentDictionary<CoinsE, CryptoCoinNetworkInfo> cryptoCoins = new ConcurrentDictionary<CoinsE, CryptoCoinNetworkInfo>();
                // declare an initially empty dictionary, keyed by CoinE, to hold a  Dictionary<CoinsE,double> 
                // ToDo: figure out how to allow the dictionary to support both fiat and crypto currencies in either the outer or nested dictionaries, for now, use a string as the key
                ConcurrentDictionary<CoinsE, Dictionary<string, double>> exchangeRates = new ConcurrentDictionary<CoinsE, Dictionary<string, double>>();

                // declare an initially empty dictionary, keyed by CoinE, to hold a task object that populates a single KeyValue pair in the cryptoCoins dictionary
                Dictionary<CoinsE, Task> cryptoCoinPopulators = new Dictionary<CoinsE, Task>();
                // declare an initially empty dictionary, keyed by CoinE, to hold a task object that populates a single KeyValue pair in the exchangeRates dictionary
                // ToDo: figure out how to allow the dictionary to support both fiat and crypto currencies in either the outer or nested dictionaries, for now, use a string as the key
                Dictionary<CoinsE, Task> exchangeRatePopulators = new Dictionary<CoinsE,Task>();

                // declare an initially empty bag that contains a Tuple of minerRigID, minerConfigID, and profitability
                ConcurrentBag<Tuple<string, string, decimal>> profitability = new ConcurrentBag<Tuple<string, string, decimal>>();
                // declare an initially empty list to hold the task objects that populate a batch N of Tuples in the profitability bag
                // ToDo: figure out how to allow the dictionary to support both fiat and crypto currencies in either the outer or nested dictionaries
                List<Task> profitabilityPopulators = new List<Task>();


                // Information that originates outside of the lifetime of the program (command line arguments, environment variables, config files, registry settings)
                // Name of file with the MinerFarm Configuration information
                var minerFarmConfigurationsFileFromOutside = @"C:\Dropbox\whertzing\CryptoCurrency\MyMiningfarm.json";
                // Define a method that will read a file of miner configurations and return a list of Tuples
                // ToDo: handle cancellation while reading/posting the miner configurations
                List<Tuple<string, string, PowerConsumption, Fees, Dictionary<CoinsE, HashRate>>> readMinerConfigurationsFileToList(string minerFarmConfigurationsFile)
                {
                    List<Tuple<string, string, PowerConsumption, Fees, Dictionary<CoinsE, HashRate>>> _rigConfigs = new List<Tuple<string, string, PowerConsumption, Fees, Dictionary<CoinsE, HashRate>>>();
                    var fs = new FileStream(minerFarmConfigurationsFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    Dictionary<string, Dictionary<string, MinerConfig>> minerFarm;
                    using (var streamReader = new StreamReader(fs))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        minerFarm = (Dictionary<string, Dictionary<string, MinerConfig>>)serializer.Deserialize(streamReader, typeof(Dictionary<string, Dictionary<string, MinerConfig>>));
                    }
                    //ToDo: rewrite as LINQ
                    foreach (var minerRigID in minerFarm.Keys)
                    {
                        foreach (var minerConfigID in minerFarm[minerRigID].Keys)
                        {
                            var minerConfig = minerFarm[minerRigID][minerConfigID];
                            var powerConsumption = minerConfig.PowerConsumption;
                            var fees = minerConfig.Fees;
                            _rigConfigs.Add(new Tuple<string, string, PowerConsumption, Fees, Dictionary<CoinsE, HashRate>>(minerRigID, minerConfigID, powerConsumption, fees, minerConfig.HashRates));
                            //Tuple<MiningRigIDT, MinerConfigIDT, PowerConsumption, Fees, Dictionary<CoinsE, HashRate>> o = new Tuple<MiningRigIDT, MinerConfigIDT, PowerConsumption, Fees, Dictionary<CoinsE, HashRate>>(minerRigID, minerConfigID, powerConsumption, fees, minerConfig.HashRates);
                        }
                    }
                    return _rigConfigs;
                }


                // cost of power

                // Create a CancellationTokenSource
                var cancellationSource = new CancellationTokenSource();

                // Create a WebGet registry with the information needed to get CryptoCoins and CurrencyExchangeRates
                WebGetRegistryKey AddWebGetRegistryEntryForThisCoinsCryptoCoins(CoinsE coin)
                {

                    WebGetRegistryKey webGetRegistryKey = new WebGetRegistryKey(coin.ToString() + "-ThisCoinsCryptoCoins");
                    if (!webGet.WebGetRegistry.Registry.ContainsKey(webGetRegistryKey))
                    {
                        webGet.WebGetRegistry.Registry[webGetRegistryKey] =
                            WebGetRegistryValueBuilder.CreateNew()
                            .AddPolicy(Polly.Policy.NoOpAsync())
                            .AddRequest(new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, "https://chain.so//api/v2/get_info/" + coin.ToString()))
                            .AddResponse(WebGetHttpResponseHowToHandleBuilder.CreateNew()
                                .AddTypeName(typeof(ATAP.Cryptocurrency.WebGetClasses.chain_so_api_v2_get_info))
                                .Build())
                            .Build();
                    }
                    return webGetRegistryKey;
                }

                do
                {
                    Console.WriteLine("(0)Exit : (1) List coins supported in minerFarmConfigurationsFile : (2) List all info about farm in minerFarmConfigurationsFile : (3) Get Coins Produced in a period using a TPL dataflow");
                    response = Console.ReadLine();
                    do
                    {
                        Console.WriteLine("Failed, not simply just a number in the list, try again");
                        response = Console.ReadLine();
                    } while (!int.TryParse(response, out saferesponse));
                    switch (saferesponse)
                    {
                        case 0:
                            Console.WriteLine("Exiting");
                            exitnow = true;
                            break;
                        case 1:
                            {
                                // read the file into the minerFarm structure
                                var fs = new FileStream(minerFarmConfigurationsFileFromOutside, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                                Dictionary<string, Dictionary<string, MinerConfig>> minerFarm;
                                using (var streamReader = new StreamReader(fs))
                                {
                                    JsonSerializer serializer = new JsonSerializer();
                                    minerFarm = (Dictionary<string, Dictionary<string, MinerConfig>>)serializer.Deserialize(streamReader, typeof(Dictionary<string, Dictionary<string, MinerConfig>>));
                                }

                                // query to get all of the coins in all of the Rigs in all MinerConfigs and print them to the console
                                minerFarm.Values.SelectMany(rig => rig.Values.SelectMany(cfg => cfg.HashRates.Keys)).ToList().ForEach((coin)=>
                                    { Console.WriteLine($"Coin {coin.ToString()}"); }
                                    );

                                //ToDo: make averageShareOfBlockRewardDT a console choice
                                //var averageShareOfBlockRewardDT = new AverageShareOfBlockRewardDT(new TimeSpan(0, 0, 1), new TimeSpan(0, 0, 1), mr01.Miners().HashRate, cc.HashRate, cc.BlockRewardPerBlock)
                                //ToDo: make numCoins a console choice
                                //var numCoins = CryptoCoinNetworkInfo.AverageShareOfBlockRewardPerNetworkHashRateSpanSafe(averageShareOfBlockRewardDT);
                            }
                            break;
                        case 2:
                            {

                                // read the file into the minerFarm structure
                                var fs = new FileStream(minerFarmConfigurationsFileFromOutside, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                                Dictionary<string, Dictionary<string, MinerConfig>> minerFarm;
                                using (var streamReader = new StreamReader(fs))
                                {
                                    JsonSerializer serializer = new JsonSerializer();
                                    minerFarm = (Dictionary<string, Dictionary<string, MinerConfig>>)serializer.Deserialize(streamReader, typeof(Dictionary<string, Dictionary<string, MinerConfig>>));
                                }

                                foreach (var minerRigID in minerFarm.Keys)
                                {
                                    foreach (var minerConfigID in minerFarm[minerRigID].Keys)
                                    {
                                        var minerConfig = minerFarm[minerRigID][minerConfigID];
                                        var powerConsumption = minerConfig.PowerConsumption;
                                        var fees = minerConfig.Fees;
                                        foreach (CoinsE coin in minerConfig.HashRates.Keys)
                                        {
                                            HashRate hashRate = minerConfig.HashRates[coin];
                                            //ToDo Localization
                                            Console.WriteLine($@"farm: [0], Rig: {minerRigID}, Config: {minerConfigID}, powerConsumption: {powerConsumption.ToString()}, fees: {fees.ToString()}, Coin: {coin}");
                                        }
                                    }
                                }
                            }
                            break;
                        case 3:
                            {


                                // Clear the dictionaries, bag, and list, cancelling then discarding any tasks that are in train.
                                // ToDo: write the function that does this

                                // Read the miner farm's configuration into a List of Tuples
                                var rigConfigs = readMinerConfigurationsFileToList(minerFarmConfigurationsFileFromOutside);

                                // This method attempts to use interlocking tasks to populate the three ConcurrentDictionary items
                                void useInterlockingTasksToPopulateModel(List<Tuple<string, string, PowerConsumption, Fees, Dictionary<CoinsE, HashRate>>> inList)
                                {
                                    // iterate over the list of Tuples
                                    foreach (var inData in inList)
                                    {
                                        //ToDo: figure out how to run the next two in parallel for a slight speedup
                                        foreach (var coin in inData.Item5.Keys)
                                        {
                                            // Look in the cryptoCoinPopulators dictionary for an entry for coin
                                            if (!cryptoCoinPopulators.ContainsKey(coin))
                                            {
                                                // if it is not present, create a task to populate cryptoCoins
                                                cryptoCoinPopulators[coin] = Task.Factory.StartNew(() =>
                                                {
                                                    ///ToDo: remove the delay and make a call to webGet instead
                                                    Task.Delay(1000, cancellationSource.Token);
                                                    cryptoCoins[coin] = CryptoCoinNetworkInfoBuilder.CreateNew().AddCoin(coin).AddHashRate(new HashRate(17000000, new TimeSpan(0, 0, 1))).Build();
                                                }, cancellationSource.Token);
                                            }

                                            // Look in the exchangeRatePopulators dictionary for an entry for coin
                                            if (!exchangeRatePopulators.ContainsKey(coin))
                                            {
                                                // if it is not present, create a task to populate exchangeRates
                                                exchangeRatePopulators[coin] = Task.Factory.StartNew(() =>
                                                {
                                                    ///ToDo: remove the delay and make a call to webGet instead
                                                    Task.Delay(1000, cancellationSource.Token);

                                                    exchangeRates[coin] = new Dictionary<string, double> {
                                                    // Coin to BTC conversion
                                                    { CoinsE.BTC.ToString(), 0.007 },
                                                    // Coin to profitabilityISOCurrencySymbol
                                                    // ToDo: figure out how to allow the nested dictionary to support both fiat and crypto currencies, for now, use a string as the key
                                                    {profitabilityISOCurrencySymbol, 300.0 }
                                                };

                                                }, cancellationSource.Token);
                                            }

                                            // ToDo: Figure out how to batch N minerConfigs of like coin into a single Task, 
                                            // Create a task to populate the profitability bag (not started)
                                            var addAction = new Action(() =>
                                              {
                                                  //ToDo: remove the delay and make a call to webGet instead
                                                  Task.Delay(1000, cancellationSource.Token);
                                                  profitability.Add(new Tuple<string, string, decimal>(inData.Item1, inData.Item2, Convert.ToDecimal(5.0)));
                                              });

                                            var taskProfitability1 = new Task(() =>
                                            {
                                                ///ToDo: remove the delay and make a call to webGet instead
                                                Task.Delay(1000, cancellationSource.Token);
                                                profitability.Add(new Tuple<string, string, decimal>(inData.Item1, inData.Item2, Convert.ToDecimal(5.0)));

                                            }, cancellationSource.Token);

                                            // same as 1 but using teh addAction method instead of a Lambda
                                            var taskProfitability2 = new Task(() => addAction(), cancellationSource.Token);

                                            // If the cryptoCoins dictionary for this coin does not have a key,
                                            // and then if the cryptoCoinPopulators of this coin has a task that is not CompletedSuccessfully 
                                            // ToDo: study this to ensure the various cancellation and faulted conditions are properly handled
                                            if (!cryptoCoins.ContainsKey(coin) && !cryptoCoinPopulators[coin].IsCompletedSuccessfully)
                                            {
                                                // Attach this task as a continuation task to the task that populates the cryptoCoins dictionary for this coin
                                                // ToDo: look at the antecedent's completion status
                                                //TaskContinuationOptions.OnlyOnRanToCompletion and TaskContinuationOptions.OnlyOnFaulted
                                                profitabilityPopulators.Add(cryptoCoinPopulators[coin].ContinueWith((t) => addAction(), cancellationSource.Token));
                                            }
                                            else
                                            {
                                                // No need to attach this as a continuation, because the previous task has completed. 
                                                profitabilityPopulators.Add(new Task(() => addAction(), cancellationSource.Token));
                                            }
                                            // If the exchangeRates dictionary for this coin does not have a key,
                                            // and then the exchangeRatePopulators of this coin has a task that is not CompletedSuccessfully 
                                            // ToDo: study this to ensure the various cancellation and faulted conditions are properly handled
                                            if (!exchangeRates.ContainsKey(coin) && !exchangeRatePopulators[coin].IsCompletedSuccessfully)
                                            {
                                                // Attach this task as a continuation task to the task that populates the exchangeRates dictionary for this coin
                                                exchangeRatePopulators[coin].ContinueWith((t) => addAction(), cancellationSource.Token);
                                            }
                                            else
                                            {
                                                // No need to attach this as a continuation, because the previous task has completed. 
                                                profitabilityPopulators.Add(new Task(() => addAction(), cancellationSource.Token));
                                            }
                                        }
                                    }
                                }

                                // This method attempts to use interlocking async tasks to populate the three ConcurrentDictionary items
                                async Task useInterlockingAsyncTasksToPopulateModel(List<Tuple<string, string, PowerConsumption, Fees, Dictionary<CoinsE, HashRate>>> inList)
                                {
                                    async Task getProfitability(Tuple<string, string, PowerConsumption, Fees, Dictionary<CoinsE, HashRate>> inData)
                                    {

                                        // declare and initialize the overall profitability
                                        decimal totalGrossRevenuePerSpanInSpecifiedCurrency = default;
                                        // loop over every coin in the supplied dictionary, and calculate its contribution to the overall profitability
                                        foreach (var coin in inData.Item5.Keys)
                                        {
                                            /*
                                            // Get the network information for this coin from the ConcurrentDictionary cryptoCoin
                                            // if cryptoCoin does not contain the key for this coin, update that dictionary with a Lazy function 
                                            CryptoCoinNetworkInfo cryptoCoin = cryptoCoins.GetOrAdd(coin.ToString(), x => new Lazy<CryptoCoinNetworkInfo>(() =>
                                            {
                                                // ToDo: call webGet for the CryptoCoinNetworkInfo network information for this coin
                                                return CryptoCoinNetworkInfoBuilder.CreateNew().AddCoin(coin).Build();
                                            }));

                                            // Get the exchangeRate information for this coin from the ConcurrentDictionary currencyExchangeRate
                                            // if currencyExchangeRate does not contain the key for this coin, update that dictionary with a Lazy function 
                                            double currencyExchangeRate = exchangeRates.GetOrAdd(coin.ToString(), x => new Lazy<double>(() =>
                                            {
                                                // ToDo: call webGet for the ExchangeRate for this coin
                                                return 0.001;
                                            }));
                                            */
                                            CryptoCoinNetworkInfo cryptoCoin;
                                            // This is just a placeholder to get the branch to compile again, it won't work
                                            if (!cryptoCoins.TryGetValue(coin, out cryptoCoin)) throw new Exception("uh-oh");
                                            Dictionary<string, double> innerDict;
                                              double currencyExchangeRate;
                                            // This is just a placeholder to get the branch to compile again, it won't work
                                            if (!exchangeRates.TryGetValue(coin, out innerDict)) throw new Exception("uh-oh");
                                            currencyExchangeRate = innerDict[profitabilityISOCurrencySymbol];
                                            // calculate the average number of coins generated by this miner during the profitabilityTimeSpan
                                            var averageShareOfBlockRewardPerSpan = CryptoCoinNetworkInfo.AverageShareOfBlockRewardPerSpanFast(new AverageShareOfBlockRewardDT(
                                                cryptoCoin.AvgBlockTime, profitabilityTimeSpan, inData.Item5[coin], cryptoCoin.HashRate, cryptoCoin.BlockRewardPerBlock
                                                ), profitabilityTimeSpan);

                                            totalGrossRevenuePerSpanInSpecifiedCurrency += (decimal)(averageShareOfBlockRewardPerSpan * currencyExchangeRate);
                                        }
                                        profitability.Add(new Tuple<string, string, decimal>(inData.Item1, inData.Item2, Convert.ToDecimal(5.0)));
                                    }
                                    // Create a list to hold every task created to populate the profitability dictionary
                                    List<Task> tasks = new List<Task>();
                                    // start a getProfitability task to update the profitability dictionary for each item in the inList
                                    foreach (var inData in inList)
                                    {
                                        tasks.Add(getProfitability(inData));
                                    }

                                    // wait for all to complete
                                    try
                                    {
                                        await Task.WhenAll(tasks.ToArray());
                                        // When all the tasks are done, the work is finished
                                    }
                                    catch (AggregateException e)
                                    {
                                        Console.WriteLine("\nThe following exceptions have been thrown by WaitAll(): (THIS WAS NOT EXPECTED)");
                                        for (int j = 0; j < e.InnerExceptions.Count; j++)
                                        {
                                            Console.WriteLine($"\n-------------------------------------------------\n{e.InnerExceptions[j].ToString()}");
                                        }
                                    }
                                }
                                /*
                                async TaskCompletionSource<bool> getNetworkInfoAsync(CoinsE coin, CancellationToken cancellationToken, IProgress<bool> progress)
                                {
                                    networkInfo.AddOrUpdate();
                                }


                                async TaskCompletionSource<bool> getExchangeRatesAsync(CoinsE coin, CancellationToken cancellationToken, IProgress<bool> progress) { }
                                async TaskCompletionSource<bool> getProfitabilityAsync(Tuple<string, string, PowerConsumption, Fees, Dictionary<CoinsE, HashRate>> tup, CancellationToken cancellationToken, IProgress<bool> progress)
                                {
                                    foreach (var c in (tup.Item5 as Dictionary<CoinsE, HashRate>).Keys)
                                    {
                                        var valuefound = networkInfo.GetOrAdd(c.ToString(),
                                            x => new Lazy<Tuple<IHashRate, TimeSpan>>(
                                                () =>
                                                {
                                                    //get from async io 
                                                    return (new HashRate(), new TimeSpan());
                                                }
                                                )
                                        );
                                    }
                                }
                                */
                                // This method attempts to use TPL DataFlow modules to populate the three ConcurrentDictionary items
                                void useDataFlowModulesToPopulateModel(List<Tuple<string, string, PowerConsumption, Fees, Dictionary<CoinsE, HashRate>>> inList)
                                {
                                    /*

                                        // The initial block is a BroadcastBlock that sends the tuple to the next three dataflow blocks 

                                        // This Func takes a tuple which includes a Dictionary whose key collection is of type CoinsE, and gets network data about each coin, stores it in an observable ConcurrentDictionary to be used both as a cache and to be displayed, and then passes the name of the coin onward only if it has not yet appeared in the ConcurrentDictionary
                                        Func < Tuple < string, string, PowerConsumption, Fees, Dictionary < CoinsE, HashRate >>>(Tuple<string, string, PowerConsumption, Fees, Dictionary<CoinsE, HashRate>> tup) getNetworkInfo = (tup =>
                                        {
                                            //var getNetworkInfo = new TransformManyBlock<Tuple<MiningRigIDT, MinerConfigIDT, PowerConsumption, Fees, Dictionary<CoinsE, HashRate>>, CoinsE>(t => {

                                            WebGetRegistryKey AddWebGetRegistryEntryForThisCoinsNetworkInfo(CoinsE coin)
                                            {
                                                WebGetRegistryKey webGetRegistryKey = new WebGetRegistryKey(coin.ToString() + "-ThisCoinsNetworkInfo");
                                                if (!webGet.WebGetRegistry.Registry.ContainsKey(webGetRegistryKey))
                                                {
                                                    webGet.WebGetRegistry.Registry[webGetRegistryKey] =
                                                        WebGetRegistryValueBuilder.CreateNew()
                                                        .AddPolicy(Policy.NoOpAsync())
                                                        .AddRequest(new HttpRequestMessage(HttpMethod.Get, "https://chain.so//api/v2/get_info/" + coin.ToString()))
                                                        .AddResponse(WebGetHttpResponseHowToHandleBuilder.CreateNew()
                                                            .AddTypeName(typeof(chain_so_api_v2_get_info))
                                                            .Build())
                                                        .Build();
                                                }
                                                return webGetRegistryKey;
                                            }

                                            Dictionary<CoinsE, HashRate> d = t.Item5;
                                            // ToDo: make this loop parallel and async, with cancellation and exception handling
                                            chain_so_api_v2_get_info result;
                                            Task[] tasks;
                                            foreach (var coin in d.Keys)
                                            {
                                                var k = AddWebGetRegistryEntryForThisCoinsNetworkInfo(coin);
                                                ///ToDo: Add cancellation and exception handling
                                                var task = webGet.ASyncWebGetFast<chain_so_api_v2_get_info>(k);
                                                result = task.Result;
                                                // Insert the result into the Concurrent
                                            }
                                            // ToDo: put this into a WaitAny for the array of tasks, and handle each results async as it comes in
                                            // result = task.Result;
                                            ///ToDo: Use TryParse to better handle exceptions??

                                        }
                                        , new ExecutionDataflowBlockOptions { CancellationToken = cancellationSource.Token });

                                        // This block joins the outputs of ..., calculates the income (in DisplayCurrency) per coin per period, sums the income, subtracts the fees, calculates the cost of power per period, and subtracts that, and stores the profit (in DisplayCurrency), in a nested observable ConcurrentDictionary (that implements a sortByProfit) to be used as a cache and to be displayed

                                        // The Func that does the work of updating the profitability 
                                        Func(Tuple < string, string, PowerConsumption, Fees, Dictionary < CoinsE, HashRate >>>>) updateFinalList = (tup =>
                                        {
                                        });
                                        // Define the final consumer block
                                        var updateProfitability = new ActionBlock<Tuple<string, string, PowerConsumption, Fees, Dictionary<CoinsE, HashRate>>>(tup => { Console.WriteLine($"Rig: " + tup.Item1.ToString()); }, new ExecutionDataflowBlockOptions { CancellationToken = cancellationSource.Token });

                                        // Connect the pipeline
                                        broadcastMinerConfigs.LinkTo(getNetworkInfo);
                                        broadcastMinerConfigs.LinkTo(getCurrencyPairs);


                                        // Create the completion tasks
                                        readMinerConfigurationsFileToList.Completion.ContinueWith(t =>
                                        {
                                            if (t.IsFaulted)
                                            {
                                                ((IDataflowBlock)printTuples).Fault(t.Exception);
                                            }
                                            else
                                            {
                                                printTuples.Complete();
                                            }
                                        });

                                        foreach (var inData in inList)
                                        {
                                            // Start the producer process
                                            readMinerConfigurationsFileToList.SendAsync(inData);

                                        }
                                        // Mark the producer as complete
                                        readMinerConfigurationsFileToList.Complete();

                                        // Wait for the final consumer bl0ock to complete (or not, to keep the main thread responsive)
                                        updateProfitability.Completion.Wait();
                                        */
                                }
                            }
                            break;
                        default: break;
                    }

                } while (exitnow == false);
                return 0;
            }
            return await AsyncConsoleWork(); ; 
        }
    }
}
