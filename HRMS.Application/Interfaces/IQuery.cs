using MediatR;

namespace HRMS.Application.Interfaces;

public interface IQuery<TResponse> : IRequest<TResponse>
{
}