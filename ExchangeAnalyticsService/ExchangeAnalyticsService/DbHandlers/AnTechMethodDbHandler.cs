using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using DbWrapperCore;
using ExchangeAnalyticsService.Models.Analytic;

namespace ExchangeAnalyticsService.DbHandlers
{
    public class AnTechMethodDbHandler : IDbDataHandler<List<AnalyticTechMethodsDbInfo>>
    {
        public List<AnalyticTechMethodsDbInfo> Execute(DbDataReader dataReader)
        {
            var values = new List<AnalyticTechMethodsDbInfo>();

            while (dataReader.Read())
            {
                var oneInfo = new AnalyticTechMethodsDbInfo()
                {
                    Id = Convert.ToUInt32(dataReader["id"]),
                    ShortName = Convert.ToString(dataReader["name"]),
                    Description = Convert.ToString(dataReader["description"])
                };

                values.Add(oneInfo);
            }


            return values;
        }
    }
}
