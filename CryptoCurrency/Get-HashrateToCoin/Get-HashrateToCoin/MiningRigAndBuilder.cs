using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace ATAP.CryptoCurrency
{
    public enum MinerSWE
    {
        //[LocalizedDescription("ethmine", typeof(Resource))]
        //[LocalizedDescription("ethmine", typeof(Resource))]
        //[LocalizedDescription("ethmine", typeof(Resource))]
        //[LocalizedDescription("ethmine", typeof(Resource))]
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
    public class HashRatesDict : IEnumerable<HashRate>, IHashRatesDict
    {
        Dictionary<CoinsE, HashRate> hashRates;
        public HashRatesDict(Dictionary<CoinsE, HashRate> hashRates)
        {
            this.hashRates = hashRates;
        }
        public Dictionary<CoinsE, HashRate> HashRates { get => hashRates; set => hashRates = value; }
        // to use a collection initializer, the collection must implement GetEnumerator in two different ways, 
        public IEnumerator<HashRate> GetEnumerator()
        {
            return hashRates.Values.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            //forces use of the non-generic implementation on the Values collection
            return ((IEnumerable)hashRates.Values).GetEnumerator();
        }
        // to use a collection initializer, the collection must implement Add 
        public void Add(CoinsE coinsE, HashRate hashRate)
        {
            hashRates.Add(coinsE, hashRate);
        }
        // to use a [] as an indexer the collection must implement the ITEM property
        public HashRate this[CoinsE key] { get { return hashRates[key]; } set { hashRates[key] = value; } }
    }
    public class MinerConfig : IHashRatesDict, IPowerConsumption, IFees
    {
        Fees fees;
        Dictionary<CoinsE, HashRate> hashRates;
        PowerConsumption powerConsumption;
        public MinerConfig(Dictionary<CoinsE, HashRate> hashRates, PowerConsumption powerConsumption, Fees fees)
        {
            this.hashRates = hashRates;
            this.powerConsumption = powerConsumption;
            this.fees = fees;
        }
        public Fees Fees { get => fees; set => fees = value; }
        public Dictionary<CoinsE, HashRate> HashRates { get => hashRates; set => hashRates = value; }
        public PowerConsumption PowerConsumption { get => powerConsumption; set => powerConsumption = value; }
    }
    public interface IMinerConfigBuilder
    {
        MinerConfig Build();
    }
    public class MinerConfigBuilder
    {
        Fees fees;
        Dictionary<CoinsE, HashRate> hashRates;
        PowerConsumption powerConsumption;
        public MinerConfigBuilder() { }
        public MinerConfigBuilder AddFees(Fees fees)
        {
            this.fees = fees;
            return this;
        }
        public MinerConfigBuilder AddHashRates(Dictionary<CoinsE, HashRate> hashRates)
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
        public Dictionary<CoinsE, HashRate> HashRates { get => hashRates; set => hashRates = value; }
        public PowerConsumption PowerConsumption { get => powerConsumption; set => powerConsumption = value; }
    }
    public class MinerRig
    {
        //ToDo decide to keep or delete MinerConfigIDT Dictionary<MinerConfigIDT, MinerConfig> minerConfigs;
        Dictionary<string, MinerConfig> minerConfigs;
        //ToDo decide to keep or delete MinerConfigIDT public MinerRig(Dictionary<MinerConfigIDT, MinerConfig> minerConfigs)
        public MinerRig(Dictionary<string, MinerConfig> minerConfigs)
        {
            this.minerConfigs = minerConfigs;
        }
        //ToDo decide to keep or delete MinerConfigIDT public Dictionary<MinerConfigIDT, MinerConfig> Miners { get => minerConfigs; set => minerConfigs = value; }
        public Dictionary<string, MinerConfig> Miners { get => minerConfigs; set => minerConfigs = value; }

    }
    public interface IMinerRigBuilder
    {
        MinerRig Build();
    }
    public class MinerRigBuilder : IMinerRigBuilder
    {
        //ToDo decide to keep or delete MinerConfigIDT Dictionary<MinerConfigIDT, MinerConfig> minerConfigs;
        Dictionary<string, MinerConfig> minerConfigs;
        public MinerRigBuilder() { }
        //ToDo decide to keep or delete MinerConfigIDT         public IMinerRigBuilder AddMinerConfigs(Dictionary<MinerConfigIDT, MinerConfig> minerConfigs)
        public IMinerRigBuilder AddMinerConfigs(Dictionary<string, MinerConfig> minerConfigs)
        {
            this.minerConfigs = minerConfigs;
            return this;
        }
        public MinerRig Build()
        {
            return new MinerRig(minerConfigs);
        }
        public static MinerRigBuilder CreateNew()
        {
            return new MinerRigBuilder();
        }
    }
}
