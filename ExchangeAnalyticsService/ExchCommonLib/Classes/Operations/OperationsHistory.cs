using System.Collections.Generic;

namespace ExchCommonLib.Classes.Operations
{
    public class OperationsHistory
    {
        public List<MarketOperation> Operations { get; set; }

        public OperationsHistory()
        {
            Operations = new List<MarketOperation>();
        }
    }
}
