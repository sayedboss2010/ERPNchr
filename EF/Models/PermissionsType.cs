using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class PermissionsType
{
    public int Id { get; set; }

    public string? NameAr { get; set; }

    public string? NameEn { get; set; }

    public virtual ICollection<HrEmployeePermission> HrEmployeePermissions { get; set; } = new List<HrEmployeePermission>();
}
