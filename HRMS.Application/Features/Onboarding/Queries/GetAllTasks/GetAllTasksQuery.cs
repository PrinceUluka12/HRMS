using HRMS.Application.Features.Employees.Dtos;
using HRMS.Application.Interfaces.Repositories;
using MediatR;

namespace HRMS.Application.Features.Onboarding.Queries.GetAllTasks;

public sealed record GetAllTasksQuery() : IRequest<IEnumerable<OnboardingTaskDto>>;

public class GetAllTasksQueryHandler(IOnboardingRepository onboardingRepository) : IRequestHandler<GetAllTasksQuery, IEnumerable<OnboardingTaskDto>>
{
    public async Task<IEnumerable<OnboardingTaskDto>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var tasks = new List<OnboardingTaskDto>();
            var data = await onboardingRepository.GetAllDataAsync();

            foreach (var stage in data)
            {
                foreach (var task in stage.Stages)
                {
                    foreach (var taskData in task.Tasks)
                    {
                        var tas = new OnboardingTaskDto();
                        
                        tas.Description = taskData.Description;
                        tas.Status = taskData.Status.ToString();
                        tas.DueDate = taskData.DueDate;
                        tas.AssignedTo = taskData.AssignedTo;
                        tas.CompletedDate = taskData.CompletedDate;
                        tas.Notes = taskData.Notes;
                        tas.TaskId = taskData.Id;
                        tas.TaskName = taskData.TaskName;
                        tasks.Add(tas);
                    }
                }
            }
            return tasks;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}