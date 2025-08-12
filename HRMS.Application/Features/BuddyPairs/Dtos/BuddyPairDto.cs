using HRMS.Domain.Enums;

namespace HRMS.Application.Features.BuddyPair.Dtos;

public class BuddyPairDto
{
    public Guid Id { get; set; }
    public Guid MentorId { get; set; }
    public string MentorName { get; set; } = null!;
    public string MentorDepartment { get; set; } = null!;
    public Guid MenteeId { get; set; }
    public string MenteeName { get; set; } = null!;
    public string MenteeDepartment { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public BuddyPairStatus Status { get; set; }
    public string AssignedBy { get; set; } = null!;
    public string? Notes { get; set; }
}