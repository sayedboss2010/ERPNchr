using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class AUserLogin
{
    public long Id { get; set; }

    public short UserId { get; set; }

    public DateTime LoginDate { get; set; }

    public DateTime? LogOutDate { get; set; }

    public string AccessToken { get; set; } = null!;
}
