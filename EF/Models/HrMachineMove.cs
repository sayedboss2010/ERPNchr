using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// ��� ���� ������
/// </summary>
public partial class HrMachineMove
{
    public byte Id { get; set; }

    public string? MoveNameAr { get; set; }

    public string? MoveNameEn { get; set; }

    public bool IsActive { get; set; }

    public int? CreatedUserId { get; set; }

    public DateOnly? CreatedDate { get; set; }

    public int? UpdatedUserId { get; set; }

    public DateOnly? UpdatedDate { get; set; }

    public int? DeletedUserId { get; set; }

    public DateOnly? DeletedDate { get; set; }

    public virtual ICollection<HrEmployeeAttendance> HrEmployeeAttendances { get; set; } = new List<HrEmployeeAttendance>();
}
