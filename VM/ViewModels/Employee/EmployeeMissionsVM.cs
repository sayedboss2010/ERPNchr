using EF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.ViewModels.Employee
{
    public class EmployeeMissionsVM
    {
        public int EmployeeUserType { get; set; }
        public string? AttachmentPath { get; set; }
        public int? Id { get; set; }
        public string? EmplyeeName { get; set; }
        public int? EmployeeId { get; set; }
        public string? DepartmentName { get; set; }
        public int? DepartmentId { get; set; }

        public string? AuthorityOfMission { get; set; }
        public string? PurposeOfMission { get; set; }

        [Required(ErrorMessage = "❌ تاريخ بداية المأمورية إجباري")]
        public DateOnly? StartDate { get; set; }

        [Required(ErrorMessage = "❌ تاريخ نهاية المأمورية إجباري")]
        public DateOnly? EndDate { get; set; }

        public bool? IsActive { get; set; }

        public bool? DirectManagerApproval { get; set; }
        public bool? DepartmentManagerApproval { get; set; }
    }


}
