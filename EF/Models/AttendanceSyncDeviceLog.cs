using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class AttendanceSyncDeviceLog
{
    public int Id { get; set; }

    public DateTime RunTime { get; set; }

    public string DeviceIp { get; set; } = null!;

    public int NewLogs { get; set; }

    public string Status { get; set; } = null!;

    public string? Message { get; set; }
}
