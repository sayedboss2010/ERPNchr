using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class PrUser
{
    public int Id { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int? UserTypeId { get; set; }

    public int? SectorId { get; set; }

    public string? FullName { get; set; }

    public long? EmployeesId { get; set; }

    public virtual ICollection<DataUpdatesLogTb> DataUpdatesLogTbs { get; set; } = new List<DataUpdatesLogTb>();

    public virtual HrEmployee? Employees { get; set; }

    public virtual ICollection<ExceptionLogTb> ExceptionLogTbs { get; set; } = new List<ExceptionLogTb>();

    public virtual ICollection<LogInHistoryTb> LogInHistoryTbs { get; set; } = new List<LogInHistoryTb>();

    public virtual ICollection<PrUserGroup> PrUserGroups { get; set; } = new List<PrUserGroup>();
}
