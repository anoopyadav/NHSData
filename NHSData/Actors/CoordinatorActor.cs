using System;
using System.Collections.Generic;
using System.Threading;
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
        private int _responseCount;
        private readonly IList<IActorRef> _dataAnalysisActors;

        public CoordinatorActor()
        {
            _logger = Context.GetLogger();
            _logger.Info("Coordinator Initailized");

            _dataAnalysisActors = new List<IActorRef>();
            IConfiguration configuration = new AddressConfiguration();
            IDataAnalyzer analyzer = new AddressDataAnalyzer("London");
            IActorRef actor = Context.ActorOf(Props.Create(() => new AddressDataAnalysisActor(analyzer, configuration)), nameof(AddressDataAnalysisActor));
            _dataAnalysisActors.Add(actor);
            actor.Tell(new InitiateAnalysisMessage());

            Receive<FileAnalysisFinishedMessage>(message => HandleFileAnalysisFinishedMessage(message));
        }

        private void HandleFileAnalysisFinishedMessage(FileAnalysisFinishedMessage message)
        {
            _logger.Info($"Received finished message from {Sender}");
            _responseCount++;

            if (_responseCount == 1)
            {

                foreach (var actor in _dataAnalysisActors)
                {
                    _logger.Info("Sending Publish Message");
                    actor.Tell(new PublishResultsMessage());
                }
                Thread.Sleep(100);
                Context.System.Terminate();
            }
        }
    }
}