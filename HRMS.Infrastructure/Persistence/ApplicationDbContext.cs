using HRMS.Application.Common.Interfaces;
using HRMS.Domain.Aggregates.DepartmentAggregate;
using HRMS.Domain.Aggregates.EmployeeAggregate;
using HRMS.Domain.Aggregates.LeaveAggregate;
using HRMS.Domain.Aggregates.PayrollAggregate;
using HRMS.Domain.Aggregates.PositionAggregate;
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Owned<PhoneNumber>();

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync(cancellationToken);
        return result;
    }
}