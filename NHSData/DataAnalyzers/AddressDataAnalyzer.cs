using System;
using System.Collections.Generic;
using Akka;
using NHSData.DataObjects;

namespace NHSData.DataAnalyzers
{
    public class AddressDataAnalyzer : IDataAnalyzer
    {
        private int _locationCount;
        private readonly string _location;

        public AddressDataAnalyzer(string location)
        {
            _location = location;
        }

        public void ConsumeRow(IDataRow row)
        {
            if (row.GetType() != typeof(Address))
            {
                throw new InvalidCastException();
            }
            var addressRow = (Address)row;
            CountLocations(addressRow);
        }

        private void CountLocations(Address row)
        {
            if (true)
            {
                _locationCount++;
            }
        }

        public IEnumerable<Tuple<string, string>> GetResults()
        {
            var results = new List<Tuple<string, string>>
            {
                new Tuple<string, string>($"Practices in {_location}", _locationCount.ToString())
            };
            Console.WriteLine(results[0]);

            return results;
        }

        public void PublishResults()
        {
            Console.WriteLine($"Practices in {_location}: {_locationCount}");
        }
    }
}