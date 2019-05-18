using System;
using System.Collections.Generic;
using System.Text;

namespace ExchCommonLib.Classes.Exchange
{
    public class ExchangeMarkets
    {
        public List<Market> MarketList { get; set; }

        public ExchangeMarkets()
        {
            MarketList = new List<Market>();
        }

    }
}
