using FastEndpoints;

namespace VaultLog.API.Endpoints;

public class HealthEndpoint: EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("/health");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken token)
    {
        await Send.OkAsync(new { status = "healthy" }, token);
    }
}