﻿using System;
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
using TechAnalysisAlgLib.Ml;
using TechAnalysisAlgLib.Series;

namespace ExchangeAnalyticsService.Analytic
{
    public class InstrumentsTechAnalyser
    {
        private readonly IParsersRepository parsersRepository;
        private readonly ITechMethodsRepository techMethodsRepository;
        private readonly ICandlesService candlesService;


        private readonly List<uint> movingAveragePeriods;

        private List<IIndicator<List<double?>>> movingAverageInstances;
        private List<IIndicator<List<double?>>> simpleResOscillatorsInstances;

        private readonly Ichimoku ichimokuIndicator;
        private readonly Macd macdIndicator;
        private readonly WilliamsPercentRange wprIndicator;

        public InstrumentsTechAnalyser(IParsersRepository parsersRepository, ITechMethodsRepository techMethodsRepository, ICandlesService candlesService)
        {
            this.parsersRepository = parsersRepository;
            this.techMethodsRepository = techMethodsRepository;
            this.candlesService = candlesService;

            movingAveragePeriods = new List<uint>() { 5, 10, 20, 30, 50, 100, 200 };
            movingAverageInstances = new List<IIndicator<List<double?>>>();
            simpleResOscillatorsInstances = new List<IIndicator<List<double?>>>();
            ichimokuIndicator = new Ichimoku();
            macdIndicator = new Macd();
            wprIndicator = new WilliamsPercentRange();
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

            var dateStart = DateTime.Now.AddDays(-5);
            if (interval == "day")
                dateStart = DateTime.Now.AddDays(-50);

            if (interval == "week")
                dateStart = DateTime.Now.AddDays(-400);

            if (interval == "month")
                dateStart = DateTime.Now.AddMonths(-50);

            var candleRequest = new CandlesRequest()
            {
                DateStart = dateStart,
                DateEnd = DateTime.Now.AddDays(5),
                InstrumentId = instrumentId,
                Interval = interval
            };

            var candles = candlesService.GetCandles(candleRequest).Candles;

            if (!candles.Any())
                return null;


            var mlRes = AnalyseMl(candles);
            var indicators = AnalyseTechIndicators(candles);

            return mlRes.Concat(indicators).ToList();
        }

        private List<AnalyticalPredictionInfo> AnalyseMl(List<Candle> candles)
        {

            var subList = candles.TakeLast(50).ToList();
            var closePrice = subList.Select(r => r.High).ToList();
            var x = new List<double>();
            for (double i = 1; i <= subList.Count; i += 1)
            {
                x.Add(i);
            }

            InterpolateMnk mnk = new InterpolateMnk(x, closePrice);
            mnk.Mnk(subList.Count - 1);
            var value = mnk.Interpolate(subList.Count + 0.001);

            var lastCandle = candles.Last();
            if (!double.IsNormal(value))
            {
                value = lastCandle.Close;
            }
            else
            {
                value = Math.Round(value, 2);
            }

            var decision = MlDecisionAnalyser.MakeDecision(value, lastCandle.Close);
            var info = new AnalyticalPredictionInfo("Искусственная нейронная сеть LSTM", value, decision.ToString(), (uint)TechMethodType.MlMethod);
            return new List<AnalyticalPredictionInfo>() { info };
        }

        private List<AnalyticalPredictionInfo> AnalyseTechIndicators(List<Candle> candles)
        {
            var closePrices = candles.Select(r => r.Close).ToList();
            var highPrices = candles.Select(r => r.High).ToList();
            var lowPrices = candles.Select(r => r.Low).ToList();

            var movingAvgPredictions = AnalyseMovingAverages(closePrices, highPrices, lowPrices);
            var oscillatorsPredictions = AnalyseOscillators(closePrices, highPrices, lowPrices);

            return movingAvgPredictions.Concat(oscillatorsPredictions).ToList();
        }


        private List<AnalyticalPredictionInfo> AnalyseMovingAverages(List<double> closePrices, List<double> highPrices, List<double> lowPrices)
        {

            var calculatedMovingAverages = new Dictionary<IIndicator<List<double?>>, SimpleSeries>();
            var movingAveragesDecision = new List<AnalyticalPredictionInfo>();

            foreach (var oneMovingAverage in movingAverageInstances)
            {
                if (oneMovingAverage.Period > closePrices.Count)
                {
                    calculatedMovingAverages.Add(oneMovingAverage, null);

                    movingAveragesDecision.Add(new AnalyticalPredictionInfo(oneMovingAverage.FullDescription, null, AnalysisDecision.Unknown.ToString(), (uint)oneMovingAverage.TechMethodType));

                    //сохранять null нужно
                    continue;
                }

                SimpleSeries series = new SimpleSeries();
                double? lastValue = null;
                var decision = AnalysisDecision.Unknown;
                try
                {
                    var values = oneMovingAverage.Calculate(closePrices);

                    series = new SimpleSeries() { SeriesValues = values };

                    decision = MovingAverageDecisionAnalyser.MakeDecision(series);
                    lastValue = values.Last();
                }
                catch (Exception e)
                {

                }

                movingAveragesDecision.Add(new AnalyticalPredictionInfo(oneMovingAverage.FullDescription, lastValue, decision.ToString(), (uint)oneMovingAverage.TechMethodType));
                calculatedMovingAverages.Add(oneMovingAverage, series);
            }

            AnalyseIchimoku(highPrices, lowPrices, closePrices, movingAveragesDecision);


            return movingAveragesDecision;
        }


