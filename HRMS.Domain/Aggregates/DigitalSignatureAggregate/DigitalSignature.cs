using HRMS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Domain.Aggregates.DigitalSignatureAggregate
{
    public sealed class DigitalSignature
    {
        public Guid Id { get; private set; }
        public Guid EmployeeId { get; private set; }
        public Guid DocumentId { get; private set; }
        public string DocumentName { get; private set; }
        public DocumentType DocumentType { get; private set; }
        public string SignatureData { get; private set; } // Base64
        public DateTime SignedAt { get; private set; }
        public string IpAddress { get; private set; }
        public SignatureStatus Status { get; private set; }
        public DateTime? RequiredBy { get; private set; }


        // EF Core requires a parameterless ctor (can be internal/protected)
        protected DigitalSignature() { }


        public DigitalSignature(Guid employeeId, Guid documentId, string documentName, DocumentType documentType, string signatureData, string ipAddress, DateTime? requiredBy = null)
        {
            Id = Guid.NewGuid();
            EmployeeId = employeeId;
            DocumentId = documentId;
            DocumentName = documentName;
            DocumentType = documentType;
            SignatureData = signatureData ?? throw new ArgumentNullException(nameof(signatureData));
            SignedAt = DateTime.UtcNow;
            IpAddress = ipAddress;
            Status = SignatureStatus.Completed; // created via "create" implies completed signature
            RequiredBy = requiredBy;
        }


        public void MarkPending()
        {
            Status = SignatureStatus.Pending;
        }


        public void Cancel()
        {
            Status = SignatureStatus.Cancelled;
        }


        public void Expire()
        {
            Status = SignatureStatus.Expired;
        }


        public void UpdateSignature(string base64Signature, string ipAddress)
        {
            SignatureData = base64Signature ?? throw new ArgumentNullException(nameof(base64Signature));
            IpAddress = ipAddress;
            SignedAt = DateTime.UtcNow;
            Status = SignatureStatus.Completed;
        }
    }
}
