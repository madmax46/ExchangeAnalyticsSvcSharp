using System.Collections.Generic;
using System.Linq;
using ExchCommonLib.Classes;

namespace ExchCommonLib.Analytics
{
    public class ConsolidateInstrumentsAnalysis
    {

        public string Summary { get; private set; }
        public uint SellCnt { get; }
        public uint BuyCnt { get; }
        public uint NeutralCnt { get; }


        public static Dictionary<int, string> DecisionStr = new Dictionary<int, string>()
        {
            {1, "Активно продавать"},
            {2, "Продавать"},
            {3, "Держать"},
            {4, "Покупать"},
            {5, "Активно покупать"},
        };

        public ConsolidateInstrumentsAnalysis(List<AnalyticalPredictionInfo> predictions)
        {


            var sellCnt = predictions.Count(r => r.PredictionDecision == "Продавать" || r.PredictionDecision == "Sell");
            var buyCnt = predictions.Count(r => r.PredictionDecision == "Покупать" || r.PredictionDecision == "Buy");
            var neutralCnt = predictions.Count(r => r.PredictionDecision == "Держать" || r.PredictionDecision == "Neutral");

            SellCnt = (uint)sellCnt;
            BuyCnt = (uint)buyCnt;
            NeutralCnt = (uint)neutralCnt;

            Summary = GetConsolidateDecision(SellCnt, BuyCnt, NeutralCnt);
        }
        public ConsolidateInstrumentsAnalysis(uint sellCnt, uint buyCnt, uint neutralCnt)
        {
            SellCnt = sellCnt;
            BuyCnt = buyCnt;
            NeutralCnt = neutralCnt;

            Summary = GetConsolidateDecision(SellCnt, BuyCnt, NeutralCnt);
        }


        //public static string GetConsolidateDecision(List<AnalyticalPredictionInfo> predictionInfos)
        //{
        //    if (!predictionInfos.Any())
        //        return "--";


        //}


        public static string GetConsolidateDecision(uint sellCnt, uint buyCnt, uint neutralCnt)
        {
            var allClassesCnt = buyCnt + sellCnt + neutralCnt;
            var classSum = (int)buyCnt - sellCnt;

            var intervalClass = 2 * (double)allClassesCnt / 5;

            var leftInterval = -1d * allClassesCnt;
            var rightInterval = leftInterval + intervalClass;
            for (var i = 1; i <= 5; i++)
            {
                if (classSum >= leftInterval && classSum <= rightInterval)
                    return DecisionStr[i];

                leftInterval += intervalClass;
                rightInterval += intervalClass;
            }

            return DecisionStr[3];
        }


    }
}
