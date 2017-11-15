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
namespace CryptoCoinConsole {
    using Money = Money<decimal>;
    class MainConsoleProgram {
        public static void Main(string[] args)
        {
            ConcurrentDictionary<string, Money> exchangeRates;
            ConcurrentDictionary<string, Tuple<IHashRate, TimeSpan>> networkInfo;
            ConcurrentDictionary<string, Tuple<string, string, TimeSpan>> profitability;

            string response;
            int saferesponse;
            bool exitnow = false;
            ;

            // Attach event handlers
            AppDomain.CurrentDomain.ProcessExit += (s, ev) => { Debug.WriteLine("process exit");
                exitnow = true; };

            Console.CancelKeyPress += (s, ev) => { Debug.WriteLine("Ctrl+C pressed");
                exitnow = true;
                ev.Cancel = true; };

            // create a variable holding the WebGet 
            WebGet webGet = WebGet.Instance;

            // create the exchangeRate dictionary
            exchangeRates = new ConcurrentDictionary<string, Money>();
            // populate it with the BTC->DisplayCurrency exchangeRate

            // Name of file with the MinerFarm Configuration information
            var minerFarmConfigurationsFile = @"C:\Dropbox\whertzing\CryptoCurrency\MyMiningfarm.json";
            do
            {
                Console.WriteLine("(0)Exit : (1) List coins supported in minerFarmConfigurationsFile : (2) List all info about farm in minerFarmConfigurationsFile : (3) Get Coins Produced in a period using a TPL dataflow");
                response = Console.ReadLine();
                do
                {
                    Console.WriteLine("Failed, not simply just a number in the list, try again");
                    response = Console.ReadLine();
                } while (!int.TryParse(response, out saferesponse));
                switch(saferesponse)
                {
                    case 0:
                        Console.WriteLine("Exiting");
                        exitnow = true;
                        break;
                    case 1:
                        {
                            // read the file into the minerFarm structure
                            var fs = new FileStream(minerFarmConfigurationsFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                            Dictionary<string, Dictionary<string, MinerConfig>> minerFarm;
                            using(var streamReader = new StreamReader(fs))
                            {
                                JsonSerializer serializer = new JsonSerializer();
                                minerFarm = (Dictionary<string, Dictionary<string, MinerConfig>>)serializer.Deserialize(streamReader, typeof(Dictionary<string, Dictionary<string, MinerConfig>>));
                            }

                            // query to get all of the coins in all of the Rigs in all MinerConfigs
                            List<CoinsE> allCoinsList = new List<CoinsE> { CoinsE.BTC };// minerFarm.Values.FindAll(r => (1==1));
                                                                                        //[miningRigID][minerConfigID1].HashRates.Keys.ToList()
                                                                                        //              .Union(farm[0][miningRigID][minerConfigID2].HashRates.Keys.ToList());


                            foreach(var coin in allCoinsList)
                            {
                                //ToDo Localization
                                Console.WriteLine($"Coin {coin}.ToString()");
                            }
                            //ToDo: make averageShareOfBlockRewardDT a console choice
                            //var averageShareOfBlockRewardDT = new AverageShareOfBlockRewardDT(new TimeSpan(0, 0, 1), new TimeSpan(0, 0, 1), mr01.Miners().HashRate, cc.HashRate, cc.BlockRewardPerBlock)
                            //ToDo: make numCoins a console choice
                            //var numCoins = CryptoCoin.AverageShareOfBlockRewardPerNetworkHashRateSpanSafe(averageShareOfBlockRewardDT);
                        }
                        break;
                    case 2:
                        {

                            // read the file into the minerFarm structure
                            var fs = new FileStream(minerFarmConfigurationsFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                            Dictionary<string, Dictionary<string, MinerConfig>> minerFarm;
                            using(var streamReader = new StreamReader(fs))
                            {
                                JsonSerializer serializer = new JsonSerializer();
                                minerFarm = (Dictionary<string, Dictionary<string, MinerConfig>>)serializer.Deserialize(streamReader, typeof(Dictionary<string, Dictionary<string, MinerConfig>>));
                            }

                            foreach(var minerRigID in minerFarm.Keys)
                            {
                                foreach(var minerConfigID in minerFarm[minerRigID].Keys)
                                {
                                    var minerConfig = minerFarm[minerRigID][minerConfigID];
                                    var powerConsumption = minerConfig.PowerConsumption;
                                    var fees = minerConfig.Fees;
                                    foreach(CoinsE coin in minerConfig.HashRates.Keys)
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
                            // Create a CancellationTokenSource
                            /*

                        var cancellationSource = new CancellationTokenSource();

                        async TaskCompletionSource<bool> getNetworkInfoAsync(CoinsE coin,CancellationToken cancellationToken, IProgress<bool> progress) {
                            networkInfo.AddOrUpdate();
                        }
                        async TaskCompletionSource<bool> getExchangeRatesAsync(CoinsE coin, CancellationToken cancellationToken, IProgress<bool> progress) { }
                        async TaskCompletionSource<bool> getProfitabilityAsync(Tuple< string, string, PowerConsumption, Fees, Dictionary < CoinsE, HashRate >> tup, CancellationToken cancellationToken, IProgress<bool> progress)
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
                            // Define a method that will read a  file of miner configurations and create a task for each
                            // ToDo: handle cancellation while reading/posting the miner configurations
                            List<Task> emitMinerConfigs(string minerFarmConfigurationsFile)
                            {
                                List<Task> _tasks = new List<Task>();
                                // var emitMinerConfigs = new TransformBlock<string, Tuple<MiningRigIDT, MinerConfigIDT, PowerConsumption, Fees, Dictionary<CoinsE, HashRate>>>(minerFarmConfigurationsFile => {
                                var fs = new FileStream(minerFarmConfigurationsFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                                List<Dictionary<MinerRigIDT, Dictionary<MinerConfigIDT, MinerConfig>>> farm;
                                using (var streamReader = new StreamReader(fs))
                                {
                                    JsonSerializer serializer = new JsonSerializer();
                                    farm = (List<Dictionary<MinerRigIDT, Dictionary<MinerConfigIDT, MinerConfig>>>)serializer.Deserialize(streamReader, typeof(List<Dictionary<MinerRigIDT, Dictionary<MinerConfigIDT, MinerConfig>>>));
                                }

                                var minerFarm = farm[0];

                                foreach (MinerRigIDT minerRigID in minerFarm.Keys)
                                {
                                    foreach (MinerConfigIDT minerConfigID in minerFarm[minerRigID].Keys)
                                    {
                                        var minerConfig = minerFarm[minerRigID][minerConfigID];
                                        var powerConsumption = minerConfig.PowerConsumption;
                                        var fees = minerConfig.Fees;
                                        _tasks.Add(Task<Tuple<MinerRigIDT, MinerConfigIDT, PowerConsumption, Fees, Dictionary<CoinsE, HashRate>>>.Factory.StartNew(updateFinalList(minerRigID, minerConfigID, powerConsumption, fees, minerConfig.HashRates)));
                                        //Tuple<MiningRigIDT, MinerConfigIDT, PowerConsumption, Fees, Dictionary<CoinsE, HashRate>> o = new Tuple<MiningRigIDT, MinerConfigIDT, PowerConsumption, Fees, Dictionary<CoinsE, HashRate>>(minerRigID, minerConfigID, powerConsumption, fees, minerConfig.HashRates);
                                        //target.SendASync(new Tuple<MiningRigIDT, MinerConfigIDT, PowerConsumption, Fees, Dictionary<CoinsE, HashRate>>(minerRigID, minerConfigID, powerConsumption, fees, minerConfig.HashRates));
                                    }
                                }
                                return _tasks;
                            }

                                // Read the miner farm's configuration, and start an updateFinalList task for each one
                                var tasks = emitMinerConfigs(minerFarmConfigurationsFile);
                                try
                                {
                                    Task.WaitAll(tasks.ToArray());
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


                                //Func(Tuple<MiningRigIDT, MinerConfigIDT, PowerConsumption, Fees, Dictionary<CoinsE, HashRate>>>>) updateFinalList = (tup =>
                                //{
                                //}
                                //{

                                // The initial block is a BroadcastBlock that sends the tuple to the next three dataflow blocks 

                                // This Func takes a tuple which includes a Dictionary whose key collection is of type CoinsE, and gets network data about each coin, stores it in an observable ConcurrentDictionary to be used both as a cache and to be displayed, and then passes the name of the coin onward only if it has not yet appeared in the ConcurrentDictionary
                                //Func << Tuple < MiningRigIDT, MinerConfigIDT, PowerConsumption, Fees, Dictionary < CoinsE, HashRate >>>> getNetworkInfo = (tup =>
                                //{
                                //    //var getNetworkInfo = new TransformManyBlock<Tuple<MiningRigIDT, MinerConfigIDT, PowerConsumption, Fees, Dictionary<CoinsE, HashRate>>, CoinsE>(t => {

                                //    WebGetRegistryKey AddWebGetRegistryEntryForThisCoinsNetworkInfo (CoinsE coin)
                                //    {
                                //        WebGetRegistryKey webGetRegistryKey = new WebGetRegistryKey(coin.ToString() + "-ThisCoinsNetworkInfo");
                                //        if (!webGet.WebGetRegistry.Registry.ContainsKey(webGetRegistryKey))
                                //        {
                                //            webGet.WebGetRegistry.Registry[webGetRegistryKey] =
                                //                WebGetRegistryValueBuilder.CreateNew()
                                //                .AddPolicy(Policy.NoOpAsync())
                                //                .AddRequest(new HttpRequestMessage(HttpMethod.Get, "https://chain.so//api/v2/get_info/"+ coin.ToString()))
                                //                .AddResponse(WebGetHttpResponseHowToHandleBuilder.CreateNew()
                                //                    .AddTypeName(typeof(chain_so_api_v2_get_info))
                                //                    .Build())
                                //                .Build();
                                //        }
                                //        return webGetRegistryKey;
                                //    }

                                //    Dictionary<CoinsE, HashRate> d = t.Item5;
                                //    // ToDo: make this loop parallel and async, with cancellation and exception handling
                                //    chain_so_api_v2_get_info result;
                                //    Task[] tasks;
                                //    foreach (var coin in d.Keys)
                                //    {
                                //        var k = AddWebGetRegistryEntryForThisCoinsNetworkInfo(coin);
                                //        ///ToDo: Add cancellation and exception handling
                                //        var task = webGet.ASyncWebGetFast<chain_so_api_v2_get_info>(k);
                                //        result = task.Result;
                                //        // Insert the result into the Concurrent
                                //    }
                                //    // ToDo: put this into a WaitAny for the array of tasks, and handle each results async as it comes in
                                //    // result = task.Result;
                                //    ///ToDo: Use TryParse to better handle exceptions??

                                //}, new ExecutionDataflowBlockOptions { CancellationToken = cancellationSource.Token });

                                //// This block joins the outputs of ..., calculates the income (in DisplayCurrency) per coin per period, sums the income, subtracts the fees, calculates the cost of power per period, and subtracts that, and stores the profit (in DisplayCurrency), in a nested observable ConcurrentDictionary (that implements a sortByProfit) to be used as a cache and to be displayed


                                //// Define the final consumer block
                                //var printTuples = new ActionBlock<Tuple<MiningRigIDT, MinerConfigIDT, PowerConsumption, Fees, Dictionary<CoinsE, HashRate>>>(tup => { Console.WriteLine($"Rig: " + tup.Item1.ToString()); }, new ExecutionDataflowBlockOptions { CancellationToken = cancellationSource.Token });

                                //// Connect the pipeline
                                //broadcastMinerConfigs.LinkTo(getNetworkInfo);
                                //broadcastMinerConfigs.LinkTo(getCurrencyPairs);


                                //// Create the completion tasks
                                //emitMinerConfigs.Completion.ContinueWith(t =>
                                //{
                                //    if (t.IsFaulted)
                                //    {
                                //        ((IDataflowBlock)printTuples).Fault(t.Exception);
                                //    }
                                //    else
                                //    {
                                //        printTuples.Complete();
                                //    }
                                //});

                                //// Start the producer process
                                //emitMinerConfigs.Post(consoleMinerFarmConfigurationsFile);

                                //// Mark the producer as complete
                                //emitMinerConfigs.Complete();

                                //// Wait for the final consumer bl0ock to complete
                                //printTuples.Completion.Wait();
                            */
                        }
                        break;
                    default: break;
                }

            } while (exitnow == false);
        }
    }
}
