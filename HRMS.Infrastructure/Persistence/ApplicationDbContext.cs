using HRMS.Application.Common.Interfaces;
using HRMS.Application.Features.Departments.Dtos;
using HRMS.Domain.Aggregates.DepartmentAggregate;
using HRMS.Domain.Aggregates.EmployeeAggregate;
using HRMS.Domain.Aggregates.LeaveAggregate;
using HRMS.Domain.Aggregates.NotificationAggregate;
using HRMS.Domain.Aggregates.OnboardingAggregate;
using HRMS.Domain.Aggregates.PayrollAggregate;
using HRMS.Domain.Aggregates.PositionAggregate;
using HRMS.Domain.Aggregates.TimeTrackingAggregate;
//using HRMS.Domain.Aggregates.TimeTrackingAggregate;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Position> Positions { get; set; }
    public DbSet<Payroll> Payrolls { get; set; }
    public DbSet<LeaveRequest> LeaveRequests { get; set; }
    public DbSet<Onboarding> Onboardings { get; set; }
    public DbSet<TimeEntry> TimeEntries { get; set; }
    public DbSet<TimeOffBalance> TimeOffBalances { get; set; }
    public DbSet<Equipment> Equipments { get; set; }
    public DbSet<EquipmentAssignment> EquipmentAssignments { get; set; }
    public DbSet<BuddyPair> BuddyPairs { get; set; }
    public DbSet<BuddyCheckIn> BuddyCheckIns { get; set; }
    public DbSet<OnboardingWorkflow> OnboardingWorkflows { get; set; }
    public DbSet<OnboardingStep> OnboardingSteps { get; set; }
    
    
    public DbSet<Notification> Notifications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder
            .Entity<DepartmentWithManagerView>(builder =>
            {
                builder.HasNoKey(); // Views usually don’t have a PK
                builder.ToView("vw_DepartmentsWithManagers"); // Map to the SQL view name
            });
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync(cancellationToken);
        return result;
    }
}