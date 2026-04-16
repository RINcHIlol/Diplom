using System.Collections.Generic;

namespace diplom.DTOs.Profile;

public class TaskDto
{
    public int Id { get; set; }
    public int TaskTypeId { get; set; }
    public string Question { get; set; }
    
    public string? Content { get; set; }
    
    public int OrderIndex { get; set; }
    public string? ExpectedOutput {get; set;}
    public List<TaskAnswerDto>? Answers { get; set; }
    public List<MatchDto>? MatchingPairs { get; set; }
}