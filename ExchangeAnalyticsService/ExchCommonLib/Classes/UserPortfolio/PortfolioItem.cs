using System;
using System.Collections.Generic;
using System.Text;

namespace ExchCommonLib.Classes.UserPortfolio
{
    public class PortfolioItem
    {
        public uint InstrumentId { get; set; }
        public string Name { get; set; }
        public int RemCount { get; set; } //количество в портфеле
        public double AvgPrice { get; set; }

        public double? CurPrice { get; set; }
        public double CurСost { get; set; }

        public double? Income { get; set; } //доход

        public string Analytics { get; set; }


    }
}
