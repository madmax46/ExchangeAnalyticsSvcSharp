using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ExchCommonLib.Classes
{
    public class Rate
    {
        public DateTime Date { get; set; }
        public float Open { get; set; }
        public float High { get; set; }
        public float Low { get; set; }
        public float Close { get; set; }
        public ulong Volume { get; set; }

        public static Rate FromRow(DataRow oneRow)
        {
            Rate rate = new Rate();
            rate.Date = Convert.ToDateTime(oneRow["date"]);
            rate.Open = Convert.ToSingle(oneRow["open"]);
            rate.High = Convert.ToSingle(oneRow["high"]);
            rate.Low = Convert.ToSingle(oneRow["low"]);
            rate.Close = Convert.ToSingle(oneRow["close"]);
            rate.Volume = Convert.ToUInt64(oneRow["volume"]);

            return rate;
        }
    }
}
