using DungeonDeskBackend.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DungeonDeskBackend.Api.Controllers;

[Route("api/session")]
[ApiController]
[Authorize]
public class SessionController
{
    private readonly ISessionService _sessionService;
    public SessionController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }
}
