using System;
using System.Collections.Generic;

namespace NHSData.Messages
{
    public class GetResultsResponseMessage
    {
        public IEnumerable<Tuple<string, string>> Results;

        public GetResultsResponseMessage(IEnumerable<Tuple<string, string>> results)
        {
            Results = results;
        }
    }
}