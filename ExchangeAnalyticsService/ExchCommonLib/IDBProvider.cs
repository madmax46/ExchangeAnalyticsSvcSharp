using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace ExchCommonLib
{

    public interface IDbDataHandler<T>
    {
        T Execute(DbDataReader dataReader);
    }

    public interface IDBProvider
    {
        string ToSqlParam(object param);
        DataTable ProcedureByName(string procedure, params object[] par);

        DataSet ProcedureDSByName(string procedure, params object[] par);

        DataTable GetDataTable(string sqlQuery);

        T Query<T>(string sqlQuery, IDbDataHandler<T> dbDataHandler);
        int Execute(string query);


    }



}
