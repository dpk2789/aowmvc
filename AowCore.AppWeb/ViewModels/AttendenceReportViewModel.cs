using System;
using System.Collections.Generic;

namespace AowCore.AppWeb.ViewModels
{
    public class AttendenceReportViewModel
    {
        public int Id { get; set; }
        public Guid EmpId { get; set; }
        public IEnumerable<EmpAttendenceViewModel> Files { get; set; }
    }
}
