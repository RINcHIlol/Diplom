using System;
using System.Collections.Generic;

namespace diplom.Models;

public partial class Lvl
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int Xp { get; set; }
}
