using System;
using System.Collections.Generic;

namespace diplom.Models;

public partial class UserProgress
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int LessonId { get; set; }

    public bool? IsCompleted { get; set; }

    public DateTime CompletedAt { get; set; }

    public virtual Lesson Lesson { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
