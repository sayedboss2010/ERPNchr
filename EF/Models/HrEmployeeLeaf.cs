using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// أجازات الموظف
/// </summary>
public partial class HrEmployeeLeaf
{
    public long Id { get; set; }

    public long? EmployeeId { get; set; }

    public byte? LeaveTypeId { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    /// <summary>
    /// السبب
    /// </summary>
    public string? Reason { get; set; }

    public bool IsActive { get; set; }

    public int? CreatedUserId { get; set; }

    public DateOnly? CreatedDate { get; set; }

    public int? UpdatedUserId { get; set; }

    public DateOnly? UpdatedDate { get; set; }

    public int? DeletedUserId { get; set; }

    public DateOnly? DeletedDate { get; set; }

    public bool FinalApproval { get; set; }

    public long? HrEmployeeLeaveBalanceId { get; set; }

    public virtual HrEmployee? Employee { get; set; }

    public virtual HrEmployeeLeaveBalance? HrEmployeeLeaveBalance { get; set; }

    public virtual HrLeaveType? LeaveType { get; set; }
}
