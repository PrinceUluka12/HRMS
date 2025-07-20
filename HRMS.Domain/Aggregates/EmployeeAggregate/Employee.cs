using HRMS.Domain.Aggregates.DepartmentAggregate;
using HRMS.Domain.Aggregates.LeaveAggregate;
using HRMS.Domain.Aggregates.OffboardingAggregate;
using HRMS.Domain.Aggregates.OnboardingAggregate;
using HRMS.Domain.Aggregates.PerformanceAggregate;
using HRMS.Domain.Aggregates.PositionAggregate;
using HRMS.Domain.Enums;
using HRMS.Domain.Exceptions;
using HRMS.Domain.SeedWork;

namespace HRMS.Domain.Aggregates.EmployeeAggregate;

public class Employee : Entity<Guid>, IAggregateRoot
{
    // Identification
    public string AzureAdId { get; private set; }
    public string EmployeeNumber { get; private set; }
    public string GovernmentId { get; private set; } // SSN/National ID
    public string TaxIdentificationNumber { get; private set; }

    // Personal Information
    public PersonName Name { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    public Gender Gender { get; private set; }
    public MaritalStatus MaritalStatus { get; private set; }

    // Contact Information
    public Email Email { get; private set; }
    public PhoneNumber WorkPhone { get; private set; }
    public PhoneNumber PersonalPhone { get; private set; }
    public Address PrimaryAddress { get; private set; }

    // Employment Details
    public DateTime HireDate { get; private set; }
    public DateTime? TerminationDate { get; private set; }
    public string? TerminationReason { get; private set; }
    public EmploymentStatus Status { get; private set; }
    public EmploymentType EmploymentType { get; private set; }
    public bool IsFullTime { get; private set; }
    public decimal FullTimeEquivalent { get; private set; } // For part-time employees

    // Organizational Structure
    public Guid DepartmentId { get; private set; }
    public Department  Department { get; private set; }
    public Guid PositionId { get; private set; }
    public Position  Position { get; private set; }
    public string JobTitle { get; private set; }


    // Compensation
    public decimal BaseSalary { get; private set; }
    public PayFrequency PayFrequency { get; private set; }
    public BankDetails BankDetails { get; private set; }

    // Emergency Contacts
    private readonly List<EmergencyContact> _emergencyContacts = new();
    public IReadOnlyCollection<EmergencyContact> EmergencyContacts => _emergencyContacts.AsReadOnly();

    // Dependents
    private readonly List<Dependent> _dependents = new();
    public IReadOnlyCollection<Dependent> Dependents => _dependents.AsReadOnly();

    // Skills & Qualifications
    private readonly List<Skill> _skills = new();
    public IReadOnlyCollection<Skill> Skills => _skills.AsReadOnly();

    private readonly List<Certification> _certifications = new();
    public IReadOnlyCollection<Certification> Certifications => _certifications.AsReadOnly();

    private readonly List<Education> _educationHistory = new();
    public IReadOnlyCollection<Education> EducationHistory => _educationHistory.AsReadOnly();

    // Leave & Attendance
    private readonly List<LeaveRequest> _leaveRequests = new();
    public IReadOnlyCollection<LeaveRequest> LeaveRequests => _leaveRequests.AsReadOnly();

    private readonly List<AttendanceRecord> _attendanceRecords = new();
    public IReadOnlyCollection<AttendanceRecord> AttendanceRecords => _attendanceRecords.AsReadOnly();

    // Performance
    private readonly List<PerformanceReview> _performanceReviews = new();
    public IReadOnlyCollection<PerformanceReview> PerformanceReviews => _performanceReviews.AsReadOnly();

    // Onboarding/Offboarding
    private readonly List<OnboardingTask> _onboardingTasks = new();
    public IReadOnlyCollection<OnboardingTask> OnboardingTasks => _onboardingTasks.AsReadOnly();

    private readonly List<OffboardingChecklist> _offboardingChecklists = new();
    public IReadOnlyCollection<OffboardingChecklist> OffboardingChecklists => _offboardingChecklists.AsReadOnly();

    // Private constructor for EF Core
    private Employee() { }

    public Employee(
        string azureAdId,
        string employeeNumber,
        string governmentId,
        string taxIdentificationNumber,
        PersonName name,
        DateTime dateOfBirth,
        Gender gender,
        MaritalStatus maritalStatus,
        Email email,
        PhoneNumber workPhone,
        PhoneNumber personalPhone,
        Address primaryAddress,
        DateTime hireDate,
        EmploymentType employmentType,
        bool isFullTime,
        Guid departmentId,
        Guid positionId,
        string jobTitle,
        decimal baseSalary,
        PayFrequency payFrequency,
        BankDetails bankDetails)
    {
        AzureAdId = azureAdId ?? throw new ArgumentNullException(nameof(azureAdId));
        EmployeeNumber = employeeNumber ?? throw new ArgumentNullException(nameof(employeeNumber));
        GovernmentId = governmentId ?? throw new ArgumentNullException(nameof(governmentId));
        TaxIdentificationNumber = taxIdentificationNumber ?? throw new ArgumentNullException(nameof(taxIdentificationNumber));
        Name = name ?? throw new ArgumentNullException(nameof(name));
        DateOfBirth = dateOfBirth;
        Gender = gender;
        MaritalStatus = maritalStatus;
        Email = email ?? throw new ArgumentNullException(nameof(email));
        WorkPhone = workPhone ?? throw new ArgumentNullException(nameof(workPhone));
        PersonalPhone = personalPhone ?? throw new ArgumentNullException(nameof(personalPhone));
        PrimaryAddress = primaryAddress ?? throw new ArgumentNullException(nameof(primaryAddress));
        HireDate = hireDate;
        EmploymentType = employmentType;
        IsFullTime = isFullTime;
        FullTimeEquivalent = isFullTime ? 1.0m : 0.5m; // Default for part-time
        DepartmentId = departmentId;
        PositionId = positionId;
        JobTitle = jobTitle ?? throw new ArgumentNullException(nameof(jobTitle));
        BaseSalary = baseSalary;
        PayFrequency = payFrequency;
        BankDetails = bankDetails ?? throw new ArgumentNullException(nameof(bankDetails));
        Status = EmploymentStatus.Active;
    }

    // Domain Methods
    public void UpdatePersonalInformation(
        PersonName name,
        DateTime dateOfBirth,
        Gender gender,
        MaritalStatus maritalStatus)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        DateOfBirth = dateOfBirth;
        Gender = gender;
        MaritalStatus = maritalStatus;
    }

