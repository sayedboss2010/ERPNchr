using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class HrEmployeePermission
{
    public long Id { get; set; }

    public long? EmployeeId { get; set; }

    public DateOnly? DateOfPermission { get; set; }

    public bool IsActive { get; set; }

    public int? CreatedUserId { get; set; }

    public DateOnly? CreatedDate { get; set; }

    public int? UpdatedUserId { get; set; }

    public DateOnly? UpdatedDate { get; set; }

    public int? DeletedUserId { get; set; }

    public DateOnly? DeletedDate { get; set; }

    public int? PermissionTypeId { get; set; }

    /// <summary>
    /// موافقة المدير المباشر
    /// </summary>
    public bool? DirectManagerApproval { get; set; }

    /// <summary>
    /// موافقة مدير الادارة
    /// </summary>
    public bool? DepartmentManagerApproval { get; set; }

    public virtual HrEmployee? Employee { get; set; }

    public virtual PermissionsType? PermissionType { get; set; }
}
