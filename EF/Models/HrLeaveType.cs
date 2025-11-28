using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// أنواع الأجازات
/// </summary>
public partial class HrLeaveType
{
    public byte Id { get; set; }

    public string? NameAr { get; set; }

    public string? NameEn { get; set; }

    public bool IsActive { get; set; }

    public int? CreatedUserId { get; set; }

    public DateOnly? CreatedDate { get; set; }

    public int? UpdatedUserId { get; set; }

    public DateOnly? UpdatedDate { get; set; }

    public int? DeletedUserId { get; set; }

    public DateOnly? DeletedDate { get; set; }

    public virtual ICollection<HrEmployeeLeaf> HrEmployeeLeaves { get; set; } = new List<HrEmployeeLeaf>();
}
