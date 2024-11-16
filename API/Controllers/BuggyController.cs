using API.Errors;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuggyController : ControllerBase
    {
        private readonly StoreContext context;

        public BuggyController(StoreContext _context)
        {
            context = _context;
        }

        [HttpGet("notfound")]
        public IActionResult GetNotFoundRequest()
        {
            string thing = null;

            if (thing == null) return NotFound(new ApiResponse(404));
            return Ok();
        }

        [HttpGet("servererror")]
        public IActionResult GetServerError()
        {

            string thing = null;

            var thingToReturn = thing.ToString();

            return Ok();
        }

        [HttpGet("badrequest")]
        public IActionResult GetBadRequest()
        {
            return BadRequest(new ApiResponse(400));
        }

        [HttpGet("badrequest/{id}")]
        public IActionResult GetNotFoundRequest(int id)
        {
            return Ok();
        }
    }
}
