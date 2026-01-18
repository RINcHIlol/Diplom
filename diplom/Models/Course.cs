using System;
using System.Collections.Generic;

namespace diplom.Models;

public partial class Course
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public byte[]? Image { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Module> Modules { get; set; } = new List<Module>();
}
