using System.Collections.Generic;
using System.Configuration;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using NHSData.DataObjects;

namespace NHSData.ReferenceData
{
    public class AddressReferenceData
    {
        public string PracticeCode { get; }
        public string Postcode { get; }

        public AddressReferenceData(string practiceCode, string postcode)
        {
            PracticeCode = practiceCode;
            Postcode = postcode;
        }
    }

    public class AddressReferenceDataMap : CsvClassMap<AddressReferenceData>
    {
        public AddressReferenceDataMap()
        {
            Map(m => m.PracticeCode).Name("PracticeCode");
            Map(m => m.Postcode).Name("Postcode");
        }
    }
    public class AddressReferenceDataWriter : IReferenceDataWriter
    {
        private readonly Dictionary<string, string> _practicesToPostcodeMap;
        private ICsvWriter _referenceDataWriter;

        public AddressReferenceDataWriter()
        {
            _practicesToPostcodeMap = new Dictionary<string, string>();
        }
        public void UpdateReferenceData(IDataRow row)
        {
            var addressRow = (AddressRow) row;
            if (!_practicesToPostcodeMap.ContainsKey(addressRow.PracticeCode))
            {
                _practicesToPostcodeMap[StripWhitespace(addressRow.PracticeCode)] = 
                    StripWhitespace(addressRow.Postcode);
            }
        }

        public void WriteReferenceData()
        {
            var destinationFile = Path.Combine(ConfigurationManager.AppSettings["DataDirectory"],
                ConfigurationManager.AppSettings["AddressReferenceData"]);

            var configuration = new CsvConfiguration();
            configuration.RegisterClassMap<AddressReferenceDataMap>();

            using (_referenceDataWriter =
                new CsvWriter(new StreamWriter(destinationFile)))
            {
                _referenceDataWriter.WriteHeader<AddressReferenceData>();
                foreach (var row in _practicesToPostcodeMap)
                {
                    _referenceDataWriter.WriteRecord(new AddressReferenceData(row.Key, row.Value));
                }
            }
        }

        private string StripWhitespace(string input)
        {
            return input.Replace(" ", string.Empty);
        }
    }
}