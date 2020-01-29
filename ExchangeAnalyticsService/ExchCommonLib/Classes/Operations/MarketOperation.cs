using System;
using ExchCommonLib.Enums;

namespace ExchCommonLib.Classes.Operations
{
    public class MarketOperation
    {
        public uint Id { get; set; }
        public uint InstrumentId { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
        public OperationType OrderType { get; set; }
        public DateTime Date { get; set; }
    }
}
