using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// الوظائف
/// </summary>
public partial class HrJob
{
    public int Id { get; set; }

    public string? TitleAr { get; set; }

    public string? TitleEn { get; set; }

    public decimal? MinSalary { get; set; }

    public decimal? MaxSalary { get; set; }

    public bool IsActive { get; set; }

    public int? CreatedUserId { get; set; }

    public DateOnly? CreatedDate { get; set; }

    public int? UpdatedUserId { get; set; }

    public DateOnly? UpdatedDate { get; set; }

    public int? DeletedUserId { get; set; }

    public DateOnly? DeletedDate { get; set; }

    public virtual ICollection<HrEmployee> HrEmployees { get; set; } = new List<HrEmployee>();
}
