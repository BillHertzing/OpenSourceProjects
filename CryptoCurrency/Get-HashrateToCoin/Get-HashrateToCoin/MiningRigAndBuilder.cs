using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ATAP.CryptoCurrency {
    public enum MinerSWE
    {
        //[LocalizedDescription("ethmine", typeof(Resource))]
        [Description("ethmine")]
        ethmine,
        [Description("claymore")]
        claymore,
        [Description("genoil")]
        genoil,
    }

    public class PowerConsumption 
    {
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
public interface IPowerConsumption
{
    PowerConsumption PowerConsumption { get; set; }
}
    public class MinerConfig : IHashRates, IPowerConsumption , IFees
    {
        Dictionary<CoinsE, List<HashRate>> hashRates;
        public Dictionary<CoinsE, List<HashRate>> HashRates { get => hashRates; set => hashRates = value; }
        PowerConsumption powerConsumption;
        public PowerConsumption PowerConsumption { get => powerConsumption; set => powerConsumption = value; }

        double feeAsAPercent;
        public double FeeAsAPercent { get => feeAsAPercent; set => feeAsAPercent = value; }

        public MinerConfig(Dictionary<CoinsE, List<HashRate>> hashRates, PowerConsumption powerConsumption,double feeAsAPercent)
        {
            this.hashRates = hashRates;
            this.powerConsumption = powerConsumption;
            this.feeAsAPercent = feeAsAPercent;
        }

    }
    public interface IMinerConfigBuilder {
        MinerConfig Build();
    }
    public class MinerConfigBuilder : IMinerConfigBuilder
    {
        double feeAsAPercent;
        Dictionary<CoinsE, List<HashRate>> hashRates;
        public Dictionary<CoinsE, List<HashRate>> HashRates { get => hashRates; set => hashRates = value; }
        PowerConsumption powerConsumption;
        public PowerConsumption PowerConsumption { get => powerConsumption; set => powerConsumption = value; }
        public MinerConfigBuilder()       {        }
        public IMinerConfigBuilder AddHashRates(Dictionary<CoinsE, List<HashRate>> hashRates)
        {
            this.hashRates = hashRates;
            return this;
        }
        public IMinerConfigBuilder AddPowerConsumption(PowerConsumption powerConsumption)
        {
            this.powerConsumption = powerConsumption;
            return this;
        }
        public IMinerConfigBuilder AddFees(double feeAsAPercent)
        {
            this.feeAsAPercent = feeAsAPercent;
            return this;
        }
        public MinerConfig Build()
        {
            return new MinerConfig(hashRates, powerConsumption, feeAsAPercent);
        }
        public static MinerConfigBuilder CreateNew()
        {
            return new MinerConfigBuilder();
        }
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
