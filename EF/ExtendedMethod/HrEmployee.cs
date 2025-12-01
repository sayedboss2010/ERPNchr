using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;



// HR_EmployeesExtension.cs
namespace EF.Models;
    public partial class HrEmployee
    {
        public Nullable<int> Year { get; set; }
        public Nullable<int> Month { get; set; }
        public string DepartmentName { get; set; }
        public string JobName { get; set; }


}
