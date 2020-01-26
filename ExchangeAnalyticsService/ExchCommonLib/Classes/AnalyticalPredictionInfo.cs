namespace ExchCommonLib.Classes
{
    public class AnalyticalPredictionInfo
    {

        public string MethodName { get; set; }
        public uint IndicatorType { get; set; }

        public double? Value { get; set; }


        public string PredictionDecision { get; set; }

        public AnalyticalPredictionInfo(string methodName, double? value, string predictionDecision, uint indicatorType)
        {
            MethodName = methodName;
            Value = value;
            PredictionDecision = predictionDecision;
            IndicatorType = indicatorType;
        }
    }
}
