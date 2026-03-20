namespace diplom.Services;

public class MessageService
{
    // public MessageService()
    // {
    // }
    private string? _message;

    public string? GetMessage()
    {
        var msg = _message;
        _message = null;
        return msg;
    }

    public void SetMessage(string message)
    {
        _message = message;
    }
}
