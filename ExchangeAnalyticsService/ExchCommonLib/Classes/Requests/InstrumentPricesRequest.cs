using System;
using System.Collections.Generic;
using System.Text;

namespace ExchCommonLib.Classes.Requests
{
    public class InstrumentPricesRequest
    {
        public uint InstrumentId { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
    }
}
