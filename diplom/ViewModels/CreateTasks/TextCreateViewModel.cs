using diplom.DTOs.Profile;

namespace diplom.ViewModels.CreateTasks;

public class TextCreateViewModel : TaskCreateViewModel
{
    public string Answer { get; set; } = "";

    public override CreateTaskDto BuildDto()
    {
        return new CreateTaskDto
        {
            TaskTypeId = 1,
            Question = Question,
        };
    }
    
    public string TextContent { get; set; }

    public override void LoadFromDto(TaskDto dto)
    {
        Question = dto.Question;

        TextContent = dto.Content ?? string.Empty;

        OnPropertyChanged(nameof(TextContent));
    }
}