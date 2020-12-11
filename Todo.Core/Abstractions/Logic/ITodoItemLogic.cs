using System.Collections.Generic;
using Todo.Core.Models.Dtos;
using Todo.Core.Models.Request;
using Todo.Core.Models.Response;

namespace Todo.Core.Abstractions.Logic
{
    /// <summary>
    /// Logic layer for Todo items
    /// </summary>
    public interface ITodoItemLogic
    {
        /// <summary>
        /// Gets todo items by user id.
        /// Returns null if no user is found.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="pagingParams">Pagin params</param>
        /// <returns>Todo items Dto</returns>
        PagedResult<TodoItemDto> GetItems(int userId, PagingParameters pagingParam);

        /// <summary>
        /// Gets todo item by list id.
        /// Returns null if no list is found.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="itemId">item id</param>
        /// <returns>Todo item Dto</returns>
        public TodoItemDto GetItem(int userId, int itemId);

        /// <summary>
        /// Creates a todo item.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="createDto">todo item to be created</param>
        /// <returns>Todo item dto</returns>
        TodoItemDto CreateItem(int userId, CreateTodoItemDto createDto);

        /// <summary>
        /// Updates a todo item.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="updateObj">todo item to be updated</param>
        /// <returns>Updated Todo item dto</returns>
        TodoItemDto UpdateItem(int userId, TodoItemDto updateObj);

        /// <summary>
        /// Deletes a todo item.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="itemId">Id of todo item to be deleted</param>
        /// <returns>True if delete was successful</returns>
        bool DeleteItem(int userId, int itemId);

        /// <summary>
        /// Gets labels for todo item
        /// Returns null if no user or item is found.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="itemId">List Id</param>
        /// <returns>List of Labels Dto</returns>
        List<LabelDto> GetItemLabels(int userId, int itemId);

        /// <summary>
        /// Creates a label for todo item.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="createLabelDto">label for todo item to be created</param>
        /// <returns>Label dto</returns>
        LabelDto CreateLabel(int userId, CreateOrDeleteLabelDto createLabelDto);

        /// <summary>
        /// Updates label for a todo item.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="updateObj">todo item to be updated</param>
        /// <returns>Updated label dto</returns>
        LabelDto UpdateLabel(int userId, UpdateLabelDto updateObj);

        /// <summary>
        /// Deletes a label for todo item.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="deleteDto">Delete Dto</param>
        /// <returns>True if delete was successful</returns>
        bool DeleteLabel(int userId, CreateOrDeleteLabelDto deleteDto);
    }
}
