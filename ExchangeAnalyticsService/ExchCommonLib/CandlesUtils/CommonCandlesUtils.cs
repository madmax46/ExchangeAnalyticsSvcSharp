using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using ExchCommonLib.Classes;
using ExchCommonLib.Enums;

namespace ExchCommonLib.CandlesUtils
{
    public static class CommonCandlesUtils
    {

        public static long DateTimeToUnix(DateTime dateTime)
        {
            var dt = DateTime.SpecifyKind(dateTime, DateTimeKind.Local);
            return ((DateTimeOffset)dt).ToUnixTimeSeconds();
        }

        public static DateTime FromUnixToDateTime(long date)
        {
            var dtOffset = DateTimeOffset.FromUnixTimeSeconds(date);
            dtOffset = dtOffset.ToLocalTime();
            return dtOffset.DateTime;
        }



        public static List<Candle> AggregateMinCandlesToInterval(List<Candle> candles, CandlesInterval interval = CandlesInterval.Min)
        {

            Dictionary<double, List<Candle>> grouped;
            if (interval == CandlesInterval.Month)
            {
                grouped = candles.GroupBy(r => (double)(r.Date.Year * 12 + r.Date.Month))
                    .Select(r => new { key = r.Key, val = r.ToList() }).ToDictionary(r => r.key, r => r.val);
            }
            else
            {
                var periodInt = GetIntervalEquivalentInMinutes(interval);

                grouped = candles.GroupBy(r => Math.Truncate((TimeSpan.FromTicks(r.Date.Ticks)).TotalMinutes / periodInt))
                    .Select(r => new { key = r.Key, val = r.ToList() }).ToDictionary(r => r.key, r => r.val);
            }

            var groupedCandles = new List<Candle>();
            foreach (var oneGroup in grouped)
            {
                var groupedCandle = GetCandleFromGroupedCandles(oneGroup.Value);
                groupedCandle.Interval = interval;
                groupedCandles.Add(groupedCandle);
            }

            return groupedCandles;
        }


        public static Candle GetCandleFromGroupedCandles(List<Candle> candles)
        {
            candles = candles.OrderBy(r => r.Date).ToList();
            double openPrice = candles.First().Open;
            double closePrice = candles.Last().Close;
            double maxPrice = candles.Max(r => r.High);
            double minPrice = candles.Min(r => r.Low);
            ulong volume = candles.Select(r => r.Volume).Aggregate((a, b) => a + b);
            DateTime minDate = GetTimeWithoutSeconds(candles.Min(r => r.Date));

            var calcCandle = new Candle()
            {
                Open = openPrice,
                Close = closePrice,
                Low = minPrice,
                High = maxPrice,
                Volume = volume,
                Date = minDate
            };

            return calcCandle;
        }

        public static bool IsWeekend(DateTime date)
        {

            if (date.DayOfWeek == DayOfWeek.Saturday)
                return true;

            if (date.DayOfWeek == DayOfWeek.Sunday)
                return true;

            return false;
        }

        public static DateTime GetTimeWithoutSeconds(DateTime source)
        {
            return new DateTime(source.Year, source.Month, source.Day, source.Hour, source.Minute, 0, source.Kind);
        }


        private static int GetIntervalEquivalentInMinutes(CandlesInterval interval)
        {
            switch (interval)
            {
                case CandlesInterval.Min:
                    return 1;
                case CandlesInterval.FiveMin:
                    return 5;
                case CandlesInterval.FifteenMin:
                    return 15;
                case CandlesInterval.ThirtyMin:
                    return 30;
                case CandlesInterval.Hour:
                    return 60;
                case CandlesInterval.Day:
                    return 1440;
                case CandlesInterval.Week:
                    return 10080;
                default:
                    return 1;
            }
        }
        //public static bool IsAllowFreeSpaceOnDisk(double minFreeSpace, string diskName)
        //{
        //    bool isAllpow = false;
        //    System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();
        //    foreach (System.IO.DriveInfo drive in drives)
        //    {
        //        if (drive.IsReady)
        //        {
        //            if (drive.Name == diskName)
        //            {
        //                var freeSpaceOnDisk = BytesCountToGb(drive.AvailableFreeSpace);
        //                if (freeSpaceOnDisk > minFreeSpace)
        //                    isAllpow = true;
        //            }
        //        }
        //    }
        //    return isAllpow;
        //}
        //public static double BytesCountToGb(long bytesCount)
        //{
        //    return Convert.ToDouble(bytesCount) / 1024 / 1024 / 1024;
        //}
    }
}
