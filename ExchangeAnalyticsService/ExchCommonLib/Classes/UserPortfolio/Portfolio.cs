using System;
using System.Collections.Generic;
using System.Text;

namespace ExchCommonLib.Classes.UserPortfolio
{
    public class Portfolio
    {

        public List<PortfolioItem> Positions { get; set; }

        public double TotalAmount { get; set; }

        public Portfolio()
        {
            Positions = new List<PortfolioItem>();
        }
    }
}
