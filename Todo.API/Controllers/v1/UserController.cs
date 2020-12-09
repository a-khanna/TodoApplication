using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Todo.Core.Abstractions.Logic;
using Todo.Core.Models.Dtos;

namespace Todo.API.Controllers.v1
{
    /// <summary>
    /// Controller for User related operations
    /// </summary>
    [Route("api/v{version:apiVersion}/todo/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> logger;
        private readonly IUserLogic userLogic;

        public UserController(ILogger<UserController> logger, IUserLogic userLogic)
        {
            this.logger = logger;
            this.userLogic = userLogic;
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="user">User dto</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<IActionResult> Register(UserDto user)
        {
            var result = await userLogic.RegisterUser(user);

            if (result)
                return Ok();
            else
                return BadRequest();
        }
    }
}
