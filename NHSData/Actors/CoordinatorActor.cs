using Akka.Actor;
using Akka.Event;
using NHSData.Common;
using NHSData.CsvMaps;
using NHSData.DataAnalyzers;
using NHSData.Messages;
using Address = NHSData.DataObjects.Address;

namespace NHSData.Actors
{
    public class CoordinatorActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger;

        public CoordinatorActor()
        {
            _logger = Context.GetLogger();
            _logger.Info("Coordinator Initailized");

            IConfiguration configuration = new AddressConfiguration();
            IDataAnalyzer analyzer = new AddressDataAnalyzer();
            IActorRef actor = Context.ActorOf(Props.Create(() => new AddressDataAnalysisActor(analyzer, configuration)));
            actor.Tell(new InitiateAnalysisMessage());

        }
    }
}