using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class PrMenu
{
    public int Id { get; set; }

    public string? MenuTitle { get; set; }

    public string? MenuTitleEn { get; set; }

    public string? MenuUrl { get; set; }

    public int? PrMenuId { get; set; }

    public bool Active { get; set; }

    public int? PrModuleId { get; set; }

    public int? PrApplicationId { get; set; }

    public int? PrApplicationCategoryId { get; set; }

    public int? GroupId { get; set; }

    public virtual ICollection<PrGroupModuleMenu> PrGroupModuleMenus { get; set; } = new List<PrGroupModuleMenu>();
}
