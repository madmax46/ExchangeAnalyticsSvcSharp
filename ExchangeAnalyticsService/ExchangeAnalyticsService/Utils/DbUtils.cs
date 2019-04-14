using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeAnalyticsService.Utils
{
    public static class DbUtils
    {
        private static object syncRoot = new object();

        private static MySqlWrap mariaDbWrapper;

        public static MySqlWrap MariaDbWrapper
        {
            get
            {
                if (mariaDbWrapper == null)
                {
                    lock (syncRoot)
                    {
                        if (mariaDbWrapper == null)
                        {
                            MySqlConfig config = new MySqlConfig()
                            {
                                Host = "localhost",
                                Port = 3307,
                                UserId = "root",
                                Password = "secret",
                                SslMode = "none",
                                Database = "stockquotes",
                                CharacterSet = "utf8"
                            };
                            mariaDbWrapper = new MySqlWrap(config);
                        }
                    }
                }

                return mariaDbWrapper;
            }
        }
    }
}
