using CsvHelper.Configuration;
using NHSData.DataObjects;

namespace NHSData.CsvMaps
{
    public sealed class PostcodeMap : CsvClassMap<PostcodeRow>
    {
        public PostcodeMap()
        {
            Map(m => m.Postcode1).Name("Postcode 1");
            Map(m => m.Postcode2).Name("Postcode 2");
            Map(m => m.Postcode3).Name("Postcode 3");
            Map(m => m.DateIntroduced).Name("Date Introduced");
            Map(m => m.UserType).Name("User Type");
            Map(m => m.Easting).Name("Easting");
            Map(m => m.Northing).Name("Northing");
            Map(m => m.PositionalQuality).Name("Positional Quality");
            Map(m => m.CountyCode).Name("County Code");
            Map(m => m.CountryName).Name("County Name");
            Map(m => m.LocalAuthorityCode).Name("Local Authority Code");
            Map(m => m.WardCode).Name("Ward Code");
            Map(m => m.WardName).Name("Ward Name");
            Map(m => m.CountryCode).Name("Country Code");
            Map(m => m.CountryName).Name("CountryName");
            Map(m => m.RegionCode).Name("Region Code");
            Map(m => m.RegionName).Name("RegionName");
            Map(m => m.ParliamentaryConstituencyCode).Name("Parliamentary Constituency Code");
            Map(m => m.ParliamentaryConstituencyName).Name("Parliamentary Constituency Name");
            Map(m => m.EuropeanElectoralRegionCode).Name("European Electoral Region Code");
            Map(m => m.EuropeanElectoralRegionName).Name("European Electoral Region Name");
            Map(m => m.PrimaryCareTrustCode).Name("Primary Care Trust Code");
            Map(m => m.PrimaryCareTrustName).Name("Primary Care Trust Name");
            Map(m => m.LowerSuperOutputAreaCode).Name("Lower Super Output Area Code");
            Map(m => m.LowerSuperOutputAreaName).Name("Lower Super Output Area Name");
            Map(m => m.MiddleSuperOutputAreaCode).Name("Middle Super Output Area Code");
            Map(m => m.MiddleSuperOutputAreaName).Name("Middle Super Output Area Name");
            Map(m => m.OutputAreaClassificationCode).Name("Output Area Classification Code");
            Map(m => m.OutputAreaClassificationName).Name("Output Area Classification Name");
            Map(m => m.Longitude).Name("Longitude");
            Map(m => m.Latitude).Name("Latitude");
            Map(m => m.SpatialAccuracy).Name("Spatial Accuracy");
            Map(m => m.LastUploaded).Name("Last Uploaded");
            Map(m => m.Location).Name("Location");
            Map(m => m.SocrataId).Name("Socrata ID");
        }
    }
}