        private List<AnalyticalPredictionInfo> AnalyseOscillators(List<double> closePrices, List<double> highPrices, List<double> lowPrices)
        {

            //var calculatedMovingAverages = new Dictionary<IIndicator<List<double?>>, SimpleSeries>();
            var oscillatorsDecision = new List<AnalyticalPredictionInfo>();



            foreach (var oneMovingAverage in simpleResOscillatorsInstances)
            {
                //if (oneMovingAverage.Period > closePrices.Count)
                //    calculatedMovingAverages.Add(oneMovingAverage, null);
                double? lastValue = null;
                var decision = AnalysisDecision.Unknown;

                try
                {
                    var values = oneMovingAverage.Calculate(closePrices);

                    var series = new SimpleSeries() { SeriesValues = values };

                    decision = MakeOscillatorsDecision(oneMovingAverage, series);
                    lastValue = values.LastOrDefault();
                }
                catch (Exception e)
                {

                }
                oscillatorsDecision.Add(new AnalyticalPredictionInfo(oneMovingAverage.FullDescription, lastValue, decision.ToString(), (uint)oneMovingAverage.TechMethodType));

                //calculatedMovingAverages.Add(oneMovingAverage, series);
            }

            AnalyseMacd(closePrices, oscillatorsDecision);
            AnalyseWpr(closePrices, highPrices, lowPrices, oscillatorsDecision);
            return oscillatorsDecision;
        }

        private void AnalyseIchimoku(List<double> highPrices, List<double> lowPrices, List<double> closePrices, List<AnalyticalPredictionInfo> movingAveragesDecision)
        {
            IchimokuSeries ichimokuRes;
            var ichimokuDecision = AnalysisDecision.Unknown;
            try
            {
                ichimokuRes = ichimokuIndicator.Calculate(highPrices, lowPrices, closePrices);
                ichimokuDecision = IchimokuDecisionAnalyser.MakeDecision(ichimokuRes, closePrices);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            movingAveragesDecision.Add(new AnalyticalPredictionInfo(ichimokuIndicator.FullDescription,
                ichimokuRes?.BaseLine?.LastOrDefault(), ichimokuDecision.ToString(), (uint)ichimokuIndicator.TechMethodType));
        }

        private void AnalyseMacd(List<double> closePrices, List<AnalyticalPredictionInfo> oscillatorsDecision)
        {
            var macdRes = new MacdSeries();
            var macdDecision = AnalysisDecision.Unknown;
            try
            {
                macdRes = macdIndicator.Calculate(closePrices);
                macdDecision = MacdDecisionAnalyser.MakeDecision(macdRes);
            }
            catch (Exception e)
            {

            }

            oscillatorsDecision.Add(new AnalyticalPredictionInfo(macdIndicator.FullDescription,
                macdRes?.MacdLine?.LastOrDefault(), macdDecision.ToString(), (uint)macdIndicator.TechMethodType));
        }

        private void AnalyseWpr(List<double> closePrices, List<double> highPrices, List<double> lowPrices, List<AnalyticalPredictionInfo> oscillatorsDecision)
        {
            var series = new SimpleSeries();
            var wprDecision = AnalysisDecision.Unknown;
            try
            {
                var val = wprIndicator.Calculate(closePrices, highPrices, lowPrices);
                series.SeriesValues = val;
                wprDecision = WprDecisionAnalyser.MakeDecision(series);
            }
            catch (Exception e)
            {

            }

            oscillatorsDecision.Add(new AnalyticalPredictionInfo(wprIndicator.FullDescription,
                series?.SeriesValues?.LastOrDefault(), wprDecision.ToString(), (uint)wprIndicator.TechMethodType));
        }


        private AnalysisDecision MakeOscillatorsDecision(IIndicator<List<double?>> indicator, SimpleSeries simpleSeries)
        {
            switch (indicator.IdName.Split(' ').First())
            {
                case "RSI": return RsiDecisionAnalyser.MakeDecision(simpleSeries);
                case "AO": return AoDecisionAnalyser.MakeDecision(simpleSeries);
                case "Bearpower": return BearpowerDecisionAnalyser.MakeDecision(simpleSeries);
                case "MOM": return MomentumDecisionAnalyser.MakeDecision(simpleSeries);
                default: return AnalysisDecision.Unknown;
            }
        }
    }
}
