using System;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Event;
using NHSData.Actors;

namespace NHSData
{
    public class Program
    {
        private static ActorSystem _nhsDataAnalysisActorSystem;
        private static ILoggingAdapter _logger;
        static void Main(string[] args)
        {
            RunActorSystem().Wait();
        }

        static async Task RunActorSystem()
        {
            _nhsDataAnalysisActorSystem = ActorSystem.Create("NhsDataAnalysis");
            //_logger = _nhsDataAnalysisActorSystem.Log;

            //_logger.Info("NhsDataAnalysis ActorSystem initialised");

            _nhsDataAnalysisActorSystem.ActorOf<CoordinatorActor>("Coordinator");

            await _nhsDataAnalysisActorSystem.WhenTerminated;
        }
    }
}
