using System;
using System.Collections.Generic;
using Akka;
using NHSData.DataObjects;

namespace NHSData.DataAnalyzers
{
    public class AddressDataAnalyzer : IDataAnalyzer
    {
        public void ConsumeRow(IDataRow row)
        {
            if (row.GetType() != typeof(Address))
            {
                throw new InvalidCastException();
            }
            var addressRow = (Address)row;


            Console.WriteLine(addressRow.PracticeName);
        }

        public IEnumerable<Tuple<string, string>> GetResults()
        {
            throw new NotImplementedException();
        }
    }
}