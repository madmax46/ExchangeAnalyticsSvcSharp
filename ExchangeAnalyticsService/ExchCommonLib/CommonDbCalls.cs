using DbWrapperCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ExchCommonLib
{
    public static class CommonDbCalls
    {
        public static DataTable GetAllParsersFromDB(IDBProvider dbProvider)
        {
            var ds = dbProvider.ProcedureByName("GetAllParsers");
            return ds;
        }
    }
}
