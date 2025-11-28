using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// ������ ���������
/// </summary>
public partial class HrEmployeeAttendance
{
    public long Id { get; set; }

    public long? EmployeeId { get; set; }

    public byte? MachineId { get; set; }

    public byte? MoveCodeId { get; set; }

    public DateOnly? ModeDate { get; set; }

    public TimeOnly? MoveTime { get; set; }

    public virtual HrEmployee? Employee { get; set; }

    public virtual HrMachineIp? Machine { get; set; }

    public virtual HrMachineMove? MoveCode { get; set; }
}
