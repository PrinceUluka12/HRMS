using HRMS.Application.Common.Interfaces;

namespace HRMS.Application.Interfaces.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}