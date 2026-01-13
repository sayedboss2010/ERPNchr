using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class AttendanceDeviceLog
{
    public long Id { get; set; }

    public byte DeviceId { get; set; }

    public DateTimeOffset ScanTime { get; set; }

    public string ScanType { get; set; } = null!;

    public bool Verified { get; set; }

    public string Source { get; set; } = null!;

    public string EnrollNumber { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }

    public virtual HrMachineIp Device { get; set; } = null!;
}
