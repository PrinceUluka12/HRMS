using HRMS.Application.Interfaces.Services.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Graph.Models;

namespace HRMS.Application.Interfaces.Services;

public class TeamsIntegrationService(
    GraphServiceClient graphServiceClient,
    ILogger<TeamsIntegrationService> logger)
    : ITeamsIntegrationService
{
    public async Task SendTeamsNotificationAsync(string teamId, string channelId, string message)
    {
        try
        {
            var chatMessage = new ChatMessage
            {
                Body = new ItemBody
                {
                    ContentType = BodyType.Html,
                    Content = message
                }
            };

            await graphServiceClient.Teams[teamId]
                .Channels[channelId]
                .Messages
                .PostAsync(chatMessage);
        }
        catch (ServiceException ex)
        {
            logger.LogError(ex, "Error sending Teams notification to team {TeamId}, channel {ChannelId}", teamId, channelId);
            throw;
        }
    }

    public async Task CreateTeamsMeetingAsync(
        string organizerId,
        IEnumerable<string> attendeeEmails,
        string subject,
        DateTime start,
        DateTime end,
        string body)
    {
        try
        {
            var onlineMeeting = new OnlineMeeting
            {
                StartDateTime = start,
                EndDateTime = end,
                Subject = subject,
                Participants = new MeetingParticipants
                {
                    Organizer = new MeetingParticipantInfo
                    {
                        Identity = new IdentitySet
                        {
                            User = new Identity
                            {
                                Id = organizerId
                            }
                        }
                    },
                    Attendees = attendeeEmails.Select(email => new MeetingParticipantInfo
                    {
                        Identity = new IdentitySet
                        {
                            User = new Identity
                            {
                                Id = email
                            }
                        }
                    }).ToList()
                }
            };

            await graphServiceClient.Users[organizerId]
                .OnlineMeetings
                .PostAsync(onlineMeeting);
        }
        catch (ServiceException ex)
        {
            logger.LogError(ex, "Error creating Teams meeting for organizer {OrganizerId}", organizerId);
            throw;
        }
    }

    public async Task<string> CreateTeamsChannelAsync(string teamId, string channelName, string description)
    {
        throw new NotImplementedException();
    }
}