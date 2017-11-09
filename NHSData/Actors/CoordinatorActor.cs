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

            _addressDataAnalysisActor.Tell(new InitiateAnalysisMessage());
            Become(AnalyseAddresses);
        }

        private void AnalyseAddresses()
        {
            Receive<FileAnalysisFinishedMessage>(message => HandleFileAnalysisFinishedMessage(message));
        }

        private void CreateAddressAnalysisActor()
        {
            //IConfiguration configuration = new AddressConfiguration();
            IDataAnalyzer analyzer = new AddressDataAnalyzer("London");
            _addressDataAnalysisActor = Context.ActorOf(Props.Create(() => new AddressDataAnalysisActor<AddressRow, AddressMap>(analyzer, Path.Combine
                (ConfigurationManager.AppSettings["DataDirectory"], "address.csv"))), nameof(AddressDataAnalysisActor<AddressRow, AddressMap>));
        }

        private void CreateReferenceDataCreatorActor()
        {
            IConfiguration configuration = new ReferenceDataCreatorConfiguration();
            _referenceDataCreatorActor =
                Context.ActorOf(Props.Create(() => new ReferenceDataCreatorActor(configuration)));

        }

        private void HandleFileAnalysisFinishedMessage(FileAnalysisFinishedMessage message)
        {
            _logger.Info($"Received finished message from {Sender}");

            if (Sender.Equals(_addressDataAnalysisActor))
            {
                _logger.Info("Sending Publish Message");
                _addressDataAnalysisActor.Tell(new PublishResultsMessage());

                // Create the Reference Data now
                //CreateReferenceDataCreatorActor();
                //_referenceDataCreatorActor.Tell(new InitiateAnalysisMessage());
            //}
            //else if (Sender.Equals(_referenceDataCreatorActor))
            //{
                Thread.Sleep(100);
                Context.System.Terminate();
            }
        }
    }
}