using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class LogInHistoryTb
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public DateTime? LogInDate { get; set; }

    public DateTime? LogOutDate { get; set; }

    public virtual PrUser? User { get; set; }
}
