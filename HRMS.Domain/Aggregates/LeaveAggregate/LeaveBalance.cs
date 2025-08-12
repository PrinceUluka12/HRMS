using System;
using HRMS.Domain.Enums;

namespace HRMS.Domain.Aggregates.LeaveAggregate
{
    public class TimeOffBalance
    {
        public Guid Id { get; private set; }
        public Guid EmployeeId { get; private set; }
        public LeaveType LeaveType { get; private set; }

        public decimal TotalAllowed { get; private set; }    // Annual allocation
        public decimal Used { get; private set; }            // Days used this year
        public decimal Pending { get; private set; }         // Days in pending requests
        public decimal Available => TotalAllowed + CarryOver - Used - Pending; // Remaining available days

        public decimal CarryOver { get; private set; }       // Days carried from previous year
        public decimal AccrualRate { get; private set; }     // Days accrued per month

        public DateTime LastAccrualDate { get; private set; }
        public int PolicyYear { get; private set; }          // Policy year (e.g., 2024)

        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        private TimeOffBalance() { }

        public TimeOffBalance(
            Guid id,
            Guid employeeId,
            LeaveType leaveType,
            decimal totalAllowed,
            decimal used,
            decimal pending,
            decimal carryOver,
            decimal accrualRate,
            DateTime lastAccrualDate,
            int policyYear,
            DateTime createdAt,
            DateTime updatedAt)
        {
            Id = id;
            EmployeeId = employeeId;
            LeaveType = leaveType;
            TotalAllowed = totalAllowed;
            Used = used;
            Pending = pending;
            CarryOver = carryOver;
            AccrualRate = accrualRate;
            LastAccrualDate = lastAccrualDate;
            PolicyYear = policyYear;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        // Example method to update used days
        public void UseDays(decimal days)
        {
            if (days <= 0)
                throw new ArgumentException("Days to use must be positive.", nameof(days));

            if (days > Available)
                throw new InvalidOperationException("Insufficient available days.");

            Used += days;
            UpdatedAt = DateTime.UtcNow;
        }

        // Example method to accrue days
        public void Accrue(decimal days, DateTime accrualDate)
        {
            if (days <= 0)
                throw new ArgumentException("Days to accrue must be positive.", nameof(days));

            if (accrualDate <= LastAccrualDate)
                throw new InvalidOperationException("Accrual date must be after last accrual date.");

            TotalAllowed += days;
            LastAccrualDate = accrualDate;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
