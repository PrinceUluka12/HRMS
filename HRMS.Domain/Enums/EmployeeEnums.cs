namespace HRMS.Domain.Enums;

public enum Gender
{
    Male,
    Female,
    NonBinary,
    Other,
    PreferNotToSay
}

public enum MaritalStatus
{
    Single,
    Married,
    Divorced,
    Widowed,
    DomesticPartnership
}

public enum EmploymentStatus
{
    Active,
    OnLeave,
    Suspended,
    Terminated,
    Retired
}

public enum EmploymentType
{
    Permanent,
    Contract,
    Temporary,
    Seasonal,
    Intern
}

public enum PayFrequency
{
    Weekly,
    BiWeekly,
    SemiMonthly,
    Monthly
}

public enum DependentRelationship
{
    Spouse,
    Child,
    Parent,
    Sibling,
    Other
}

public enum AttendanceStatus
{
    Present,
    Absent,
    Late,
    HalfDay,
    OnLeave,
    Holiday
}