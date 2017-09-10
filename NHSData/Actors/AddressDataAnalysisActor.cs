using System.Runtime.Remoting.Messaging;
using Akka.Actor;
using NHSData.Common;
using NHSData.DataAnalyzers;

namespace NHSData.Actors
{
    public class AddressDataAnalysisActor : BaseDataAnalysisActor
    {
        public AddressDataAnalysisActor(IDataAnalyzer analyzer, IConfiguration configuration)
            : base(analyzer, configuration)
        {
            
        }

        protected override void Reading()
        {
            throw new System.NotImplementedException();
        }

        protected override void Done()
        {
            throw new System.NotImplementedException();
        }
    }
}