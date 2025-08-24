namespace HRMS.Infrastructure.External;

public class JobBoardApiClient
{
    public async Task<bool> PostJobAsync(Guid jobVacancyId, string jobBoardName)
    {
        // TODO: Implement external API call to post job to job board
        await Task.Delay(100); // simulate async work
        return true;
    }
}