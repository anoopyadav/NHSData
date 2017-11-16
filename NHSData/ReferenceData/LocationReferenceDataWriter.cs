using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using NHSData.DataObjects;

namespace NHSData.ReferenceData
{
    public class LocationReferenceData
    {
        public string PracticeCode { get; set; }
        public string Postcode { get; set; }

        public LocationReferenceData(string practiceCode, string postcode)
        {
            PracticeCode = practiceCode;
            Postcode = postcode;
        }
    }

    public sealed class LocationReferenceDataMap : CsvClassMap<LocationReferenceData>
    {
        public LocationReferenceDataMap()
        {
            Map(m => m.PracticeCode).Name("PracticeCode");
            Map(m => m.Postcode).Name("Postcode");
        }
    }
    public class LocationReferenceDataWriter : IReferenceDataWriter
    {
        private readonly IList<Tuple<string, string>> _practiceToPostcodeMap;
        private ICsvWriter _referenceDataWriter;
        public LocationReferenceDataWriter()
        {
            _practiceToPostcodeMap = new List<Tuple<string, string>>();
        }
        public void UpdateReferenceData(IDataRow row)
        {
            var locationRow = (LocationReferenceDataRow)row;
            _practiceToPostcodeMap.Add(new Tuple<string, string>(locationRow.PracticeCode, locationRow.Postcode));
        }

        public void WriteReferenceData()
        {
            var destinationFile = Path.Combine(ConfigurationManager.AppSettings["DataDirectory"], "LocationReferenceData.csv");

            using (_referenceDataWriter = new CsvWriter(new StreamWriter(destinationFile)))
            {
                _referenceDataWriter.WriteHeader<LocationReferenceData>();

                foreach (var row in _practiceToPostcodeMap)
                {
                    _referenceDataWriter.WriteRecord(new LocationReferenceData(row.Item1, row.Item2));
                }
            }
        }
    }
}