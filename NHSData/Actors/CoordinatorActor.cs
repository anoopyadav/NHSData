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
        private IActorRef _prescriptionDataAnalysisActor;

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
            IDataAnalyzer analyzer = new AddressDataAnalyzer("London");
            _addressDataAnalysisActor = Context.ActorOf(Props.Create(() => new AddressDataAnalysisActor<AddressRow, AddressMap>(analyzer, Path.Combine
                (ConfigurationManager.AppSettings["DataDirectory"], "address.csv"))), "AddressDataAnalysisActor");
        }

        private void CreateReferenceDataCreatorActor()
        {
            _referenceDataCreatorActor = Context.ActorOf(Props.Create(() => new ReferenceDataCreatorActor(
                Path.Combine(ConfigurationManager.AppSettings["DataDirectory"], "postcode.csv"))));
        }

        private void CreatePrescriptionAnalysisActor()
        {
            IDataAnalyzer analyzer = new AddressDataAnalyzer("");
            _prescriptionDataAnalysisActor = Context.ActorOf(Props.Create(() => new
                PrescriptionDataAnalysisActor<PrescriptionRow, PrescriptionMap>(analyzer,
                    Path.Combine(ConfigurationManager.AppSettings["DataDirectory"], "prescription.csv"))));
        }

        private void HandleFileAnalysisFinishedMessage(FileAnalysisFinishedMessage message)
        {
            _logger.Info($"Received finished message from {Sender}");

            if (Sender.Equals(_addressDataAnalysisActor))
            {
                _logger.Info("Sending Publish Message");
                _addressDataAnalysisActor.Tell(new PublishResultsMessage());
                _addressDataAnalysisActor.Tell(PoisonPill.Instance);
            }
            else if (Sender.Equals(_referenceDataCreatorActor))
            {
                _referenceDataCreatorActor.Tell(PoisonPill.Instance);
                CreatePrescriptionAnalysisActor();
                _prescriptionDataAnalysisActor.Tell(new LoadReferenceDataMessage());
            }
            else if (Sender.Equals(_prescriptionDataAnalysisActor))
            {
                _logger.Info("Shutting Down...");
                Thread.Sleep(100);
                Context.System.Terminate();
            }
        }
    }
}