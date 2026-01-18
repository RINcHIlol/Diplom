using System;
using System.Collections.Generic;

namespace diplom.Models;

public partial class TaskType
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
