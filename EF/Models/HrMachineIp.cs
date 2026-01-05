using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// ������� ������
/// </summary>
public partial class HrMachineIp
{
    public byte Id { get; set; }

    public string? MachineNameAr { get; set; }

    public string? MachineNameEn { get; set; }

    public string? MachineIp { get; set; }

    public int? BranchId { get; set; }

    public bool IsActive { get; set; }

    public int? CreatedUserId { get; set; }

    public DateOnly? CreatedDate { get; set; }

    public int? UpdatedUserId { get; set; }

    public DateOnly? UpdatedDate { get; set; }

    public int? DeletedUserId { get; set; }

    public DateOnly? DeletedDate { get; set; }

    /// <summary>
    /// اخر موعد لسحب البصمات
    /// </summary>
    public DateOnly? LastPullDate { get; set; }

    public int? Port { get; set; }

    public virtual ICollection<AttendanceDeviceLog> AttendanceDeviceLogs { get; set; } = new List<AttendanceDeviceLog>();

    public virtual HrBranch? Branch { get; set; }

    public virtual ICollection<HrEmployeeAttendance> HrEmployeeAttendances { get; set; } = new List<HrEmployeeAttendance>();
}
