using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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
        private readonly IMapper mapper;

        public TodoListController(IHttpContextAccessor httpContextAccessor, ITodoListLogic todoListLogic, IMapper mapper)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.todoListLogic = todoListLogic;
            this.mapper = mapper;
        }

        /// <summary>
        /// Gets the todo lists for a user
        /// </summary>
        /// <param name="input">Paging parameters for the request</param>
        /// <returns>Action result containing PagedResult or ErrorResponse</returns>
        /// <response code="200">Gets the response model and returns Ok response</response>
        /// <response code="400">User was not found in the database</response>
        /// <response code="401">The user is not logged in</response>
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
        /// Get a complete todo list object by id. (Includes its items and labels)
        /// </summary>
        /// <param name="id">Id of the list</param>
        /// <returns>Action result containing todo List or ErrorResponse</returns>
        /// <response code="200">Gets the response model and returns Ok response</response>
        /// <response code="400">User or list was not found in the database</response>
        /// <response code="401">The user is not logged in</response>
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

        /// <summary>
        /// Creates a Todolist.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Action result containing todo List or ErrorResponse</returns>
        /// <response code="201">Creates todolist record and returns the location where created.</response>
        /// <response code="401">The user is not logged in</response>
        /// <response code="400">User was not found in the database</response>
        [ProducesResponseType(typeof(Response<TodoListDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]        
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost()]
        public IActionResult Create(CreateTodoListDto input)
        {
            var userId = int.Parse(httpContextAccessor.HttpContext.User.FindFirst(Constants.UserIdClaim)?.Value);
            var result = todoListLogic.CreateList(userId, input);
            if (result == null)
                return BadRequest(new ErrorResponse
                {
                    Status = false,
                    Message = "User not found in the database."
                });
            else
                return CreatedAtAction(nameof(GetSingle), new { result.Id }, new Response<TodoListDto>
                {
                    Status = true,
                    Model = result
                });
        }

        /// <summary>
        /// Update todo list
        /// </summary>
        /// <param name="updateObj">Update object</param>
        /// <returns>Action result containing todo List or ErrorResponse</returns>
        /// <response code="200">Update todo list and returns Ok result</response>
        /// <response code="401">User is not logged in.</response>
        /// <response code="400">Invalid data. No data exists for the given Id</response>
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response<TodoListDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [HttpPut]
        public IActionResult Update(UpdateTodoListDto updateObj)
        {
            int userId = int.Parse(httpContextAccessor.HttpContext.User.FindFirst(Constants.UserIdClaim)?.Value);
            var updateDto = mapper.Map<TodoListDto>(updateObj);

            var updatedResult = todoListLogic.UpdateList(userId, updateDto);

            if (updatedResult == null)
            {
                return BadRequest(new ErrorResponse
                {
                    Status = false,
                    Message = "List not found in the database."
                });
            }
            else
            {
                return Ok(new Response<TodoListDto>
                {
                    Status = true,
                    Model = updatedResult
                });
            }
        }

        /// <summary>
        /// Update todo list using JsonPatchDocument
        /// </summary>
        /// <param name="listId"></param>
        /// <param name="patchDocument">Patch data</param>
        /// <returns>Action result containing todo List or ErrorResponse</returns>
        /// <response code="200">Update todo list and returns Ok result</response>
        /// <response code="401">User is not logged in.</response>
        /// <response code="400">Invalid data. No data exists for the given Id</response>
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response<TodoListDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [HttpPatch]
        public IActionResult Patch([Required] int listId, [FromBody] JsonPatchDocument<UpdateTodoListDto> patchDocument)
        {
            int userId = int.Parse(httpContextAccessor.HttpContext.User.FindFirst(Constants.UserIdClaim)?.Value);

            var existingData = todoListLogic.GetList(userId, listId);
            if (existingData == null)
            {
                return BadRequest(new ErrorResponse
                {
                    Status = false,
                    Message = "List not found in the database."
                });
            }
            var patchDto = mapper.Map<JsonPatchDocument<TodoListDto>>(patchDocument);
            var existingDto = mapper.Map<TodoListDto>(existingData);

            patchDto.ApplyTo(existingDto);

            var updatedResult = todoListLogic.UpdateList(userId, existingDto);

            if (updatedResult == null)
            {
                return BadRequest(new ErrorResponse
                {
                    Status = false,
                    Message = "List not found in the database."
                });
            }
            else
            {
                return Ok(new Response<TodoListDto>
                {
                    Status = true,
                    Model = updatedResult
                });
            }
        }

        /// <summary>
        /// Delete todo list
        /// </summary>
        /// <param name="id">Id of the object to be deleted</param>
        /// <returns>Action result containing todo List or ErrorResponse</returns>
        /// <response code="200">Deletes todo list and returns Ok result</response>
        /// <response code="401">User is not logged in.</response>
        /// <response code="400">Invalid data. No data exists for the given Id</response>
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            int userId = int.Parse(httpContextAccessor.HttpContext.User.FindFirst(Constants.UserIdClaim)?.Value);

            var deleteResult = todoListLogic.DeleteList(userId, id);

            if (deleteResult)
            {
                return Ok(new Response<string>
                {
                    Status = true,
                    Model = $"Todo List with id {id} deleted."
                });               
            }
            else
            {
                return BadRequest(new ErrorResponse
                {
                    Status = false,
                    Message = "List not found in the database."
                });
            }
        }

        /// <summary>
        /// Gets the labels for todo lists for a user
        /// </summary>
        /// <param name="listId">Id of the list</param>
        /// <returns>Action result containing PagedResult or ErrorResponse</returns>
        /// <response code="200">Gets the response model and returns Ok response</response>
        /// <response code="400">User or list was not found in the database</response>
        /// <response code="401">The user is not logged in</response>
        [ProducesResponseType(typeof(Response<List<LabelDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("Label")]
        public IActionResult GetLabels([FromQuery] int listId)
        {
            var userId = int.Parse(httpContextAccessor.HttpContext.User.FindFirst(Constants.UserIdClaim)?.Value);
            var result = todoListLogic.GetListLabels(userId, listId);

            if (result == null)
                return BadRequest(new ErrorResponse
                {
                    Status = false,
                    Message = "User or list not found in the database."
                });
            else
                return Ok(new Response<List<LabelDto>>
                {
                    Status = true,
                    Model = result
                });
        }

        /// <summary>
        /// Assign a label to a Todolist.
        /// </summary>
        /// <param name="input">Create label dto</param>
        /// <returns>Action result containing todo List or ErrorResponse</returns>
        /// <response code="201">Creates todolist record and returns the location where created.</response>
        /// <response code="401">The user is not logged in</response>
        /// <response code="400">User or list was not found in the database</response>
        [ProducesResponseType(typeof(Response<LabelDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost("Label")]
        public IActionResult CreateLabel(CreateOrDeleteLabelDto input)
        {
            var userId = int.Parse(httpContextAccessor.HttpContext.User.FindFirst(Constants.UserIdClaim)?.Value);
            var result = todoListLogic.CreateLabel(userId, input);
            if (result == null)
                return BadRequest(new ErrorResponse
                {
                    Status = false,
                    Message = "User or list not found in the database."
                });
            else
                return CreatedAtAction(nameof(GetSingle), new { result.Id }, new Response<LabelDto>
                {
                    Status = true,
                    Model = result
                });
        }

        /// <summary>
        /// Update label for a todo list
        /// </summary>
        /// <param name="updateDto">Update object</param>
        /// <returns>Action result containing todo List or ErrorResponse</returns>
        /// <response code="200">Update label for todo list and returns Ok result</response>
        /// <response code="401">User is not logged in.</response>
        /// <response code="400">List or label does not exist</response>
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response<LabelDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [HttpPut("Label")]
        public IActionResult UpdateLabel(UpdateLabelDto updateDto)
        {
            int userId = int.Parse(httpContextAccessor.HttpContext.User.FindFirst(Constants.UserIdClaim)?.Value);

            var updatedResult = todoListLogic.UpdateLabel(userId, updateDto);

            if (updatedResult == null)
            {
                return BadRequest(new ErrorResponse
                {
                    Status = false,
                    Message = "List or label not found in the database."
                });
            }
            else
            {
                return Ok(new Response<LabelDto>
                {
                    Status = true,
                    Model = updatedResult
                });
            }
        }

        /// <summary>
        /// Delete label for a todo list
        /// </summary>
        /// <param name="deleteDto">deleteDto containing list id and label name</param>
        /// <returns>Action result containing todo List or ErrorResponse</returns>
        /// <response code="200">Delete label for todo list and returns Ok result</response>
        /// <response code="401">User is not logged in.</response>
        /// <response code="400">List or label does not exist</response>
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [HttpDelete("Label")]
        public IActionResult DeleteLabel(CreateOrDeleteLabelDto deleteDto)
        {
            int userId = int.Parse(httpContextAccessor.HttpContext.User.FindFirst(Constants.UserIdClaim)?.Value);

            var deleteResult = todoListLogic.DeleteLabel(userId, deleteDto);

            if (deleteResult)
            {
                return Ok(new Response<string>
                {
                    Status = true,
                    Model = $"Label {deleteDto.Label} deleted for list with id {deleteDto.ParentId}."
                });
            }
            else
            {
                return BadRequest(new ErrorResponse
                {
                    Status = false,
                    Message = "List or label not found in the database."
                });
            }
        }
    }
}
