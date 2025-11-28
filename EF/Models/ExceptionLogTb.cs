using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class ExceptionLogTb
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string? Exception { get; set; }

    public string? ExceptionType { get; set; }

    public DateTime? ExceptionTime { get; set; }

    public virtual PrUser? User { get; set; }
}
