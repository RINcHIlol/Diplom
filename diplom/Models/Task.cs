using System;
using System.Collections.Generic;

namespace diplom.Models;

public partial class Task
{
    public int Id { get; set; }

    public int ModuleId { get; set; }

    public int TaskTypeId { get; set; }

    public string Question { get; set; } = null!;

    public string CorrectAnswer { get; set; } = null!;

    public int OrderIndex { get; set; }

    public string? Content { get; set; }

    public virtual Module Module { get; set; } = null!;

    public virtual ICollection<TaskAnswer> TaskAnswers { get; set; } = new List<TaskAnswer>();

    public virtual TaskType TaskType { get; set; } = null!;
}
