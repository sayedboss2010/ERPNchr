using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// إدارات الفرع
/// </summary>
public partial class HrBranchDepartment
{
    public int Id { get; set; }

    public int? BranchId { get; set; }

    public int? DepartmentId { get; set; }

    public bool IsActive { get; set; }

    public int? CreatedUserId { get; set; }

    public DateOnly? CreatedDate { get; set; }

    public int? UpdatedUserId { get; set; }

    public DateOnly? UpdatedDate { get; set; }

    public int? DeletedUserId { get; set; }

    public DateOnly? DeletedDate { get; set; }

    public virtual HrBranch? Branch { get; set; }

    public virtual HrDepartment? Department { get; set; }
}
