using HRMS.Domain.Aggregates.EmployeeAggregate;
using HRMS.Domain.Aggregates.LeaveAggregate;
using HRMS.Domain.Aggregates.OffboardingAggregate;
using HRMS.Domain.Aggregates.OnboardingAggregate;
using HRMS.Domain.Aggregates.PerformanceAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Infrastructure.Persistence.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");

        builder.HasKey(e => e.Id);

        // Identification
        builder.Property(e => e.AzureAdId).HasMaxLength(128).IsRequired();
        builder.Property(e => e.EmployeeNumber).HasMaxLength(20).IsRequired();
        builder.Property(e => e.GovernmentId).HasMaxLength(50).IsRequired();
        builder.Property(e => e.TaxIdentificationNumber).HasMaxLength(50).IsRequired();

        // Personal Info
        builder.OwnsOne(e => e.Name, name =>
        {
            name.Property(n => n.FirstName).HasMaxLength(50).IsRequired();
            name.Property(n => n.LastName).HasMaxLength(50).IsRequired();
        });

        builder.Property(e => e.DateOfBirth).IsRequired();
        builder.Property(e => e.Gender).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(e => e.MaritalStatus).HasConversion<string>().HasMaxLength(20).IsRequired();

        // Contact Info
        builder.OwnsOne(e => e.Email, email =>
        {
            email.Property(p => p.Value).HasColumnName("Email").HasMaxLength(100).IsRequired();
        });

        builder.OwnsOne(e => e.WorkPhone, phone =>
        {
            phone.Property(p => p.CountryCode).HasMaxLength(5).IsRequired();
            phone.Property(p => p.Number).HasMaxLength(20).IsRequired();
        });

        builder.OwnsOne(e => e.PersonalPhone, phone =>
        {
            phone.Property(p => p.CountryCode).HasMaxLength(5).IsRequired();
            phone.Property(p => p.Number).HasMaxLength(20).IsRequired();
        });

        builder.OwnsOne(e => e.PrimaryAddress, address =>
        {
            address.Property(a => a.Street).HasMaxLength(100);
            address.Property(a => a.City).HasMaxLength(50);
            address.Property(a => a.State).HasMaxLength(50);
            address.Property(a => a.PostalCode).HasMaxLength(10);
            address.Property(a => a.Country).HasMaxLength(50);
        });

        // Employment
        builder.Property(e => e.HireDate).IsRequired();
        builder.Property(e => e.TerminationDate);
        builder.Property(e => e.TerminationReason).HasMaxLength(500);
        builder.Property(e => e.Status).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(e => e.EmploymentType).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(e => e.IsFullTime).IsRequired();
        builder.Property(e => e.BaseSalary).HasPrecision(18, 2).IsRequired();

        // Org Structure
        builder.HasOne(e => e.Department)
            .WithMany()
            .HasForeignKey(e => e.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(e => e.Position)
            .WithMany()
            .HasForeignKey(e => e.PositionId)
            .OnDelete(DeleteBehavior.Restrict);
        
       
        builder.Property(e => e.JobTitle).HasMaxLength(100).IsRequired();
        

        // Compensation
        builder.Property(e => e.BaseSalary).HasPrecision(18, 2).IsRequired();
        builder.Property(e => e.PayFrequency).HasConversion<string>().HasMaxLength(20).IsRequired();

        builder.OwnsOne(e => e.BankDetails, bd =>
        {
            bd.Property(b => b.BankName).HasMaxLength(100);
            bd.Property(b => b.AccountNumber).HasMaxLength(50);
            bd.Property(b => b.RoutingNumber).HasMaxLength(20);
        });

        // Emergency Contacts
        builder.OwnsMany(e => e.EmergencyContacts, ec =>
        {
            ec.WithOwner().HasForeignKey("EmployeeId");
            ec.Property<Guid>("Id").ValueGeneratedOnAdd();
            ec.HasKey("Id");
            ec.Property(c => c.Name).IsRequired().HasMaxLength(100);
            ec.Property(c => c.Relationship).IsRequired().HasMaxLength(50);
            ec.Property(c => c.Email).IsRequired().HasMaxLength(100);

            ec.OwnsOne(c => c.PhoneNumber, pn =>
            {
                pn.Property(p => p.CountryCode).HasMaxLength(5).IsRequired();
                pn.Property(p => p.Number).HasMaxLength(20).IsRequired();
            });

            ec.ToTable("EmployeeEmergencyContacts");
        });

        // Dependents
        builder.OwnsMany(e => e.Dependents, d =>
        {
            d.WithOwner().HasForeignKey("EmployeeId");
            d.Property<Guid>("Id").ValueGeneratedOnAdd();
            d.HasKey("Id");
            d.ToTable("EmployeeDependents");
        });

        // Skills
        builder.OwnsMany(e => e.Skills, s =>
        {
            s.WithOwner().HasForeignKey("EmployeeId");
            s.Property<Guid>("Id").ValueGeneratedOnAdd();
            s.HasKey("Id");
            s.Property(p => p.Name).HasMaxLength(100).IsRequired();
            s.Property(p => p.Level).HasConversion<string>().HasMaxLength(20);
            s.Property(p => p.AcquiredDate).IsRequired();
            s.ToTable("EmployeeSkills");
        });

        // Certifications
        builder.OwnsMany(e => e.Certifications, c =>
        {
            c.WithOwner().HasForeignKey("EmployeeId");
            c.Property<Guid>("Id").ValueGeneratedOnAdd();
            c.HasKey("Id");
            c.Property(p => p.Name).HasMaxLength(100).IsRequired();
            c.Property(p => p.IssuingOrganization).HasMaxLength(100).IsRequired();
            c.Property(p => p.IssueDate).IsRequired();
            c.Property(p => p.ExpirationDate);
            c.ToTable("EmployeeCertifications");
        });

        // Education History
        builder.OwnsMany(e => e.EducationHistory, ed =>
        {
            ed.WithOwner().HasForeignKey("EmployeeId");
            ed.Property<Guid>("Id").ValueGeneratedOnAdd();
            ed.HasKey("Id");
            ed.ToTable("EmployeeEducationHistory");
        });

        // Leave Requests
        builder.Metadata
            .FindNavigation(nameof(Employee.LeaveRequests))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(e => e.LeaveRequests)
            .WithOne(lr => lr.Employee)
            .HasForeignKey("EmployeeId")
            .OnDelete(DeleteBehavior.Cascade);

        // Offboarding Checklists
        builder.Metadata
            .FindNavigation(nameof(Employee.OffboardingChecklists))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
        
        builder.HasMany(e => e.OffboardingChecklists)
            .WithOne()
            .HasForeignKey("EmployeeId")
            .OnDelete(DeleteBehavior.Cascade);
        
        // Performance Reviews
        builder.Metadata
            .FindNavigation(nameof(Employee.PerformanceReviews))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
        
        builder.HasMany(e => e.PerformanceReviews)
            .WithOne()
            .HasForeignKey("EmployeeId")
            .OnDelete(DeleteBehavior.Cascade);

        // Onboarding Tasks
        builder.Metadata
            .FindNavigation(nameof(Employee.OnboardingTasks))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
        
        builder.HasMany(e => e.OnboardingTasks)
            .WithOne()
            .HasForeignKey("EmployeeId")
            .OnDelete(DeleteBehavior.Cascade);

        
        // Attendance Records
        builder.OwnsMany(e => e.AttendanceRecords, a =>
        {
            a.WithOwner().HasForeignKey("EmployeeId");
            a.Property<Guid>("Id").ValueGeneratedOnAdd();
            a.HasKey("Id");
            a.Property(p => p.Date).IsRequired();
            a.Property(p => p.ClockInTime);
            a.Property(p => p.ClockOutTime);
            a.Property(p => p.Status).HasConversion<string>().IsRequired();
            a.Property(p => p.Notes).HasMaxLength(250);
            a.ToTable("EmployeeAttendanceRecords");
        });

        // Indexes
        builder.HasIndex(e => e.AzureAdId).IsUnique();
        builder.HasIndex(e => e.EmployeeNumber).IsUnique();
    }
}
