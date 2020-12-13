using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Todo.Core.Abstractions.Logic;
using Todo.Core.Models.Dtos;
using Todo.Core.Models.Response;

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
        private readonly IUserLogic userLogic;

        public UserController(IUserLogic userLogic)
        {
            this.userLogic = userLogic;
        }

        /// <summary>
        /// Verify user credentails. Return a JWT to be used for subsequent requests.
        /// </summary>
        /// <param name="credentials">Username and password</param>
        /// <returns></returns>
        /// <response code="200">Gets the response model and returns Ok response</response>
        /// <response code="400">username or password is incorrect</response>
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [HttpPost("authenticate")]
        public IActionResult Authenticate(CredentialsDto credentials)
        {
            var result = userLogic.VerifyLogin(credentials);

            if (string.IsNullOrEmpty(result))
                return BadRequest(new ErrorResponse
                {
                    Status = false,
                    Message = "username or password is incorrect"
                });
            else
                return Ok(new Response<string>
                {
                    Status = true,
                    Model = result,
                    Message = "Successfully authenticated."
                });
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="user">User dto</param>
        /// <returns></returns>
        /// <response code="200">Gets the response model and returns Ok response</response>
        /// <response code="400">There was a problem with registering user</response>
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDto user)
        {
            var result = await userLogic.RegisterUser(user);

            if (result)
                return Ok(new Response<string>
                {
                    Status = true,
                    Message = "Successfully registered user."
                });           
            else
                return BadRequest(new ErrorResponse
                {
                    Status = false,
                    Message = "User already exists."
                });
        }
    }
}
