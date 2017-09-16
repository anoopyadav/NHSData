using System;
using System.CodeDom;
using System.Data;
using System.Diagnostics;
using System.Linq;
using Akka.Actor;
using Akka.Event;
using CsvHelper;
using NHSData.Common;
using NHSData.DataAnalyzers;
using NHSData.DataObjects;
using NHSData.Messages;

namespace NHSData.Actors
{
    public abstract class BaseDataAnalysisActor : ReceiveActor
    {
        protected ILoggingAdapter Logger { get; }
        protected IDataAnalyzer Analyzer { get; }
        protected IConfiguration Configuration { get; }
        protected ICsvReader CsvReader { get; }

        protected BaseDataAnalysisActor(IDataAnalyzer analyzer, IConfiguration configuration)
        {
            Analyzer = analyzer;
            Configuration =  configuration;
            CsvReader = configuration.Reader;
            Logger = Context.GetLogger();

            Receive<InitiateAnalysisMessage>(message =>
            {
                Logger.Info("Received InitiateAnalysisMessage, Proceeding with file analysis.");
                PerformAnalysis();
            });

            Receive<PublishResultsMessage>(message =>
            {
                Logger.Info("Publishing Results.");
                Logger.Info($"Results - {Analyzer.GetResults().First().ToString()}");
            });

        }

        protected abstract void PerformAnalysis();
    }
}