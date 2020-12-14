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
    /// Controller for Todo items related operations
    /// </summary>
    [Route("api/v{version:apiVersion}/todo/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize]
    public class TodoItemController : ControllerBase
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ITodoItemLogic todoItemLogic;
        private readonly IMapper mapper;

        public TodoItemController(IHttpContextAccessor httpContextAccessor, ITodoItemLogic todoItemLogic, IMapper mapper)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.todoItemLogic = todoItemLogic;
            this.mapper = mapper;
        }

        /// <summary>
        /// Gets all todo items for a user matching the filter criteria.
        /// </summary>
        /// <param name="input">Paging parameters for the request</param>
        /// <returns>Action result containing PagedResult or ErrorResponse</returns>
        /// <response code="200">Gets the response model and returns Ok response</response>
        /// <response code="400">User was not found in the database</response>
        /// <response code="401">The user is not logged in</response>
        [ProducesResponseType(typeof(Response<PagedResult<TodoItemDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("Items")]
        public IActionResult Get([FromQuery] PagingParameters input)
        {
            var userId = int.Parse(httpContextAccessor.HttpContext.User.FindFirst(Constants.UserIdClaim)?.Value);
            var result = todoItemLogic.GetItems(userId, input);

            if (result == null)
                return BadRequest(new ErrorResponse
                {
                    Status = false,
                    Message = "User not found in the database."
                });
            else
                return Ok(new Response<PagedResult<TodoItemDto>>
                {
                    Status = true,
                    Model = result
                });
        }

        /// <summary>
        /// Get a todo item by id
        /// </summary>
        /// <param name="id">Id of the item</param>
        /// <returns>Action result containing todo item or ErrorResponse</returns>
        /// <response code="200">Gets the response model and returns Ok response</response>
        /// <response code="400">User or item was not found in the database</response>
        /// <response code="401">The user is not logged in</response>
        [ProducesResponseType(typeof(Response<TodoItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("{id}")]
        public IActionResult GetItemById(int id)
        {
            var userId = int.Parse(httpContextAccessor.HttpContext.User.FindFirst(Constants.UserIdClaim)?.Value);
            var result = todoItemLogic.GetItem(userId, id);

            if (result == null)
                return BadRequest(new ErrorResponse
                {
                    Status = false,
                    Message = "User or item not found in the database."
                });
            else
                return Ok(new Response<TodoItemDto>
                {
                    Status = true,
                    Model = result
                });
        }

        /// <summary>
        /// Creates a TodoItem.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Action result containing todo item or ErrorResponse</returns>
        /// <response code="201">Creates todo item record and returns the location where created.</response>
        /// <response code="401">The user is not logged in</response>
        /// <response code="400">User or list was not found in the database</response>
        [ProducesResponseType(typeof(Response<TodoItemDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost]
        public IActionResult Create(CreateTodoItemDto input)
        {
            var userId = int.Parse(httpContextAccessor.HttpContext.User.FindFirst(Constants.UserIdClaim)?.Value);
            var result = todoItemLogic.CreateItem(userId, input);
            if (result == null)
                return BadRequest(new ErrorResponse
                {
                    Status = false,
                    Message = "User or list not found in the database."
                });
            else
                return CreatedAtAction(nameof(GetItemById), new { result.Id }, new Response<TodoItemDto>
                {
                    Status = true,
                    Model = result
                });
        }

        /// <summary>
        /// Update todo item
        /// </summary>
        /// <param name="updateObj">Update object</param>
        /// <returns>Action result containing todo item or ErrorResponse</returns>
        /// <response code="200">Update todo item and returns Ok result</response>
        /// <response code="401">User is not logged in.</response>
        /// <response code="400">Invalid data. No data exists for the given Id</response>
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response<TodoItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [HttpPut]
        public IActionResult Update(UpdateTodoItemDto updateObj)
        {
            int userId = int.Parse(httpContextAccessor.HttpContext.User.FindFirst(Constants.UserIdClaim)?.Value);
            var updateDto = mapper.Map<TodoItemDto>(updateObj);

            var updatedResult = todoItemLogic.UpdateItem(userId, updateDto);

            if (updatedResult == null)
            {
                return BadRequest(new ErrorResponse
                {
                    Status = false,
                    Message = "Item not found in the database."
                });
            }
            else
            {
                return Ok(new Response<TodoItemDto>
                {
                    Status = true,
                    Model = updatedResult
                });
            }
        }

        /// <summary>
        /// Update todo item using JsonPatchDocument
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="patchDocument">Patch data</param>
        /// <returns>Action result containing todo item or ErrorResponse</returns>
        /// <response code="200">Update todo item and returns Ok result</response>
        /// <response code="401">User is not logged in.</response>
        /// <response code="400">Invalid data. No data exists for the given Id</response>
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response<TodoItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [HttpPatch]
        public IActionResult Patch([Required] int itemId, [FromBody] JsonPatchDocument<UpdateTodoItemDto> patchDocument)
        {
            int userId = int.Parse(httpContextAccessor.HttpContext.User.FindFirst(Constants.UserIdClaim)?.Value);

            var existingData = todoItemLogic.GetItem(userId, itemId);
            if (existingData == null)
            {
                return BadRequest(new ErrorResponse
                {
                    Status = false,
                    Message = "Item not found in the database."
                });
            }
            var patchDto = mapper.Map<JsonPatchDocument<TodoItemDto>>(patchDocument);
            var existingDto = mapper.Map<TodoItemDto>(existingData);

            patchDto.ApplyTo(existingDto);

            var updatedResult = todoItemLogic.UpdateItem(userId, existingDto);

            if (updatedResult == null)
            {
                return BadRequest(new ErrorResponse
                {
                    Status = false,
                    Message = "Item not found in the database."
                });
            }
            else
            {
                return Ok(new Response<TodoItemDto>
                {
                    Status = true,
                    Model = updatedResult
                });
            }
        }

        /// <summary>
        /// Delete todo item
        /// </summary>
        /// <param name="id">Id of the object to be deleted</param>
        /// <returns>Action result containing todo item or ErrorResponse</returns>
        /// <response code="200">Deletes todo item and returns Ok result</response>
        /// <response code="401">User is not logged in.</response>
        /// <response code="400">Invalid data. No data exists for the given Id</response>
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            int userId = int.Parse(httpContextAccessor.HttpContext.User.FindFirst(Constants.UserIdClaim)?.Value);

            var deleteResult = todoItemLogic.DeleteItem(userId, id);

            if (deleteResult)
            {
                return Ok(new Response<string>
                {
                    Status = true,
                    Model = $"Todo item with id {id} deleted."
                });
            }
            else
            {
                return BadRequest(new ErrorResponse
                {
                    Status = false,
                    Message = "Item not found in the database."
                });
            }
        }

        /// <summary>
        /// Gets the labels for todo item
        /// </summary>
        /// <param name="itemId">Id of the item</param>
        /// <returns>Action result containing PagedResult or ErrorResponse</returns>
        /// <response code="200">Gets the response model and returns Ok response</response>
        /// <response code="400">User or item was not found in the database</response>
        /// <response code="401">The user is not logged in</response>
        [ProducesResponseType(typeof(Response<List<LabelDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("Label")]
        public IActionResult GetLabels([FromQuery] int itemId)
        {
            var userId = int.Parse(httpContextAccessor.HttpContext.User.FindFirst(Constants.UserIdClaim)?.Value);
            var result = todoItemLogic.GetItemLabels(userId, itemId);

            if (result == null)
                return BadRequest(new ErrorResponse
                {
                    Status = false,
                    Message = "User or item not found in the database."
                });
            else
                return Ok(new Response<List<LabelDto>>
                {
                    Status = true,
                    Model = result
                });
        }

        /// <summary>
        /// Assign a label to a TodoItem.
        /// </summary>
        /// <param name="input">Create label dto</param>
        /// <returns>Action result containing todo item or ErrorResponse</returns>
        /// <response code="201">Creates label and returns the location where created.</response>
        /// <response code="401">The user is not logged in</response>
        /// <response code="400">User or item was not found in the database</response>
        [ProducesResponseType(typeof(Response<LabelDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost("Label")]
        public IActionResult AssignLabel(CreateLabelDto input)
        {
            var userId = int.Parse(httpContextAccessor.HttpContext.User.FindFirst(Constants.UserIdClaim)?.Value);
            var result = todoItemLogic.CreateLabel(userId, input);
            if (result == null)
                return BadRequest(new ErrorResponse
                {
                    Status = false,
                    Message = "User or item not found in the database."
                });
            else
                return CreatedAtAction(nameof(GetItemById), new { result.Id }, new Response<LabelDto>
                {
                    Status = true,
                    Model = result
                });
        }

        /// <summary>
        /// Update label for a todo item
        /// </summary>
        /// <param name="updateDto">Update object</param>
        /// <returns>Action result containing todo item or ErrorResponse</returns>
        /// <response code="200">Update label for todo item and returns Ok result</response>
        /// <response code="401">User is not logged in.</response>
        /// <response code="400">Item or label does not exist</response>
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response<LabelDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [HttpPut("Label")]
        public IActionResult UpdateLabel(UpdateLabelDto updateDto)
        {
            int userId = int.Parse(httpContextAccessor.HttpContext.User.FindFirst(Constants.UserIdClaim)?.Value);

            var updatedResult = todoItemLogic.UpdateLabel(userId, updateDto);

            if (updatedResult == null)
            {
                return BadRequest(new ErrorResponse
                {
                    Status = false,
                    Message = "Item or label not found in the database."
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
        /// Delete label for a todo item
        /// </summary>
        /// <param name="deleteDto">deleteDto containing item id and label name</param>
        /// <returns>Action result containing todo item or ErrorResponse</returns>
        /// <response code="200">Delete label for todo item and returns Ok result</response>
        /// <response code="401">User is not logged in.</response>
        /// <response code="400">item or label does not exist</response>
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [HttpDelete("Label")]
        public IActionResult DeleteLabel(DeleteLabelDto deleteDto)
        {
            int userId = int.Parse(httpContextAccessor.HttpContext.User.FindFirst(Constants.UserIdClaim)?.Value);

            var deleteResult = todoItemLogic.DeleteLabel(userId, deleteDto);

            if (deleteResult)
            {
                return Ok(new Response<string>
                {
                    Status = true,
                    Model = $"Label {deleteDto.Label} deleted for item with id {deleteDto.ParentId}."
                });
            }
            else
            {
                return BadRequest(new ErrorResponse
                {
                    Status = false,
                    Message = "Item or label not found in the database."
                });
            }
        }
    }
}
