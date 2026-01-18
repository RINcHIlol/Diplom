using System;
using System.Collections.Generic;

namespace diplom.Models;

public partial class TaskAnswer
{
    public int Id { get; set; }

    public int TaskId { get; set; }

    public string AnswerText { get; set; } = null!;

    public bool? IsCorrect { get; set; }

    public int? MatchKey { get; set; }

    public int OrderIndex { get; set; }

    public virtual Task Task { get; set; } = null!;
}
