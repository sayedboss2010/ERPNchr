using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class AttendanceSyncErrorLog
{
    public int Id { get; set; }

    public DateTime RunTime { get; set; }

    public string? DeviceIp { get; set; }

    public string ErrorMessage { get; set; } = null!;
}
