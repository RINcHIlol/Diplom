using System.Collections.Generic;

namespace diplom.DTOs.Profile;

public class ModuleProgressDto
{
    public int ModuleId { get; set; }
    public string Title { get; set; }
    public bool IsCompleted { get; set; }

    public List<LessonProgressDto> Lessons { get; set; }
}