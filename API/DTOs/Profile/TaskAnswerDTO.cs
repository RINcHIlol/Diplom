namespace API.DTOs.Profile;

public class TaskAnswerDto
{
    public int Id { get; set; }
    public string AnswerText { get; set; }

    public int OrderIndex { get; set; }
    public bool IsCorrect { get; set; }

    public List<MatchingPairDTO>? MatchingPairs { get; set; }
}