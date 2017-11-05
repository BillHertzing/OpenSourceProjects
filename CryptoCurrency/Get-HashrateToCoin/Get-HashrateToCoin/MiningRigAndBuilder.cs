using System;
using System.Collections.Generic;
using System.ComponentModel;
namespace ATAP.CryptoCurrency {
    public enum MinerSWE {
        //[LocalizedDescription("ethmine", typeof(Resource))]
        //[LocalizedDescription("ethmine", typeof(Resource))]
        //[LocalizedDescription("ethmine", typeof(Resource))]
        //[LocalizedDescription("ethmine", typeof(Resource))]
        [Description("ethmine")]
        ETHMINE,
        [Description("claymore")]
        CLAYMORE,
        [Description("genoil")]
        GENOIL,
    }
    public class PowerConsumption {
        double powerConsumed;
        int powerConsumedUOM;
        public PowerConsumption(double powerConsumed, int powerConsumedUOM)
        {
            this.powerConsumed = powerConsumed;
            this.powerConsumedUOM = powerConsumedUOM;
        }
        public double PowerConsumed { get => powerConsumed; set => powerConsumed = value; }
        public int PowerConsumedUOM { get => powerConsumedUOM; set => powerConsumedUOM = value; }
    }
    public interface IPowerConsumption {
        PowerConsumption PowerConsumption { get; set; }
    }
    public interface IFees
    {
        Fees Fees { get; set; }
    }
    public class MinerConfig : IHashRates, IPowerConsumption, IFees {
        Fees fees;
        Dictionary<CoinsE, List<HashRate>> hashRates;
        PowerConsumption powerConsumption;
        public MinerConfig(Dictionary<CoinsE, List<HashRate>> hashRates, PowerConsumption powerConsumption, Fees fees)
        {
            this.hashRates = hashRates;
            this.powerConsumption = powerConsumption;
            this.fees = fees;
        }
        public Fees Fees { get => fees; set => fees = value; }
        public Dictionary<CoinsE, List<HashRate>> HashRates { get => hashRates; set => hashRates = value; }
        public PowerConsumption PowerConsumption { get => powerConsumption; set => powerConsumption = value; }
    }
    public interface IMinerConfigBuilder {
        MinerConfig Build();
    }
    public class MinerConfigBuilder  {
        Fees fees;
        Dictionary<CoinsE, List<HashRate>> hashRates;
        PowerConsumption powerConsumption;
        public MinerConfigBuilder() { }
        public MinerConfigBuilder AddFees(Fees fees)
        {
            this.fees = fees;
            return this;
        }
        public MinerConfigBuilder AddHashRates(Dictionary<CoinsE, List<HashRate>> hashRates)
        {
            this.hashRates = hashRates;
            return this;
        }
        public MinerConfigBuilder AddPowerConsumption(PowerConsumption powerConsumption)
        {
            this.powerConsumption = powerConsumption;
            return this;
        }
        public MinerConfig Build()
        {
            return new MinerConfig(hashRates, powerConsumption, fees);
        }
        public static MinerConfigBuilder CreateNew()
        {
            return new MinerConfigBuilder();
        }
        public Dictionary<CoinsE, List<HashRate>> HashRates { get => hashRates; set => hashRates = value; }
        public PowerConsumption PowerConsumption { get => powerConsumption; set => powerConsumption = value; }
    }
    public class MinerConfigIDT {
        string iD;
        public MinerConfigIDT(string id)
        {
            iD = id;
        }
        public string ID { get { return iD; }
            set { iD = value; } }
    }
    public class MiningRig {
        Dictionary<MinerConfigIDT, MinerConfig> minerConfigs;
        public MiningRig(Dictionary<MinerConfigIDT, MinerConfig> minerConfigs)
        {
            this.minerConfigs = minerConfigs;
        }
        public Dictionary<MinerConfigIDT, MinerConfig> Miners { get => minerConfigs; set => minerConfigs = value; }
    }
    public interface IMiningRigBuilder {
        MiningRig Build();
    }
    public class MiningRigBuilder : IMiningRigBuilder {
        Dictionary<MinerConfigIDT, MinerConfig> minerConfigs;
        public MiningRigBuilder() { }
        public IMiningRigBuilder AddMinerConfigs(Dictionary<MinerConfigIDT, MinerConfig> minerConfigs)
        {
            this.minerConfigs = minerConfigs;
            return this;
        }
        public MiningRig Build()
        {
            return new MiningRig(minerConfigs);
        }
        public static MiningRigBuilder CreateNew()
        {
            return new MiningRigBuilder();
        }
    }
}
