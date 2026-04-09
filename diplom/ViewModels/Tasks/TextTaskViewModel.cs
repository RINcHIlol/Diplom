namespace diplom.ViewModels.Tasks;

public class TextTaskViewModel : TaskViewModel
{
    //убрать
    public string? UserAnswer { get; set; }

    public override void Submit()
    {
        // 👉 отправка UserAnswer
    }
}