using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Features.DigitalSignature.Dtos
{
    public class SignatureRequestDto
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public Guid DocumentId { get; set; }
        public string DocumentName { get; set; }
        public string DocumentType { get; set; }
        public DateTime RequestedAt { get; set; }
        public DateTime? RequiredBy { get; set; }
        public string Status { get; set; }
        public int RemindersSent { get; set; }
    }
}
