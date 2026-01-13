using EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.ViewModels.Employee;

namespace VM.ViewModels
{
    public class GlobalSearchVM
    {
        public List<HrJob> Jobs { get; set; } = new();
        public List<HrDepartment> Departments { get; set; } = new();
        public List<HrEmployee> EmployeeAttendance { get; set; } = new();
        public List<HrEmployeeOfficialMission> EmployeeMissions { get; set; } = new();
        public List<HrEmployeePermission> EmployeePermissionV { get; set; } = new();

    }

}
