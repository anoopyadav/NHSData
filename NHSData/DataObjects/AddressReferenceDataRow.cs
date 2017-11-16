namespace NHSData.DataObjects
{
    public class AddressReferenceDataRow
    {
        public string PracticeCode { get; }
        public string Postcode { get; }

        public AddressReferenceDataRow(string practiceCode, string postcode)
        {
            PracticeCode = practiceCode;
            Postcode = postcode;
        }
    }

}