using System.Threading.Tasks;
using Akka.Actor;
using NHSData.Actors;

namespace NHSData
{
    public class Program
    {
        private static ActorSystem _nhsDataAnalysisActorSystem;
        static void Main(string[] args)
        {
            RunActorSystem().Wait();
        }

        static async Task RunActorSystem()
        {
            _nhsDataAnalysisActorSystem = ActorSystem.Create("NhsDataAnalysis");

            _nhsDataAnalysisActorSystem.ActorOf<CoordinatorActor>("Coordinator");

            await _nhsDataAnalysisActorSystem.WhenTerminated;
        }
    }
}
//