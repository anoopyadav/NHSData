using System.Collections.Generic;
using System.Configuration;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using NHSData.DataObjects;

namespace NHSData.ReferenceData
{
    public class PostcodeReferenceDataRow : IDataRow
    {
        public string Postcode { get; set; }
        public string Region { get; set; }
        public PostcodeReferenceDataRow(string postcode, string region)
        {
            Postcode = postcode;
            Region = region;
        }

        public PostcodeReferenceDataRow()
        {
        }
    }

    public sealed class PostcodeReferenceDataMap : CsvClassMap<PostcodeReferenceDataRow>
    {
        public PostcodeReferenceDataMap()
        {
            Map(x => x.Postcode).Name("Postcode");
            Map(x => x.Region).Name("Region");
        }
    }

    public class PostcodeReferenceDataWriter : IReferenceDataWriter
    {
        private readonly Dictionary<string, string> _postcodeToRegionMap;
        private ICsvWriter _referenceDataWriter;
        public PostcodeReferenceDataWriter()
        {
            _postcodeToRegionMap = new Dictionary<string, string>();
        }
        public void UpdateReferenceData(IDataRow row)
        {
            var postcodeRow = (PostcodeRow) row;
            if (!_postcodeToRegionMap.ContainsKey(postcodeRow.Postcode1))
            {
                _postcodeToRegionMap.Add(postcodeRow.Postcode1, postcodeRow.RegionName);
            }
        }

        public void WriteReferenceData()
        {
            var destinationFile = Path.Combine(ConfigurationManager.AppSettings["DataDirectory"], "RegionReferenceData.csv");
            using (_referenceDataWriter = new CsvWriter(new StreamWriter(destinationFile)))
            {
                _referenceDataWriter.WriteHeader<PostcodeReferenceDataRow>();
                foreach (var pair in _postcodeToRegionMap)
                {
                    _referenceDataWriter.WriteRecord(new PostcodeReferenceDataRow(pair.Key, pair.Value));
                }
            }
        }
    }
}