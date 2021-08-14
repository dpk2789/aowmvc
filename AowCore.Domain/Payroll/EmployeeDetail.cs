using AowCore.Domain.Common;
using System;
using System.Collections.Generic;

namespace AowCore.Domain.Payroll
{
    public class EmployeeDetail : AuditableEntity<Guid>
    {
        public DateTime? DateOfBirth { get; set; }
        public DateTime? JoiningDate { get; set; }
        public string FullName { get; set; }
        public string EnrollmentNumber { get; set; }
        public decimal? WorkHours { get; set; }
        public decimal? OverTimeHours { get; set; }
        public decimal? WorkHourTotal { get; set; }
        public decimal? RatePerHour { get; set; }
        public decimal? RatePerHourOvertime { get; set; }
        public decimal? OverTimeTotal { get; set; }
        public decimal? DaySalary { get; set; }
        public string AttendenceType { get; set; }
        public virtual IList<EmpAttendence> EmpAttendences { get; set; }
        public Guid LedgerId { get; set; }
        public virtual Ledger Ledger { get; set; }

    }
}
