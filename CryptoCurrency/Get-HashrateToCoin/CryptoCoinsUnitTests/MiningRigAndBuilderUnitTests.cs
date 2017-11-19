using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ATAP.UnitTestUtilities;
using Newtonsoft.Json;
using Xunit;
using UnitsNet;

namespace ATAP.CryptoCurrency.MinerRigAndBuilderUnitTests {
    public class Fixture {
        public double feeAsAPercent;
        public Fees fees;
        public HashRate hashRate;
        public HashRate hashRate1000X;
        public HashRate hashRate2T;
        public HashRate hashRate2X;
        public Dictionary<CoinsE, HashRate> hashRates;
        public int hashRateUOM;
        public double hashRateValue;
        public TimeSpan hashRatePeriod;
        public MinerConfig minerConfig;
        public PowerConsumption powerConsumption;
        public TimeSpan powerConsumptionUOM;
        public Power powerConsumptionWatts;
        public TimeSpan powerConsumptionPeriod;
        //ToDo decide to keep or delete MinerConfigIDT public Dictionary<MinerConfigIDT, MinerConfig> minerConfigDict;
        public Dictionary<string, MinerConfig> minerConfigDict;
        //ToDo decide to keep or delete MinerConfigIDT public MinerConfigIDT minerConfigID;
        public string minerConfigID;
        //ToDo decide to keep or delete MinerConfigIDT public Dictionary<MinerRigIDT, Dictionary<MinerConfigIDT, MinerConfig>> minerFarmDict;
        public Dictionary<string, Dictionary<string, MinerConfig>> minerFarmDict;
        //ToDo decide to keep or delete MinerRigIDT public MinerRigIDT minerRigIDT;
        public string minerRigIDT;
        public TemporaryFile tempFileWithMinerRigs;
        public Fixture()
        {
            hashRatePeriod = new TimeSpan(0, 0, 1);
            hashRateValue = 10000;
            
            hashRate = new HashRate(hashRateValue, hashRatePeriod);
            hashRate2X = new HashRate(hashRateValue * 2,  hashRatePeriod);
            hashRate1000X = new HashRate(hashRateValue * 1000, hashRatePeriod);
            hashRate2T = new HashRate(hashRateValue,  hashRatePeriod + hashRatePeriod);
            powerConsumptionWatts = new Power(1000.0);
            powerConsumptionPeriod = new TimeSpan(1,0,0);
            powerConsumption = new PowerConsumption(powerConsumptionWatts, powerConsumptionPeriod);
            feeAsAPercent = 1.0;
            fees = new Fees(feeAsAPercent);
            hashRates = new Dictionary<CoinsE, HashRate> { { CoinsE.ETH, hashRate } };
            minerConfig = new MinerConfig(hashRates, powerConsumption, fees);
            //ToDo decide to keep or delete MinerConfigIDT new MinerConfigIDT("MinerConfig1");
            minerConfigID = "MinerConfig1";
            //ToDo decide to keep or delete MinerConfigIDT minerConfigDict = new Dictionary<MinerConfigIDT, MinerConfig> { { minerConfigID, minerConfig } };
            minerConfigDict = new Dictionary<string, MinerConfig> { { minerConfigID, minerConfig } };
            //ToDo decide to keep or delete MinerRigIDT minerRigIDT = new MinerRigIDT("MinerRig1");
            minerRigIDT = "MinerRig1";
            //ToDo decide to keep or delete MinerRigIDT minerFarmDict = new Dictionary<MinerRigIDT, Dictionary<MinerConfigIDT, MinerConfig>> { { minerRigIDT, minerConfigDict } };
            minerFarmDict = new Dictionary<string, Dictionary<string, MinerConfig>> { { minerRigIDT, minerConfigDict } };
            // Define a MinerFarm using builders (fluent), which will be the one used for testing
            //ToDo decide to keep or delete MinerRigIDT Dictionary<MinerRigIDT, Dictionary<MinerConfigIDT, MinerConfig>> farm =
            Dictionary<string, Dictionary<string, MinerConfig>> farm =
                //ToDo decide to keep or delete MinerRigIDT and MinerConfigIDT new Dictionary<MinerRigIDT, Dictionary<MinerConfigIDT, MinerConfig>> {
                new Dictionary<string, Dictionary<string, MinerConfig>> {
                    {//ToDo decide to keep or delete MinerRigIDT  new MinerRigIDT("MinerRig1"),
                        "MinerRig1", 
                    //ToDo decide to keep or delete MinerConfigIDT  new Dictionary<MinerConfigIDT, MinerConfig> {

                        new Dictionary<string, MinerConfig> {
                            //ToDo decide to keep or delete MinerConfigIDT  // new MinerConfigIDT("claymore-NormalClocks-ETHDSH"), 
                            { "claymore-normalclocks-ETHDSH", 
                                // use the same Miner Config values defined earlier
                                MinerConfigBuilder.CreateNew()
                                    .AddPowerConsumption(new PowerConsumption(new UnitsNet.Power(1000), new TimeSpan(1,0,0)))
                                    .AddFees(new Fees(1.0))
                                    .AddHashRates(hashRates)
                                    .Build()
            }}}};
            tempFileWithMinerRigs = new TemporaryFile();
        }
    }

    
    public class MinerConfigTests : IClassFixture<Fixture> {
        public Fixture fixture;
        public MinerConfigTests(Fixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        void MinerConfigBuilderAddFeesReturnsMinerConfigWithFees()
        {
            var f = fixture.fees;
            var a = MinerConfigBuilder.CreateNew()
                        .AddFees(f)
                        .Build();
            Assert.Equal(f.FeeAsAPercent, a.Fees.FeeAsAPercent);
        }
        [Fact]
        void MinerConfigBuilderAddHashRatesReturnsMinerConfigWithHashRates()
        {
            var h = fixture.hashRates;
            var a = MinerConfigBuilder.CreateNew()
                        .AddHashRates(fixture.hashRates)
                        .Build();
            Assert.Equal(fixture.hashRates, a.HashRates);
        }
        [Fact]
        void MinerConfigBuilderAddPowerConsumptionReturnsMinerConfigWithPowerConsumption()
        {
            var p = fixture.powerConsumption;
            var a = MinerConfigBuilder.CreateNew()
                        .AddPowerConsumption(p)
                        .Build();
            Assert.Equal(p, a.PowerConsumption);
        }
        [Fact]
        void MinerConfigBuilderAddPowerFeesHashRatesReturnsMinerConfigWithPowerFeesHashRates()
        {
            var p = fixture.powerConsumption;
            var h = fixture.hashRates;
            var f = fixture.fees;
            var a = MinerConfigBuilder.CreateNew()
                        .AddPowerConsumption(p)
                        .AddFees(f)
                        .AddHashRates(fixture.hashRates)
                        .Build();
            Assert.Equal(fixture.hashRates, a.HashRates);
        }
        [Fact]
        void MinerConfigBuilderReturnsMinerConfigT()
        {
            var a = MinerConfigBuilder.CreateNew()
                        .Build();
            Assert.IsType<MinerConfig>(a);
        }
    }
    public class MinerRigTests : IClassFixture<Fixture> {
        Fixture fixture;
        public MinerRigTests(Fixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        void MinerRigBuilderAddMinerConfigReturnsMinerRigWithMinerConfig()
        {
            var a = fixture.minerConfigDict;
            var b = MinerRigBuilder.CreateNew()
                        .AddMinerConfigs(fixture.minerConfigDict)
                        .Build();
            Assert.Equal(a, b.Miners);
        }

        [Fact]
        void MinerRigBuilderReturnsMinerRigT()
        {
            var a = MinerRigBuilder.CreateNew()
                        .Build();
            Assert.IsType<MinerRig>(a);
        }
    }
    public class MinerFarmTests : IClassFixture<Fixture> {
        Fixture fixture;
        public MinerFarmTests(Fixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        void MinerFarmFileConvertsToMinerFarmType()
        {
            var fs = new FileStream(fixture.tempFileWithMinerRigs, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            using(var streamReader = new StreamReader(fs)) { JsonSerializer serializer = new JsonSerializer();
                //ToDo decide to keep or delete MinerConfigIDT 
                //ToDo decide to keep or delete MinerRigIDT 
                //var farm = (Dictionary<MinerRigIDT, Dictionary<MinerConfigIDT, MinerConfig>>)serializer.Deserialize(streamReader, typeof(Dictionary<MinerRigIDT, Dictionary<MinerConfigIDT, MinerConfig>>));
                var farm = (Dictionary<string, Dictionary<string, MinerConfig>>)serializer.Deserialize(streamReader, typeof(Dictionary<string, Dictionary<string, MinerConfig>>));
            }
        }
    

        [Fact]
        void MinerRigBuilderReturnsMinerRigT()
        {
            var a = MinerRigBuilder.CreateNew()
                        .Build();
            Assert.IsType<MinerRig>(a);
        }
    }
}
