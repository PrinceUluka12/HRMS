namespace HRMS.Domain.Exceptions;

public class AzureAdException : DomainException
{
    public AzureAdException(string message) : base(message) { }
    public AzureAdException(string message, Exception inner) : base(message, inner) { }
}