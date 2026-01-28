
using System.IO;
using diplom.ModelsApi;

namespace diplom.Services;

public class SessionService
{
    private const string TokenFile = "session.token";

    public User? CurrentUser { get; private set; }
    public string? Token { get; private set; }

    public bool IsAuthorized => CurrentUser != null;

    public void Login(User user, string token)
    {
        CurrentUser = user;
        Token = token;

        File.WriteAllText(TokenFile, token);
    }

    public void Logout()
    {
        CurrentUser = null;
        Token = null;

        if (File.Exists(TokenFile))
            File.Delete(TokenFile);
    }

    public void LoadToken()
    {
        if (File.Exists(TokenFile))
        {
            Token = File.ReadAllText(TokenFile);
        }
    }
}