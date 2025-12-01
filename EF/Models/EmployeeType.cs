using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class EmployeeType
{
    public int Id { get; set; }

    public string? EmployeeTypeNameAr { get; set; }

    public string? EmployeeTypeNameEn { get; set; }

    public virtual ICollection<HrEmployee> HrEmployees { get; set; } = new List<HrEmployee>();
}
