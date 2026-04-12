using System.Collections.Generic;
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
}