using Akka.Actor;
using Akka.Event;

namespace NHSData.Actors
{
    public class CoordinatorActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger;

        public CoordinatorActor()
        {
            _logger = Context.GetLogger();
            _logger.Info("Coordinator Initailized");
        }
    }
}