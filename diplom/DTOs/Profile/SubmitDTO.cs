using System.Collections.Generic;

namespace diplom.DTOs.Profile;

public class SubmitDto
{
    public int TaskId { get; set; }
    public int UserId { get; set; }

    public List<int> AnswerIds { get; set; } = new();

    public string? TextAnswer { get; set; }
    
    public List<MatchDto>? Matches { get; set; }
}