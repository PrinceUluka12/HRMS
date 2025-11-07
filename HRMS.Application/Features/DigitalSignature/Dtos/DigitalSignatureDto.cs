using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Features.DigitalSignature.Dtos
{
    public class DigitalSignatureDto
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid DocumentId { get; set; }
        public string DocumentName { get; set; }
        public string DocumentType { get; set; }
        public string SignatureData { get; set; }
        public DateTime SignedAt { get; set; }
        public string IpAddress { get; set; }
        public string Status { get; set; }
        public DateTime? RequiredBy { get; set; }
    }
}
