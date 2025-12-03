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
        public int? Id { get; set; }
        public string? EmplyeeName { get; set; }
        public int? EmployeeId { get; set; }
        public string? DepartmentName { get; set; }
        public int? DepartmentId { get; set; }

        public string? AuthorityOfMission { get; set; }

        public string? PurposeOfMission { get; set; }
        [Required(ErrorMessage = "❌ تاريخ بداية المامورية إجباري")]
        public DateOnly? StartDate { get; set; }
        [Required(ErrorMessage = "❌ تاريخ نهاية المامورية إجباري")]
        public DateOnly? EndDate { get; set; }

        public bool? IsActive { get; set; }

        public int? CreatedUserId { get; set; }

        public DateOnly? CreatedDate { get; set; }

        public int? UpdatedUserId { get; set; }

        public DateOnly? UpdatedDate { get; set; }

        public int? DeletedUserId { get; set; }

        public DateOnly? DeletedDate { get; set; }

        /// <summary>
        /// موافقة المدير المباشر
        /// </summary>
        public bool? DirectManagerApproval { get; set; }

        /// <summary>
        /// موافقة مدير الادارة
        /// </summary>
        public bool? DepartmentManagerApproval { get; set; }

        public virtual HrDepartment? Department { get; set; }

        public virtual HrEmployee? Employee { get; set; }

    }
   
}
