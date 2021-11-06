using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartWallit.Core.Models;

namespace SmartWallit.Controllers
{
    [Route("api/[controller]")]
    [Consumes("application/json"), Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesErrorResponseType(typeof(ErrorDetails))]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
    }
}
