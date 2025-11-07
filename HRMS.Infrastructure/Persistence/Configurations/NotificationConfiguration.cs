using HRMS.Domain.Aggregates.NotificationAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Persistence.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> b)
        {
            b.ToTable("Notifications");
            b.HasKey(x => x.Id);
            b.Property(x => x.EmployeeId).IsRequired(false);
            b.Property(x => x.Title).HasMaxLength(200).IsRequired();
            b.Property(x => x.Message).HasMaxLength(2000);
            b.Property(x => x.Type).HasConversion<string>();
            b.Property(x => x.Status).HasConversion<string>();
            b.Property(x => x.CreatedAt).IsRequired();
            b.Property(x => x.ExpiresAt).IsRequired(false);
            b.Property(x => x.RemindersSent).IsRequired();
            b.Property(x => x.PayloadJson)
    .HasColumnType("NVARCHAR(MAX)")
    .IsRequired(false);
        }
    }
}
