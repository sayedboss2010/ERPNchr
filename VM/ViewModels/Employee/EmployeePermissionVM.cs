using EF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.ViewModels.Employee
{
    public class EmployeePermissionVM
    {
        public long Id { get; set; }

        public long? EmployeeId { get; set; }
        public string? DepartmentName { get; set; }
        public string? EmplyeeName { get; set; }
        public string? PermissionTypeName { get; set; }
        [Required(ErrorMessage = "❌ تاريخ بداية المامورية إجباري")]
        public DateOnly? DateOfPermission { get; set; }

        public bool IsActive { get; set; }

        public int? CreatedUserId { get; set; }

        public DateOnly? CreatedDate { get; set; }

        public int? UpdatedUserId { get; set; }

        public DateOnly? UpdatedDate { get; set; }

        public int? DeletedUserId { get; set; }

        public DateOnly? DeletedDate { get; set; }

        public int? PermissionTypeId { get; set; }

        /// <summary>
        /// موافقة المدير المباشر
        /// </summary>
        public bool? DirectManagerApproval { get; set; }

        /// <summary>
        /// موافقة مدير الادارة
        /// </summary>
        public bool? DepartmentManagerApproval { get; set; }

        public virtual PermissionsType? PermissionType { get; set; }
    }
}
