using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class PrUserGroup
{
    public int Id { get; set; }

    public bool Active { get; set; }

    public int UserId { get; set; }

    public int? PrGroupModuleMenuId { get; set; }

    public virtual PrGroupModuleMenu? PrGroupModuleMenu { get; set; }

    public virtual PrUser User { get; set; } = null!;
}
