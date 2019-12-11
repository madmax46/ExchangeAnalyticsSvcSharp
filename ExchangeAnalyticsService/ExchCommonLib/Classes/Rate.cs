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
        public float Open { get; set; }
        public float High { get; set; }
        public float Low { get; set; }
        public float Close { get; set; }
        public ulong Volume { get; set; }

        public CandlesInterval Interval { get; set; }
        public static Candle FromRow(DataRow oneRow)
        {
            Candle candle = new Candle();
            candle.Date = Convert.ToDateTime(oneRow["date"]);
            candle.Open = Convert.ToSingle(oneRow["open"]);
            candle.High = Convert.ToSingle(oneRow["high"]);
            candle.Low = Convert.ToSingle(oneRow["low"]);
            candle.Close = Convert.ToSingle(oneRow["close"]);
            candle.Volume = Convert.ToUInt64(oneRow["volume"]);
            candle.Interval = CandlesInterval.Min;
            return candle;
        }
    }
}
