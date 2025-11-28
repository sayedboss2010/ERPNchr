using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class PrModule
{
    public int Id { get; set; }

    public string? ModuleName { get; set; }

    public string? ModuleNameEn { get; set; }

    public string? ModuleDescription { get; set; }

    public int? PrApplicationId { get; set; }

    public int? PrApplicationCategoryId { get; set; }

    public bool Active { get; set; }

    public virtual ICollection<PrGroupModuleMenu> PrGroupModuleMenus { get; set; } = new List<PrGroupModuleMenu>();
}
