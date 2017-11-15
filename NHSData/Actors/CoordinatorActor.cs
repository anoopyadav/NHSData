using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading;
using Akka.Actor;
using Akka.Event;
using NHSData.Common;
using NHSData.CsvMaps;
using NHSData.DataAnalyzers;
using NHSData.DataObjects;
using NHSData.Messages;

namespace NHSData.Actors
{
    public class CoordinatorActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger;
        private IActorRef _addressDataAnalysisActor;
        private IActorRef _referenceDataCreatorActor;

        public CoordinatorActor()
        {
            _logger = Context.GetLogger();
            _logger.Info("Coordinator Initailized");

            CreateAddressAnalysisActor();
            CreateReferenceDataCreatorActor();

            _addressDataAnalysisActor.Tell(new InitiateAnalysisMessage());
            _referenceDataCreatorActor.Tell(new InitiateAnalysisMessage());
            Become(Analyse);
        }

        private void Analyse()
        {
            Receive<FileAnalysisFinishedMessage>(message => HandleFileAnalysisFinishedMessage(message));
        }

        private void CreateAddressAnalysisActor()
        {
            //IConfiguration configuration = new AddressConfiguration();
            IDataAnalyzer analyzer = new AddressDataAnalyzer("London");
            _addressDataAnalysisActor = Context.ActorOf(Props.Create(() => new AddressDataAnalysisActor<AddressRow, AddressMap>(analyzer, Path.Combine
                (ConfigurationManager.AppSettings["DataDirectory"], "address.csv"))), "AddressDataAnalysisActor");
        }

        private void CreateReferenceDataCreatorActor()
        {
            _referenceDataCreatorActor = Context.ActorOf(Props.Create(() => new ReferenceDataCreatorActor(
                Path.Combine(ConfigurationManager.AppSettings["DataDirectory"], "postcode.csv"))));
        }

        private void HandleFileAnalysisFinishedMessage(FileAnalysisFinishedMessage message)
        {
            _logger.Info($"Received finished message from {Sender}");

            if (Sender.Equals(_addressDataAnalysisActor))
            {
                _logger.Info("Sending Publish Message");
                _addressDataAnalysisActor.Tell(new PublishResultsMessage());
            }
            else if (Sender.Equals(_referenceDataCreatorActor))
            {
                _logger.Info("Shutting Down...");
                Thread.Sleep(100);
                Context.System.Terminate();
            }
        }
    }
}