using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Domain.Aggregates.DigitalSignatureAggregate.Events
{
    public sealed class SignatureCreatedEvent
    {
        public Guid SignatureId { get; }
        public Guid EmployeeId { get; }
        public Guid DocumentId { get; }


        public SignatureCreatedEvent(Guid signatureId, Guid employeeId, Guid documentId)
        {
            SignatureId = signatureId;
            EmployeeId = employeeId;
            DocumentId = documentId;
        }
    }
}
