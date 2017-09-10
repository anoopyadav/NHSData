using CsvHelper.Configuration;
using NHSData.DataObjects;

namespace NHSData.CsvMaps
{
    public sealed class AddressMap : CsvClassMap<Address>
    {
        public AddressMap()
        {
            Map (m => m.Period).Name("Period");
            Map (m => m.PracticeCode).Name("PracticeCode");
            Map (m => m.PracticeName).Name("PracticeName");
            Map (m => m.PracticeBuilding).Name("PracticeBuilding");
            Map (m => m.StreetAddress).Name("Address");
            Map (m => m.Locality).Name("Locality");
            Map (m => m.Town).Name("Town");
            Map (m => m.Postcode).Name("Postcode");
        }
    }
}