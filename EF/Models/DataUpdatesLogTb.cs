using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class DataUpdatesLogTb
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string? Operation { get; set; }

    public string? TableName { get; set; }

    public int? RawId { get; set; }

    public DateTime? UpdateTime { get; set; }

    public string? Description { get; set; }

    public virtual PrUser? User { get; set; }
}
