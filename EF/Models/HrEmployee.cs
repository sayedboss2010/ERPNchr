using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// الموظفين
/// </summary>
public partial class HrEmployee
{
    public long Id { get; set; }

    /// <summary>
    /// كود الموظف
    /// </summary>
    public long? EmpCode { get; set; }

    public string? NameAr { get; set; }

    public string? NameEn { get; set; }

    public string? AddressAr { get; set; }

    public string? AddressEn { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public DateOnly? Birthdate { get; set; }

    public DateOnly? HireDate { get; set; }

    public int? CurrentJobId { get; set; }

    public int? CurrentBranchDeptId { get; set; }

    /// <summary>
    /// ملوش لازمه
    /// </summary>
    public decimal? CurrentSalary { get; set; }

    public byte? CurrentFunctionalDegreeId { get; set; }

    public bool IsManager { get; set; }

    public bool IsActive { get; set; }

    public int? CreatedUserId { get; set; }

    public DateOnly? CreatedDate { get; set; }

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

    public virtual HrBranchDepartment? CurrentBranchDept { get; set; }

    public virtual HrJob? CurrentJob { get; set; }

    public virtual ICollection<HrEmployeeAttendance> HrEmployeeAttendances { get; set; } = new List<HrEmployeeAttendance>();

    public virtual ICollection<HrEmployeeLateness> HrEmployeeLatenesses { get; set; } = new List<HrEmployeeLateness>();

    public virtual ICollection<HrEmployeeLeaveBalance> HrEmployeeLeaveBalances { get; set; } = new List<HrEmployeeLeaveBalance>();

    public virtual ICollection<HrEmployeeLeaf> HrEmployeeLeaves { get; set; } = new List<HrEmployeeLeaf>();

    public virtual ICollection<PrUser> PrUsers { get; set; } = new List<PrUser>();
}
