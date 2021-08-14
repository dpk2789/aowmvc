using System;

namespace AowCore.AppWeb.ViewModels
{
    public class EmpAttendenceViewModel
    {
        public Guid Id { get; set; }
        public int SrNo { get; set; }
        public string MachineNumber { get; set; }
        public string FullName { get; set; }
        public string EnrollmentNumber { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan InTime { get; set; }
        public TimeSpan OutTime { get; set; }
        public decimal? OverTimeHours { get; set; }
        public decimal? WorkHourTotal { get; set; }
        public decimal? WorkMinutes { get; set; }
        public decimal? RatePerHour { get; set; }
        public decimal? RatePerHourOvertime { get; set; }
        public decimal? OverTimeTotal { get; set; }
    }
}
