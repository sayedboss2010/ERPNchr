using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class PrGroup
{
    public int Id { get; set; }

    public string? GroupName { get; set; }

    public string? GroupNameEn { get; set; }

    public bool Active { get; set; }

    public DateOnly? CreatedDate { get; set; }

    public DateOnly? LastModifiedDate { get; set; }

    public string? Note { get; set; }

    public int? PrApplicationId { get; set; }

    public int? PrApplicationCategoryId { get; set; }

    public bool IsMinistry { get; set; }

    public int? OrderBy { get; set; }

    public virtual ICollection<PrGroupModuleMenu> PrGroupModuleMenus { get; set; } = new List<PrGroupModuleMenu>();
}
