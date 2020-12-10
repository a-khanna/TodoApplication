using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Todo.Core;
using Todo.Core.Abstractions.Logic;
using Todo.Core.Models.Dtos;
using Todo.Core.Models.Request;
using Todo.Core.Models.Response;

namespace Todo.API.Controllers.v1
{
    /// <summary>
    /// Controller for Todo list related operations
    /// </summary>
    [Route("api/v{version:apiVersion}/todo/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize]
    public class TodoListController : ControllerBase
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ITodoListLogic todoListLogic;

        public TodoListController(IHttpContextAccessor httpContextAccessor, ITodoListLogic todoListLogic)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.todoListLogic = todoListLogic;
        }

        /// <summary>
        /// Gets the todo lists for a user
        /// </summary>
        /// <param name="input">Paging parameters for the request</param>
        /// <returns>Action result containing PagedResult or ErrorResponse</returns>
        [ProducesResponseType(typeof(Response<PagedResult<TodoListDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet]
        public IActionResult Get([FromQuery] PagingParameters input)
        {
            var userId = int.Parse(httpContextAccessor.HttpContext.User.FindFirst(Constants.UserIdClaim)?.Value);
            var result = todoListLogic.GetLists(userId, input);

            if (result == null)
                return BadRequest(new ErrorResponse
                {
                    Status = false,
                    Message = "User not found in the database."
                });
            else
                return Ok(new Response<PagedResult<TodoListDto>>
                {
                    Status = true,
                    Model = result
                });
        }

        /// <summary>
        /// Get a todo list by id
        /// </summary>
        /// <param name="id">Id of the list</param>
        /// <returns>Action result containing todo List or ErrorResponse</returns>
        [ProducesResponseType(typeof(Response<TodoListDetailedDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("{id}")]
        public IActionResult GetSingle(int id)
        {
            var userId = int.Parse(httpContextAccessor.HttpContext.User.FindFirst(Constants.UserIdClaim)?.Value);
            var result = todoListLogic.GetList(userId, id);

            if (result == null)
                return BadRequest(new ErrorResponse
                {
                    Status = false,
                    Message = "User or list not found in the database."
                });
            else
                return Ok(new Response<TodoListDetailedDto>
                {
                    Status = true,
                    Model = result
                });
        }
    }
}
