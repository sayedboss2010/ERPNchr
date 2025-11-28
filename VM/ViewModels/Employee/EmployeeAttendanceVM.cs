using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.ViewModels
{
    public class EmployeeAttendanceVM
    {
        public string EmployeeName { get; set; }
        public string EmpCode { get; set; }
        public string jopName { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly? CheckIn { get; set; }
        public TimeOnly? CheckOut { get; set; }
        public TimeSpan? Delay { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
    }

}
