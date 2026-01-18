using System;
using System.Collections.Generic;

namespace diplom.Models;

public partial class Lesson
{
    public int Id { get; set; }

    public int ModuleId { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public int OrderIndex { get; set; }

    public virtual Module Module { get; set; } = null!;

    public virtual ICollection<UserProgress> UserProgresses { get; set; } = new List<UserProgress>();
}
