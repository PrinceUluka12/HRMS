using HRMS.Application.Features.TimeTracking.Dtos;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Interfaces.Services.Contracts;
using HRMS.Application.Wrappers;
using HRMS.Domain.Aggregates.TimeTrackingAggregate;
//using HRMS.Domain.Aggregates.TimeTrackingAggregate;
using HRMS.Domain.Enums;
using HRMS.Domain.Interfaces;

namespace HRMS.Application.Interfaces.Services;

public class TimeTrackingService(ITimeEntryRepository repository, IUnitOfWork unitOfWork) : ITimeTrackingService
{
    public async Task<Guid> ClockInAsync(Guid employeeId, DateOnly date, TimeOnly time, decimal latitude,
        decimal longitude)
    {
        await unitOfWork.BeginTransactionAsync(CancellationToken.None);
        try
        {
            var entry = new TimeEntry(Guid.NewGuid(), employeeId, date, time, 0);
            var location = new TimeTrackingLocation(Guid.NewGuid(), entry.Id, ClockActionType.ClockIn, DateTime.UtcNow,
                latitude, longitude);
            entry.AddLocation(location);

            await repository.AddAsync(entry);
            await unitOfWork.CommitTransactionAsync();


            return entry.Id;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            await unitOfWork.RollbackTransactionAsync();

            throw;
        }
    }

    public async Task ClockOutAsync(Guid timeEntryId, TimeOnly time, decimal latitude, decimal longitude)
    {
        await unitOfWork.BeginTransactionAsync(CancellationToken.None);
        try
        {
            var entry = await repository.GetByIdWithIncludesAsync(timeEntryId);
            if (entry == null) throw new Exception("Time entry not found");

            entry.SetClockOut(time);
            entry.AddLocation(new TimeTrackingLocation(Guid.NewGuid(), timeEntryId, ClockActionType.ClockOut,
                DateTime.UtcNow, latitude, longitude));
            entry.MarkCompleted();
            repository.Update(entry);
            await unitOfWork.CommitTransactionAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            await unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task AddLocationAsync(Guid timeEntryId, ClockActionType actionType, decimal latitude,
        decimal longitude, DateTime timestamp)
    {
        var entry = await repository.GetByIdWithIncludesAsync(timeEntryId);
        if (entry == null) throw new Exception("Time entry not found");

        entry.AddLocation(new TimeTrackingLocation(Guid.NewGuid(), timeEntryId, actionType, timestamp, latitude,
            longitude));
        await unitOfWork.SaveChangesAsync();
    }

    public async Task CompleteEntryAsync(Guid timeEntryId)
    {
        var entry = await repository.GetByIdWithIncludesAsync(timeEntryId);
        if (entry == null) throw new Exception("Time entry not found");

        entry.MarkCompleted();
        await unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<TimeEntry>> GetByEmployeeIdAsync(Guid id)
    {
        var entry = await repository.GetEntryByEmployeeId(id);
        
        if (entry == null) throw new Exception("Time entry not found");
        return entry;
    }

    public async Task<Guid> ManualEntryAsync(ManualEntryDto request)
    {
        await unitOfWork.BeginTransactionAsync(CancellationToken.None);
        try
        {
            var entry = new TimeEntry(Guid.NewGuid(), request.EmployeeId, request.Date, request.StartTime, request.TotalHours);
            var location = new TimeTrackingLocation(Guid.NewGuid(), entry.Id, ClockActionType.ClockIn, DateTime.Now,
                request.Latitude, request.Longitude);
            entry.AddLocation(location);
            entry.SetDescription(request.Description); 
            entry.SetProject(request.Project);

            entry.SetClockOut(request.EndTime);

            var location1 = new TimeTrackingLocation(Guid.NewGuid(), entry.Id, ClockActionType.ClockOut, DateTime.Now,
                request.Latitude, request.Longitude);
            entry.AddLocation(location1);

            entry.MarkCompleted();
            
            await repository.AddAsync(entry);
            await unitOfWork.CommitTransactionAsync();
            
            return entry.Id;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}