using ExchangeAnalyticsService.IRepositories;
using ExchangeAnalyticsService.Models;
using ExchCommonLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeAnalyticsService.Repositories
{
    public class AccountRepository : IAccountRepository
    {

        public IDBProvider dbProvider { get; private set; }


        public AccountRepository(IDBProvider dbProvider)
        {
            this.dbProvider = dbProvider;
        }


        public Person GetUserByLoginAndPassword(string login, string password)
        {
            var table = dbProvider.ProcedureByName("svc_check_auth", login, password);
            if (table.Rows.Count == 0)
                return null;

            var person = GetPersonFromRow(table.Rows[0]);
            return person;
        }


        private Person GetPersonFromRow(DataRow row)
        {
            Person person = new Person()
            {
                Login = Convert.ToString(row["login"]),
                FirstName = Convert.ToString(row["firstName"]),
                SecondName = Convert.ToString(row["secondName"]),
                EMail = Convert.ToString(row["email"]),
                Role = Convert.ToString(row["role"]),
            };
            return person;
        }
    }
}
