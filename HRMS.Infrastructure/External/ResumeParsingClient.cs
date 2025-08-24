namespace HRMS.Infrastructure.External;

public class ResumeParsingClient
{
    public async Task<string> ParseResumeAsync(string resumeUrl)
    {
        // TODO: Call resume parsing service (e.g., external API)
        await Task.Delay(100);
        return "Parsed resume content or structured data";
    }
}