using HRMS.Application.Common.Interfaces;
using HRMS.Domain.Aggregates.DigitalSignatureAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Interfaces.Repositories
{
    public interface IDigitalSignatureRepository : IGenericRepository<DigitalSignature>
    {
        Task<IEnumerable<DigitalSignature>> GetByEmployeeAsync(Guid employeeId);
        Task<IEnumerable<DigitalSignature>> GetPendingByEmployeeAsync(Guid employeeId);
    }
}
