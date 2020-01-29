using MySqlWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeAnalyticsService.Utils
{
    public static class DbUtils
    {
        private static object syncRoot = new object();
        private static object syncRoot2 = new object();

        private static MySqlWrap mariaDbWrapper;
        private static MySqlWrap smAnalyticsDbWrapper;
        private static MySqlWrap userPortfolioDbWrapper;

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




        public static MySqlWrap SmAnalyticsDb
        {
            get
            {
                if (smAnalyticsDbWrapper == null)
                {
                    lock (syncRoot2)
                    {
                        if (smAnalyticsDbWrapper == null)
                        {
                            MySqlConfig config = new MySqlConfig()
                            {
                                Host = "89.208.196.51",
                                Port = 3306,
                                UserId = "root",
                                Password = "admin1234",
                                SslMode = "none",
                                Database = "smanalytics",
                                CharacterSet = "utf8"
                            };
                            smAnalyticsDbWrapper = new MySqlWrap(config);
                        }
                    }
                }

                return smAnalyticsDbWrapper;
            }
        }

        public static MySqlWrap UserPortfolioDb
        {
            get
            {
                if (userPortfolioDbWrapper == null)
                {
                    lock ("userPortfolioDbWrapper")
                    {
                        if (userPortfolioDbWrapper == null)
                        {
                            MySqlConfig config = new MySqlConfig()
                            {
                                Host = "89.208.196.51",
                                Port = 3306,
                                UserId = "root",
                                Password = "admin1234",
                                SslMode = "none",
                                Database = "users_portfolio",
                                CharacterSet = "utf8"
                            };
                            userPortfolioDbWrapper = new MySqlWrap(config);
                        }
                    }
                }

                return userPortfolioDbWrapper;
            }
        }

    }
}
