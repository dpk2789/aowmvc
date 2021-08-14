using AowCore.Domain.Common;
using System;

namespace AowCore.Domain.Payroll
{
    public class EmpAttendence : AuditableEntity<Guid>
    {
        public int SrNo { get; set; }
        public string MachineNumber { get; set; }
        public string FullName { get; set; }
        public string EnrollmentNumber { get; set; }
        public string Mode { get; set; }
        public DateTime Date { get; set; }
        public DateTime PunchTime { get; set; }
        public Guid EmployeeDetailId { get; set; }
        public virtual EmployeeDetail EmployeeDetail { get; set; }
    }
}
