using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class PrSystem
{
    public int Id { get; set; }

    public string? SystemNameAr { get; set; }

    public string? SystemNameEn { get; set; }

    public bool Active { get; set; }

    public string? ImgePath { get; set; }
}
