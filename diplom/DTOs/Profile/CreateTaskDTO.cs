using System.Collections.Generic;

namespace diplom.DTOs.Profile;

public class CreateTaskDto
{
    public int TaskTypeId { get; set; }
    public string Question { get; set; }
    public string? Content { get; set; }
    public string? ExpectedOutput { get; set; }

    public int LessonId { get; set; }
    
    public List<CreateAnswerDto> Answers { get; set; } = new();
    public List<CreateMatchingPairDTO>? MatchingPairs { get; set; } 
}

public class CreateAnswerDto
{
    public string AnswerText { get; set; }
    public bool IsCorrect { get; set; }
    public int OrderIndex { get; set; }
}

public class CreateMatchingPairDTO
{
    public int LeftIndex { get; set; }
    public int RightIndex { get; set; }
}