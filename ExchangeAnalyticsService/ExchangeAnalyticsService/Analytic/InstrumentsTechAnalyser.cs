using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeAnalyticsService.IRepositories;
using ExchangeAnalyticsService.Models.Analytic;
using ExchangeAnalyticsService.Repositories;
using ExchangeAnalyticsService.Services.Interfaces;
using ExchCommonLib.Classes;
using ExchCommonLib.Classes.Requests;
using TechAnalysisAlgLib.BidDecision;
using TechAnalysisAlgLib.Enums;
using TechAnalysisAlgLib.Indicators.MovingAverage;
using TechAnalysisAlgLib.Indicators.Oscillators;
using TechAnalysisAlgLib.Interfaces;
using TechAnalysisAlgLib.Series;

namespace ExchangeAnalyticsService.Analytic
{
    public class InstrumentsTechAnalyser
    {
        private readonly IParsersRepository parsersRepository;
        private readonly ITechMethodsRepository techMethodsRepository;
        private readonly ICandlesService candlesService;


        private readonly List<uint> movingAveragePeriods = new List<uint>() { 5, 10, 20, 30, 50, 100, 200 };

        private List<IIndicator<List<double?>>> movingAverageInstances = new List<IIndicator<List<double?>>>();
        private List<IIndicator<List<double?>>> simpleResOscillatorsInstances = new List<IIndicator<List<double?>>>();

        private readonly Ichimoku ichimokuIndicator = new Ichimoku();
        private readonly Macd macdIndicator = new Macd();
        private readonly WilliamsPercentRange wprIndicator = new WilliamsPercentRange();

        public InstrumentsTechAnalyser(IParsersRepository parsersRepository, ITechMethodsRepository techMethodsRepository, ICandlesService candlesService)
        {
            this.parsersRepository = parsersRepository;
            this.techMethodsRepository = techMethodsRepository;
            this.candlesService = candlesService;




            var techMethods = techMethodsRepository.GetTechMethodsInfoFromDb();
            InitMovingAverages(techMethods);
            InitOscillators(techMethods);
        }


        private void InitMovingAverages(List<AnalyticTechMethodsDbInfo> techMethods)
        {
            if (movingAverageInstances == null)
                movingAverageInstances = new List<IIndicator<List<double?>>>();

            movingAveragePeriods.ForEach(r => movingAverageInstances.Add(new SimpleMovingAverage(r)));
            movingAveragePeriods.ForEach(r => movingAverageInstances.Add(new ExponentialMovingAverage(r)));
            movingAveragePeriods.ForEach(r => movingAverageInstances.Add(new WeightedMovingAverage(r)));

            foreach (var oneMovingAvg in movingAverageInstances)
            {
                var curTechName = techMethods.FirstOrDefault(r => r.ShortName == oneMovingAvg.IdName);
                if (curTechName == null)
                    continue;

                oneMovingAvg.FullDescription = curTechName.Description;
            }

            ichimokuIndicator.FullDescription =
                techMethods.FirstOrDefault(r => r.ShortName == ichimokuIndicator.IdName)?.Description;
        }

        private void InitOscillators(List<AnalyticTechMethodsDbInfo> techMethods)
        {
            if (simpleResOscillatorsInstances == null)
                simpleResOscillatorsInstances = new List<IIndicator<List<double?>>>();

            simpleResOscillatorsInstances.Add(new RelativeStrengthIndex());
            simpleResOscillatorsInstances.Add(new AwesomeOscillator());
            simpleResOscillatorsInstances.Add(new Momentum());
            simpleResOscillatorsInstances.Add(new BearsPower());

            foreach (var oneOscillator in simpleResOscillatorsInstances)
            {
                var curTechName = techMethods.FirstOrDefault(r => r.ShortName == oneOscillator.IdName);
                if (curTechName == null)
                    continue;

                oneOscillator.FullDescription = curTechName.Description;
            }

            macdIndicator.FullDescription =
                techMethods.FirstOrDefault(r => r.ShortName == macdIndicator.IdName)?.Description;

            wprIndicator.FullDescription =
                techMethods.FirstOrDefault(r => r.ShortName == wprIndicator.IdName)?.Description;
        }


