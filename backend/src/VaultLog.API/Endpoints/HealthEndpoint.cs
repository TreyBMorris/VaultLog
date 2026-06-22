using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace VaultLog.API.Endpoints;

[Authorize]
public class HealthEndpoint: EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("/health");
    }

    public override async Task HandleAsync(CancellationToken token)
    {
        await Send.OkAsync(new { status = "healthy" }, token);
    }
}