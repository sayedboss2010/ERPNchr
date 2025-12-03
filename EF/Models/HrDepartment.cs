using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// الإدارات
/// </summary>
public partial class HrDepartment
{
    public int Id { get; set; }

    public string? NameAr { get; set; }

    public string? NameEn { get; set; }

    public bool IsActive { get; set; }

    public int? CreatedUserId { get; set; }

    public DateOnly? CreatedDate { get; set; }

    public int? UpdatedUserId { get; set; }

    public DateOnly? UpdatedDate { get; set; }

    public int? DeletedUserId { get; set; }

    public DateOnly? DeletedDate { get; set; }

    public virtual ICollection<HrBranchDepartment> HrBranchDepartments { get; set; } = new List<HrBranchDepartment>();

    public virtual ICollection<HrEmployeeOfficialMission> HrEmployeeOfficialMissions { get; set; } = new List<HrEmployeeOfficialMission>();

    public virtual ICollection<HrEmployee> HrEmployees { get; set; } = new List<HrEmployee>();
}
