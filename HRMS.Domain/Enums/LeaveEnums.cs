namespace HRMS.Domain.Enums;




public enum LeaveType
{
    Vacation,
    Sick,
    Personal,
    Bereavement,
    JuryDuty,
    Military
}

public enum AccrualMethod
{
    Monthly,
    Yearly,
    PerPayPeriod
}

public enum ApproverType
{
    Manager,
    HR,
    Admin
}

public enum ActionType
{
    Submitted,
    Approved,
    Denied,
    Cancelled,
    Modified
}

public enum RequestStatus
{
    Draft,
    Submitted,
    PendingManager,
    PendingHr,
    Approved,
    Denied,
    Cancelled,
    Expired
}