using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// تاخيرات الموظفين
/// </summary>
public partial class HrEmployeeLateness
{
    public long Id { get; set; }

    public long? HrEmployeesId { get; set; }

    public int? HrLatenessTypeId { get; set; }

    public int? Day { get; set; }

    public int? Month { get; set; }

    public int? Year { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public long? UserCreationId { get; set; }

    public DateTime? UserCreationDate { get; set; }

    public long? UserUpdationId { get; set; }

    public DateTime? UserUpdationDate { get; set; }

    public long? UserDeletionId { get; set; }

    public DateTime? UserDeletionDate { get; set; }

    public virtual HrEmployee? HrEmployees { get; set; }
}
