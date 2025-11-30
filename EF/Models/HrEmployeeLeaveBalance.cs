using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// رصيد الإجازات السنوي (اعتيادي + عارضة)
/// </summary>
public partial class HrEmployeeLeaveBalance
{
    public long Id { get; set; }

    /// <summary>
    /// رقم الموظف
    /// </summary>
    public long EmployeeId { get; set; }

    public int Year { get; set; }

    /// <summary>
    /// إجمالي الإجازات الاعتيادية في السنة
    /// </summary>
    public decimal TotalDays { get; set; }

    /// <summary>
    /// الايام المستخدمه فى السنة
    /// </summary>
    public decimal UsedDays { get; set; }

    /// <summary>
    /// إجمالي الإجازات العارضة في السنة
    /// </summary>
    public decimal CasualTotalDays { get; set; }

    /// <summary>
    /// ايام العارضة المستخدمة
    /// </summary>
    public decimal CasualUsedDays { get; set; }

    /// <summary>
    /// إجمالي الإجازات العارضة في السنة
    /// </summary>
    public decimal AnnualTotalDays { get; set; }

    /// <summary>
    /// ايام العارضة المستخدمة
    /// </summary>
    public decimal AnnualUsedDays { get; set; }

    public int? CreatedUserId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? UpdatedUserId { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool IsActive { get; set; }

    /// <summary>
    /// تم احتساب الإجازة على باقي الرصيد الاعتيادي (0 = لا، 1 = نعم)
    /// </summary>
    public bool IsAnnualBalanceCalculated { get; set; }

    /// <summary>
    /// باقي الايام العارضة
    /// </summary>
    public int? AnnualRemainingDays { get; set; }

    /// <summary>
    /// باقي الايام العارضة
    /// </summary>
    public int? CasualRemainingDays { get; set; }

    public int? TotalDaysReminig { get; set; }

    public virtual HrEmployee Employee { get; set; } = null!;

    public virtual ICollection<HrEmployeeLeaf> HrEmployeeLeaves { get; set; } = new List<HrEmployeeLeaf>();
}
