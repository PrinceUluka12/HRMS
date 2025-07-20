namespace HRMS.Application.Interfaces.Services.Contracts;

public interface ITeamsIntegrationService
{
    Task SendTeamsNotificationAsync(string teamId, string channelId, string message);
    Task CreateTeamsMeetingAsync(string organizerId, IEnumerable<string> attendeeEmails, string subject, DateTime start, DateTime end, string body);
    Task<string> CreateTeamsChannelAsync(string teamId, string channelName, string description);
}