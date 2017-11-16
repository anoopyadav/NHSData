using System.Configuration;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using NHSData.CsvMaps;
using NHSData.ReferenceData;

namespace NHSData.Common
{
    public class ReferenceDataCreatorConfiguration : IConfiguration
    {
        public ICsvReader Reader { get; }
        public IReferenceDataReader ReferenceDataReader { get; }
        public IReferenceDataWriter ReferenceDataWriter { get; }

        public ReferenceDataCreatorConfiguration()
        {
            var csvConfiguration = new CsvConfiguration();
            csvConfiguration.RegisterClassMap<PostcodeMap>();
            var path = Path.Combine(ConfigurationManager.AppSettings["DataDirectory"], "postcode.csv");
            Reader = new CsvReader(new StreamReader(File.OpenRead(path)), csvConfiguration);

            ReferenceDataReader = new LocationReferenceDataReader();
            ReferenceDataWriter = new PostcodeReferenceDataWriter();

        }
    }
}