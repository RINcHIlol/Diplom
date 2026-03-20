using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace diplom.Services.Handlers;


public class AuthHandler : DelegatingHandler
{
    private readonly SessionService _session;

    public AuthHandler(SessionService session)
    {
        _session = session;
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(_session.Token))
        {
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", _session.Token);
        }

        return base.SendAsync(request, cancellationToken);
    }
}