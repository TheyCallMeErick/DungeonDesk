using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DungeonDeskBackend.Api.Controllers;

[Route("api/session")]
[ApiController]
[Authorize]
public class SessionController
{
    public SessionController()
    {
        // constructor logic here
    }

    // class members here
}
