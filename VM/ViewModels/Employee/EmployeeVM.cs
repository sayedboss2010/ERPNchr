using EF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.ViewModels
{
    public class EmployeeVM
    {
        public bool Disability { get; set; }
        public long Id { get; set; }
        public string EmolyeetypeName { get; set; }
        public string DepartmentName { get; set; }
        public string BranchName { get; set; }
        public string JobName { get; set; }

        /// <summary>
        /// كود الموظف
        /// </summary>
        public long? EmpCode { get; set; }

        public string? NameAr { get; set; }

        public string? NameEn { get; set; }
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة")]
        public string? Email { get; set; }

        public string? Password { get; set; }

        public string? AddressAr { get; set; }

        public string? AddressEn { get; set; }

        public string? PhoneNumber { get; set; }

        public DateOnly? Birthdate { get; set; }

        public DateOnly? HireDate { get; set; }

        public int? CurrentJobId { get; set; }

        /// <summary>
        /// ملوش لازمه
        /// </summary>
        public decimal? CurrentSalary { get; set; }

        public byte? CurrentFunctionalDegreeId { get; set; }

        public bool IsMananger { get; set; }

        public bool IsActive { get; set; }

        public int? CreatedUserId { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? UpdatedUserId { get; set; }

        public DateOnly? UpdatedDate { get; set; }

        public int? DeletedUserId { get; set; }

        public DateOnly? DeletedDate { get; set; }

        /// <summary>
        /// الرقم التأميني
        /// </summary>
        public string? InsuranceNumber { get; set; }

        /// <summary>
        /// الرقم القومي
        /// </summary>
        [Required(ErrorMessage = "الرقم القومي مطلوب")]
       
        public string? Nid { get; set; }

        /// <summary>
        /// تاريخ دخول للتأمين
        /// </summary>
        public DateOnly? DateIn { get; set; }

        /// <summary>
        /// تاريخ الخروج من التأمين
        /// </summary>
        public DateOnly? DateOut { get; set; }

        public string? NidPath { get; set; }

        public string? Mobile { get; set; }

        public string? EmpCodeNew { get; set; }

        public bool? Isbank { get; set; }

        /// <summary>
        /// تاريخ التعيين
        /// </summary>
        public DateOnly? AppointmentDate { get; set; }

        public int? HrJobGradesId { get; set; }

        public int? EmployeeTypeId { get; set; }

        public int? BranchId { get; set; }

        public int? DepartmentId { get; set; }

        public virtual HrBranch? Branch { get; set; }

        public virtual HrJob? CurrentJob { get; set; }

        public virtual HrDepartment? Department { get; set; }

        public virtual EmployeeType? EmployeeType { get; set; }

        public virtual ICollection<HrEmployeeAttendance> HrEmployeeAttendances { get; set; } = new List<HrEmployeeAttendance>();

        public virtual ICollection<HrEmployeeLateness> HrEmployeeLatenesses { get; set; } = new List<HrEmployeeLateness>();

        public virtual ICollection<HrEmployeeLeaveBalance> HrEmployeeLeaveBalances { get; set; } = new List<HrEmployeeLeaveBalance>();

        public virtual ICollection<HrEmployeeLeaf> HrEmployeeLeaves { get; set; } = new List<HrEmployeeLeaf>();

        public virtual ICollection<PrUser> PrUsers { get; set; } = new List<PrUser>();
    }
}
