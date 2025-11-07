using HRMS.Domain.Aggregates.DigitalSignatureAggregate;
using HRMS.Domain.Aggregates.LeaveAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Persistence.Configurations
{
    public class DigitalSignatureConfiguration : IEntityTypeConfiguration<DigitalSignature>
    {
        public void Configure(EntityTypeBuilder<DigitalSignature> b)
        {
            b.ToTable("digital_signatures");
            b.HasKey(x => x.Id);
            b.Property(x => x.EmployeeId).IsRequired();
            b.Property(x => x.DocumentId).IsRequired();
            b.Property(x => x.DocumentName).HasMaxLength(256);
            b.Property(x => x.DocumentType).HasConversion<string>();
            b.Property(x => x.SignatureData).IsRequired();
            b.Property(x => x.SignedAt);
            b.Property(x => x.IpAddress).HasMaxLength(45);
            b.Property(x => x.Status).HasConversion<string>();
            b.Property(x => x.RequiredBy);
        }
    }
}