    public void UpdateContactInformation(
        Email email,
        PhoneNumber workPhone,
        PhoneNumber personalPhone,
        Address primaryAddress,
        Address? secondaryAddress)
    {
        Email = email ?? throw new ArgumentNullException(nameof(email));
        WorkPhone = workPhone ?? throw new ArgumentNullException(nameof(workPhone));
        PersonalPhone = personalPhone ?? throw new ArgumentNullException(nameof(personalPhone));
        PrimaryAddress = primaryAddress ?? throw new ArgumentNullException(nameof(primaryAddress));
        
    }

    public void UpdateEmploymentDetails(
        Guid departmentId,
        Guid positionId,
        Guid? managerId,
        string jobTitle,
        string? costCenter,
        EmploymentType employmentType,
        bool isFullTime,
        decimal fullTimeEquivalent)
    {
        DepartmentId = departmentId;
        PositionId = positionId;
        JobTitle = jobTitle ?? throw new ArgumentNullException(nameof(jobTitle));
        EmploymentType = employmentType;
        IsFullTime = isFullTime;
        FullTimeEquivalent = fullTimeEquivalent;
    }

    public void UpdateCompensation(
        decimal baseSalary,
        PayFrequency payFrequency,
        BankDetails bankDetails)
    {
        BaseSalary = baseSalary;
        PayFrequency = payFrequency;
        BankDetails = bankDetails ?? throw new ArgumentNullException(nameof(bankDetails));
    }

    public void TerminateEmployment(DateTime terminationDate, string reason)
    {
        if (Status != EmploymentStatus.Active)
            throw new DomainException("Only active employees can be terminated");

        TerminationDate = terminationDate;
        TerminationReason = reason ?? throw new ArgumentNullException(nameof(reason));
        Status = EmploymentStatus.Terminated;
    }

    public void RehireEmployee(DateTime rehireDate)
    {
        if (Status != EmploymentStatus.Terminated)
            throw new DomainException("Only terminated employees can be rehired");

        HireDate = rehireDate;
        TerminationDate = null;
        TerminationReason = null;
        Status = EmploymentStatus.Active;
    }

    public void PlaceOnLeave(DateTime startDate, DateTime? endDate)
    {
        if (Status != EmploymentStatus.Active)
            throw new DomainException("Only active employees can be placed on leave");

        Status = EmploymentStatus.OnLeave;
        // Additional leave tracking would be handled by a LeaveRequest
    }

    public void ReturnFromLeave()
    {
        if (Status != EmploymentStatus.OnLeave)
            throw new DomainException("Only employees on leave can return");

        Status = EmploymentStatus.Active;
    }

    // Additional domain methods for collections...
    public void AddEmergencyContact(EmergencyContact contact)
    {
        if (contact == null) throw new ArgumentNullException(nameof(contact));
        _emergencyContacts.Add(contact);
    }

    public void AddDependent(Dependent dependent)
    {
        if (dependent == null) throw new ArgumentNullException(nameof(dependent));
        _dependents.Add(dependent);
    }

    public void AddSkill(Skill skill)
    {
        if (skill == null) throw new ArgumentNullException(nameof(skill));
        _skills.Add(skill);
    }

    public void RequestLeave(LeaveRequest leaveRequest)
    {
        
    }

    // Other collection management methods...
}