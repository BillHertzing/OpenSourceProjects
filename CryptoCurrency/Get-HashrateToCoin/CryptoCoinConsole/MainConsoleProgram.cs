using System;
using System.Collections.Generic;
using ATAP.CryptoCurrency;
using System.Diagnostics;

namespace CryptoCoinConsole {
    class MainConsoleProgram {
        static void Main(string[] args)
        {
            string response;
            int saferesponse;
            bool exitnow = false;
            CryptoCoinBuilder ccbuilder = new CryptoCoinBuilder();
            MiningRigBuilder mrbuilder = new MiningRigBuilder();

            // Attach event handlers
            AppDomain.CurrentDomain.ProcessExit += (s, ev) =>
            {
                Debug.WriteLine("process exit");
                exitnow = true;
            };

            Console.CancelKeyPress += (s, ev) =>
            {
                Debug.WriteLine("Ctrl+C pressed");
                exitnow = true;
                ev.Cancel = true;
            };

            List<HashRate> hrl = new List<HashRate> { new HashRate(1, 1, new TimeSpan(0, 0, 1)) };
            Dictionary<CoinsE, List<HashRate>> coinHashRateDict = new Dictionary<CoinsE, List<HashRate>> { { CoinsE.BTC, hrl } };
            PowerConsumption pc = new PowerConsumption(100, 1);
            Fees f = new Fees(1);
            MinerConfig minerConfig = MinerConfigBuilder.CreateNew()
                .AddFees(f)
                              .AddPowerConsumption(pc)
                             .AddHashRates(coinHashRateDict)
                              .Build();
            MinerConfigIDT minerConfigID = new MinerConfigIDT("ncat016");
            do
            {
                Console.WriteLine("(0)Exit : (1) Coins supported in miner01");
                response = Console.ReadLine();
                do
                {
                    Console.WriteLine("Failed, not simply just a number in the list, try again");
                    response = Console.ReadLine();
                } while (!int.TryParse(response, out saferesponse));
                switch(saferesponse) {
                    case 0:
                        Console.WriteLine("Exiting");
                        exitnow = true;
                        break;
                    case 1:
                        //ToDo: replace with Console request/response pairs
                        CryptoCoin cc = ccbuilder
                            .AddCoin(CoinsE.BTC)
                                            .AddHashRate(new HashRate(5, 1, new TimeSpan(0, 0, 1)))
                                            .AddDTSAndSpan(new DTSandSpan(DateTime.Now, TimeSpan.Zero))
                                            .AddAvgBlockTime(new TimeSpan(0, 0, 1))
                                            .AddBlockReward(2.5)
                                            .Build();

                        MiningRig mr01 = mrbuilder.AddMinerConfigs(new Dictionary<MinerConfigIDT, MinerConfig> { { minerConfigID, minerConfig } })
                                             .Build();
                        var coins = mr01.Miners[minerConfigID].HashRates.Keys;
                        foreach(var acoin in coins) { Console.WriteLine($@"{acoin}"); }
                        //var averageShareOfBlockRewardDT = new AverageShareOfBlockRewardDT(new TimeSpan(0, 0, 1), new TimeSpan(0, 0, 1), mr01.Miners().HashRate, cc.HashRate, cc.BlockRewardPerBlock)
                        //var numcoins = CryptoCoin.AverageShareOfBlockRewardPerNetworkHashRateSpanSafe(averageShareOfBlockRewardDT);
                        break;
                    default: break; }

            } while (exitnow == false);
        }
    }
}
