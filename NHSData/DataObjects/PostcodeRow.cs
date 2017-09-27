namespace NHSData.DataObjects
{
    public class PostcodeRow : IDataRow
    {
        public string Postcode1 { get; set; }
        public string Postcode2 { get; set; }
        public string Postcode3 { get; set; }
        public string DateIntroduced { get; set; }
        public string UserType { get; set; }
        public string Easting { get; set; }
        public string Northing { get; set; }
        public string PositionalQuality { get; set; }
        public string CountyCode { get; set; }
        public string CountyName { get; set; }
        public string LocalAuthorityCode { get; set; }
        public string LocalAuthorityName { get; set; }
        public string WardCode { get; set; }
        public string WardName { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string RegionCode { get; set; }
        public string RegionName { get; set; }
        public string ParliamentaryConstituencyCode { get; set; }
        public string ParliamentaryConstituencyName { get; set; }
        public string EuropeanElectoralRegionCode { get; set; }
        public string EuropeanElectoralRegionName { get; set; }
        public string PrimaryCareTrustCode { get; set; }
        public string PrimaryCareTrustName { get; set; }
        public string LowerSuperOutputAreaCode { get; set; }
        public string LowerSuperOutputAreaName { get; set; }
        public string MiddleSuperOutputAreaCode { get; set; }
        public string MiddleSuperOutputAreaName { get; set; }
        public string OutputAreaClassificationCode { get; set; }
        public string OutputAreaClassificationName { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string SpatialAccuracy { get; set; }
        public string LastUploaded { get; set; }
        public string Location { get; set; }
        public string SocrataId { get; set; }
    }
}
