using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class VwEmployeeActivity
{
    public long? EmployeeId { get; set; }

    public string? EmployeeName { get; set; }

    public int? DepartmentId { get; set; }

    public int? BranchId { get; set; }

    public string? DepartmentName { get; set; }

    public string? BranchName { get; set; }

    public string RecordType { get; set; } = null!;

    public string? Details { get; set; }

    public string? ExtraInfo { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public bool IsActive { get; set; }

    public string DirectManagerStatus { get; set; } = null!;

    public string DeptManagerStatus { get; set; } = null!;

    public int? LeaveDays { get; set; }

    public DateOnly? CreatedDate { get; set; }
}
