using System;
using System.Collections.Generic;

namespace diplom.Models;

public partial class Module
{
    public int Id { get; set; }

    public int CourseId { get; set; }

    public string Title { get; set; } = null!;

    public int OrderIndex { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
