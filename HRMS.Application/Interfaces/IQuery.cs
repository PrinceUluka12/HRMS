using MediatR;

namespace HRMS.Application.Common.Interfaces;

public interface IQuery<TResponse> : IRequest<TResponse>
{
}