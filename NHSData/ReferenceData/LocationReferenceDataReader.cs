using System.Collections.Generic;
using System.Configuration;
using System.IO;
using CsvHelper;
using NHSData.DataObjects;

namespace NHSData.ReferenceData
{
    public class LocationReferenceDataReader : IReferenceDataReader
    {
        private readonly Dictionary<string, string> _locationDictionary;
        private readonly ICsvReader _csvReader; 

        public LocationReferenceDataReader()
        {
            _locationDictionary = new Dictionary<string, string>();
            _csvReader = new CsvReader(new StreamReader(Path.Combine(ConfigurationManager.AppSettings["DataDirectory"], ConfigurationManager.AppSettings["AddressReferenceData"])));
        }

        public void LoadReferenceData()
        {
            _csvReader.Read();
            _csvReader.ReadHeader();

            while(_csvReader.Read())
            {
                var record = _csvReader.GetRecord<AddressReferenceDataRow>();
                _locationDictionary.Add(record.PracticeCode, record.Postcode);
            }
        }

        public IDictionary<string, string> GetReferenceData()
        {
            return _locationDictionary;
        }
    }
}