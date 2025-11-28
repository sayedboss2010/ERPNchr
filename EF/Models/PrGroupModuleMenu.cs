using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class PrGroupModuleMenu
{
    public int Id { get; set; }

    public int PrGroupId { get; set; }

    public int PrModuleId { get; set; }

    public int PrMenuId { get; set; }

    public bool? IsActive { get; set; }

    public int? OrderBy { get; set; }

    public virtual PrGroup PrGroup { get; set; } = null!;

    public virtual PrMenu PrMenu { get; set; } = null!;

    public virtual PrModule PrModule { get; set; } = null!;

    public virtual ICollection<PrUserGroup> PrUserGroups { get; set; } = new List<PrUserGroup>();
}
