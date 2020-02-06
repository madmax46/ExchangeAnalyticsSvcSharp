using ExchCommonLib.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ExchCommonLib.Classes
{
    public class Candle
    {
        public DateTime Date { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public ulong Volume { get; set; }

        public CandlesInterval Interval { get; set; }
        public static Candle FromRow(DataRow oneRow)
        {
            Candle candle = new Candle();
            candle.Date = Convert.ToDateTime(oneRow["date"]);
            candle.Open = Math.Round(Convert.ToDouble(oneRow["open"]), 2);
            candle.High = Math.Round(Convert.ToDouble(oneRow["high"]), 2);
            candle.Low = Math.Round(Convert.ToDouble(oneRow["low"]), 2);
            candle.Close = Math.Round(Convert.ToDouble(oneRow["close"]), 2);
            candle.Volume = Convert.ToUInt64(oneRow["volume"]);
            candle.Interval = (CandlesInterval)Convert.ToUInt32(oneRow["intervalAgg"]);
            return candle;
        }
    }
}
