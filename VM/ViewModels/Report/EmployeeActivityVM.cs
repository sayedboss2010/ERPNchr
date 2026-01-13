using EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.ViewModels
{
    public class EmployeeActivityVM
    {
        public int Employee_ID { get; set; }
        public string EmployeeName { get; set; }
        public int DepartmentID { get; set; }
        public int BranchID { get; set; }
        public string DepartmentName { get; set; }
        public string BranchName { get; set; }
        public string RecordType { get; set; }
        public string Details { get; set; }
        public string ExtraInfo { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public string DirectManagerStatus { get; set; }
        public string DeptManagerStatus { get; set; }
        public int? LeaveDays { get; set; }
        public DateTime Created_Date { get; set; }
    }

    public class EmployeeActivityReportVM
    {
        public List<VwEmployeeActivity> Activities { get; set; } = new();
        public List<RecordSummaryVM> Summary { get; set; } = new();
        public List<DepartmentRecordsVM> ByDepartment { get; set; } = new();
    }

    public class DepartmentRecordsVM
    {
        public string DepartmentName { get; set; }
        public List<VwEmployeeActivity> Records { get; set; } = new();
    }

    public class RecordSummaryVM
    {
        public string RecordType { get; set; }
        public int Count { get; set; }
    }

}

