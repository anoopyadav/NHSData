using System.Linq;
using Akka.Actor;
using Akka.Event;
using NHSData.DataAnalyzers;
using NHSData.Messages;

namespace NHSData.Actors
{
    public abstract class BaseDataAnalysisActor<TRowType, TRowMap> : ReceiveActor
    {
        protected ILoggingAdapter Logger { get; }
        protected IDataAnalyzer Analyzer { get; }
        protected IActorRef CsvReaderActor { get; }

        protected BaseDataAnalysisActor(IDataAnalyzer analyzer, string sourcePath)
        {
            Analyzer = analyzer;
            Logger = Context.GetLogger();
            CsvReaderActor = Context.ActorOf(Props.Create<CsvReaderActor<TRowType, TRowMap>>(sourcePath), $"CsvReaderActor");
            Become(ProcessData);
            Logger.Info("Base Constructor");
        }

        protected void ProcessData()
        {
            Receive<InitiateAnalysisMessage>(message =>
            {
                Logger.Info("Received InitiateAnalysisMessage, Proceeding with file analysis.");
                CsvReaderActor.Tell(new InitiateAnalysisMessage());
            });

            Receive<PublishResultsMessage>(message =>
            {
                Logger.Info("Publishing Results.");
                Logger.Info($"Results - {Analyzer.GetResults().First().ToString()}");
            });

            Receive<DataRowMessage>(message => ProcessRow(message));

            Receive<FileAnalysisFinishedMessage>(message => PostAnalysis());
        }

        protected abstract void ProcessRow(DataRowMessage message);
        protected abstract void PostAnalysis();
    }
}