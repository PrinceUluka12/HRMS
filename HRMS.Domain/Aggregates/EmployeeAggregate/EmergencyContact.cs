namespace HRMS.Domain.Aggregates.EmployeeAggregate;

public class EmergencyContact
{
    public EmergencyContact() { }

    public EmergencyContact(string name, string relationship, string phoneNumber, string email)
    {
        Name = name;
        Relationship = relationship;
        PhoneNumber = phoneNumber;
        Email = email;
    }

    public string Name { get; private set; }
    public string Relationship { get; private set; }
    public string PhoneNumber { get; private set; }
    public string Email { get; private set; }
}
