namespace HRMS.Domain.Aggregates.EmployeeAggregate;
public class PhoneNumber
{
    public string CountryCode { get; private set; }
    public string Number { get; private set; }

    private PhoneNumber() { } // Needed by EF

    public PhoneNumber(string countryCode, string number)
    {
        CountryCode = countryCode;
        Number = number;
    }

    public override string ToString() => $"+{CountryCode} {Number}";
}