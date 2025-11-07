using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Wrappers;
using HRMS.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Features.Notifications.Queries
{
    public class GetUnreadCountQuery : IRequest<BaseResult<int>>
    {
        public Guid EmployeeId { get; set; }
    }

    public class GetUnreadCountHandler(INotificationRepository _repository) : IRequestHandler<GetUnreadCountQuery, BaseResult<int>>
    {
        public async Task<BaseResult<int>> Handle(GetUnreadCountQuery request, CancellationToken cancellationToken)
        {
            var data =  await _repository.GetUnreadCountAsync(request.EmployeeId);

            return BaseResult<int>.Ok(data);
        }
    }
}
