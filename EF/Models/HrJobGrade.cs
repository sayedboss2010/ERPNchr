using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class HrJobGrade
{
    public int Id { get; set; }

    public string NameAr { get; set; } = null!;

    public string? NameEn { get; set; }

    public bool IsActive { get; set; }

    public int? CreatedUserId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? UpdatedUserId { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? DeletedUserId { get; set; }

    public DateTime? DeletedDate { get; set; }
}