        public List<AnalyticalPredictionInfo> AnalyseInstrument(uint instrumentId, string interval)
        {
            var candleRequest = new CandlesRequest()
            {
                DateStart = new DateTime(2000, 1, 1),
                DateEnd = new DateTime(2021, 1, 1),
                InstrumentId = instrumentId,
                Interval = interval
            };

            var candles = candlesService.GetCandles(candleRequest).Candles;

            if (!candles.Any())
                return null;

            var movingAvgPredictions = AnalyseMovingAverages(candles);
            var oscillatorsPredictions = AnalyseOscillators(candles);

            return movingAvgPredictions.Concat(oscillatorsPredictions).ToList();
        }


        private List<AnalyticalPredictionInfo> AnalyseMovingAverages(List<Candle> candles)
        {
            var closePrices = candles.Select(r => r.Close).ToList();
            var highPrices = candles.Select(r => r.High).ToList();
            var lowPrices = candles.Select(r => r.Low).ToList();

            var calculatedMovingAverages = new Dictionary<IIndicator<List<double?>>, SimpleSeries>();
            var movingAveragesDecision = new List<AnalyticalPredictionInfo>();

            foreach (var oneMovingAverage in movingAverageInstances)
            {
                if (oneMovingAverage.Period > closePrices.Count)
                {
                    calculatedMovingAverages.Add(oneMovingAverage, null);

                    //сохранять null нужно
                    continue;
                }


                var values = oneMovingAverage.Calculate(closePrices);

                var series = new SimpleSeries() { SeriesValues = values };

                var decision = MovingAverageDecisionAnalyser.MakeDecision(series);
                var lastValue = values.Last();
                movingAveragesDecision.Add(new AnalyticalPredictionInfo(oneMovingAverage.FullDescription, lastValue, decision.ToString(), (uint)oneMovingAverage.IndicatorType));
                calculatedMovingAverages.Add(oneMovingAverage, series);
            }


            var ichimokuRes = ichimokuIndicator.Calculate(highPrices, lowPrices, closePrices);
            movingAveragesDecision.Add(new AnalyticalPredictionInfo(ichimokuIndicator.FullDescription, ichimokuRes.ConversionLine.Last(), "Neutral", (uint)ichimokuIndicator.IndicatorType));


            return movingAveragesDecision;
        }

        private List<AnalyticalPredictionInfo> AnalyseOscillators(List<Candle> candles)
        {
            var closePrices = candles.Select(r => r.Close).ToList();

            //var calculatedMovingAverages = new Dictionary<IIndicator<List<double?>>, SimpleSeries>();
            var oscillatorsDecision = new List<AnalyticalPredictionInfo>();



            foreach (var oneMovingAverage in simpleResOscillatorsInstances)
            {
                //if (oneMovingAverage.Period > closePrices.Count)
                //    calculatedMovingAverages.Add(oneMovingAverage, null);


                var values = oneMovingAverage.Calculate(closePrices);

                var series = new SimpleSeries() { SeriesValues = values };

                var decision = MakeOscillatorsDecision(oneMovingAverage, series);
                var lastValue = values.Last();
                oscillatorsDecision.Add(new AnalyticalPredictionInfo(oneMovingAverage.FullDescription, lastValue, decision.ToString(), (uint)oneMovingAverage.IndicatorType));

                //calculatedMovingAverages.Add(oneMovingAverage, series);
            }

            return oscillatorsDecision;
        }


        private AnalysisDecision MakeOscillatorsDecision(IIndicator<List<double?>> indicator, SimpleSeries simpleSeries)
        {

            switch (indicator.IdName.Split(' ').First())
            {
                case "RSI": return AnalysisDecision.Buy;
                case "AO": return AnalysisDecision.Buy;
                case "Bearpower": return AnalysisDecision.Buy;
                case "MOM": return AnalysisDecision.Buy;
                default: return AnalysisDecision.Neutral;
            }
        }
    }
}
