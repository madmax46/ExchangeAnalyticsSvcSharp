using DbWrapperCore;
using ExchCommonLib.Classes;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmaAnalyseWorker.Workers
{
    public class AutoInstrumentsAnalyser
    {
        private readonly ILogger<AutoInstrumentsAnalyser> logger;

        private readonly Task mainWorkTask;
        private readonly IDBProvider smanAlyticsDb;
        private readonly IDBProvider stockQuotesDb;

        public AutoInstrumentsAnalyser(ILogger<AutoInstrumentsAnalyser> logger, IDBProvider smanAlyticsDb, IDBProvider stockQuotesDb)
        {
            this.logger = logger;
            this.smanAlyticsDb = smanAlyticsDb;
            this.stockQuotesDb = stockQuotesDb;

            mainWorkTask = new Task(EndlessMethod);
            logger.LogDebug("AutoInstrumentsAnalyser init");
        }

        public void RunAsync()
        {
            mainWorkTask.Start();
        }


        private void EndlessMethod()
        {
            while (true)
            {
                try
                {
                    AnalyseAllInstruments();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "error");
                }

                Thread.Sleep(100);
            }
        }


        private void AnalyseAllInstruments()
        {
            var instruments = LoadInstrumentsFromDb();


        }

        private List<ParserInfo> LoadInstrumentsFromDb()
        {
            var table = stockQuotesDb.ProcedureByName("svc_getParsers");

            List<ParserInfo> parsersInfo = new List<ParserInfo>();
            foreach (DataRow oneRow in table.Rows)
            {
                var parser = ParserInfo.FromRow(oneRow);
                parsersInfo.Add(parser);
            }
            return parsersInfo;
        }

        private void LoadTechMethodsFromDb()
        {

        }
    }
}
