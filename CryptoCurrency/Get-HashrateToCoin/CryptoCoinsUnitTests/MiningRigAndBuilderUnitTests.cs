using System;
using System.Collections.Generic;
using Xunit;
namespace ATAP.CryptoCurrency.MiningRigAndBuilderUnitTests {
    public class MinerPrimitives {
        TimeSpan timeSpan;
        double hashRateValue;
        int hashRateUOM ;
        HashRate hashRate;
        HashRate hashRate2X;
        HashRate hashRate2T;
        HashRate hashRate1000X;
        double powerConsumptionValue;
        int powerConsumptionUOM;
        PowerConsumption powerConsumption;
        double feeAsAPercent;
        Fees fees;

        public TimeSpan TimeSpan { get => timeSpan; set => timeSpan = value; }
        public HashRate HashRate { get => hashRate; set => hashRate = value; }
        public HashRate HashRate2X { get => hashRate2X; set => hashRate2X = value; }
        public HashRate HashRate2T { get => hashRate2T; set => hashRate2T = value; }
        public double PowerConsumptionValue { get => powerConsumptionValue; set => powerConsumptionValue = value; }
        public int PowerConsumptionUOM { get => powerConsumptionUOM; set => powerConsumptionUOM = value; }
        public double HashRateValue { get => hashRateValue; set => hashRateValue = value; }
        public int HashRateUOM { get => hashRateUOM; set => hashRateUOM = value; }
        public PowerConsumption PowerConsumption { get => powerConsumption; set => powerConsumption = value; }
        public double FeeAsAPercent { get => feeAsAPercent; set => feeAsAPercent = value; }
        public Fees Fees { get => fees; set => fees = value; }

        public MinerPrimitives()
        {
            timeSpan = new TimeSpan(0, 0, 1);
            hashRateValue = 10000;
            hashRateUOM = 1;
            hashRate = new HashRate(hashRateValue, hashRateUOM, timeSpan);
            hashRate2X = new HashRate(hashRateValue * 2, hashRateUOM, timeSpan);
            hashRate2T = new HashRate(hashRateValue, hashRateUOM, timeSpan + timeSpan);
            hashRate1000X = new HashRate(hashRateValue, hashRateUOM * 1000, timeSpan);
            powerConsumptionValue = 1;
            powerConsumptionUOM = 1000;
            powerConsumption = new PowerConsumption(powerConsumptionValue, powerConsumptionUOM);
            feeAsAPercent = 1.0;
            fees = new Fees(FeeAsAPercent);


        }
    }
    public class MinerConfigPrimitives : MinerPrimitives
    {
        MinerConfig minerConfig;
        MinerConfigIDT minerConfigID;
        Dictionary<MinerConfigIDT, MinerConfig> minerConfigDict;
        public MinerConfigPrimitives()
        {
            minerConfig = MinerConfigBuilder.CreateNew().Build();
            minerConfigID = new MinerConfigIDT("1");
            minerConfigDict = new Dictionary<MinerConfigIDT, MinerConfig> { { minerConfigID, minerConfig } };
            //MinerConfig minerConfig = MinerConfigBuilder.CreateNew().Add(MinerPrimitives.PowerConsumption).Build;
            //Dictionary<MinerIDT, Miner> MinerDict = new Dictionary<MinerIDT, Miner>().Add("Miner1", new Miner());
            //MiningRigs = new List<MiningRig>(MiningRigBuilder.CreateNew().AddMiners(new Dictionary < MinerIDT, Miner > ("Miner1",MinerBuilder.CreateNew().AddMinerConfigs()))).Build());
        }
    }
    public class MinerConfigTests : IClassFixture<MinerPrimitives>
    {
        MinerPrimitives minerPrimitives;
        public MinerConfigTests(MinerPrimitives minerPrimitives)
        {
            this.minerPrimitives = minerPrimitives;
        }

       


        [Fact]
        void MinerConfigBuilderReturnsMinerConfigT()
        {
            var a = MinerConfigBuilder.CreateNew().Build();
            Assert.IsType(typeof(MinerConfig), a);
        }
        [Fact]
        void MinerConfigBuilderAddPowerConsumptionReturnsMinerConfigWithPowerConsumption()
        {
            var p = minerPrimitives.PowerConsumption;
            var a = MinerConfigBuilder.CreateNew().AddPowerConsumption(p).Build();
            Assert.Equal(p, a.PowerConsumption);
        }
        [Fact]
        void MinerConfigBuilderAddFeesReturnsMinerConfigWithFees()
        {
            var f = minerPrimitives.Fees;
            var a = MinerConfigBuilder.CreateNew().AddFees(f).Build();
            Assert.Equal(f.FeeAsAPercent, a.Fees.FeeAsAPercent);
        }
        void MinerConfigBuilderAddHashRateReturnsMinerConfigWithHashRate()
        {
            var h = minerPrimitives.FeeAsAPercent;
            var a = MinerConfigBuilder.CreateNew().AddFees(minerPrimitives.Fees).Build();
            Assert.Equal(minerPrimitives.FeeAsAPercent, a.Fees.FeeAsAPercent);
        }
    }
    public class MiningRigTests : IClassFixture<MinerConfigPrimitives>
    {
        MinerConfigPrimitives minerConfigPrimitives;
        public MiningRigTests(MinerConfigPrimitives minerConfigPrimitives)
        {
            this.minerConfigPrimitives = minerConfigPrimitives;
        }

        [Fact]
        void MiningRigBuilderReturnsMiningRigObjectT()
        {
            var a = MiningRigBuilder.CreateNew().Build();
            Assert.IsType(typeof(MiningRig), a);
        }
        //[Fact]
        //void MiningRigBuilderReturnsMiningRigObjectWithMinersDictionary()
        //{
        //    var a = MiningRigBuilder.CreateNew().AddMiners(miningRigPrimitives.minersCollection).Build();
        //    Assert.IsType(typeof(Dictionary<MinerIDT, Miner>), a.Miners);
        //}
    }
}
