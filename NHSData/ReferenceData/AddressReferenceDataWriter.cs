using System.Collections.Generic;
using System.Configuration;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using NHSData.DataObjects;

namespace NHSData.ReferenceData
{

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

            using (_referenceDataWriter =
                new CsvWriter(new StreamWriter(destinationFile)))
            {
                _referenceDataWriter.WriteHeader<AddressReferenceDataRow>();
                foreach (var row in _practicesToPostcodeMap)
                {
                    _referenceDataWriter.WriteRecord(new AddressReferenceDataRow(row.Key, row.Value));
                }
            }
        }

        private string StripWhitespace(string input)
        {
            return input.Replace(" ", string.Empty);
        }
    }
}