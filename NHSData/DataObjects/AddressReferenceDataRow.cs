namespace NHSData.DataObjects
{
    public class AddressReferenceDataRow : IDataRow
    {
        public string PracticeCode { get; set; }
        public string Postcode { get; set; }

        public AddressReferenceDataRow(string practiceCode, string postcode)
        {
            PracticeCode = practiceCode;
            Postcode = postcode;
        }
    }

}