using System.Collections.Generic;
using Todo.Core.Models.Dtos;
using Todo.Core.Models.Request;
using Todo.Core.Models.Response;
using Todo.Core.Models.Sql;

namespace Todo.Core.Abstractions.Data
{
    /// <summary>
    /// Repository for handling todo items
    /// </summary>
    public interface ITodoItemRepository
    {
        /// <summary>
        /// Gets todo items by user id.
        /// Returns null if no user is found.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="pagingParams">Pagin params</param>
        /// <returns>Todo item entities</returns>
        PagedResult<TodoItem> GetItems(int userId, PagingParameters pagingParams);

        /// <summary>
        /// Gets todo items by list id.
        /// Returns null if item is not found.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="itemId">item id</param>
        /// <returns>Todo item entity</returns>
        TodoItem GetItem(int userId, int itemId);

        /// <summary>
        /// Creates a item.
        /// Returns null if user is not found.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="createDto">todo item to be created</param>
        /// <returns>Todo item</returns>
        TodoItem CreateItem(int userId, CreateTodoItemDto createDto);

        /// <summary>
        /// Updates a todo item.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="updateObj">todo item to be updated</param>
        /// <returns>Updated Todo item dto</returns>
        TodoItem UpdateItem(int userId, TodoItemDto updateDto);

        /// <summary>
        /// Deletes a todo item.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="itemId">Id of todo item to be deleted</param>
        /// <returns>True if delete was successful</returns>
        bool DeleteItem(int userId, int itemId);

        /// <summary>
        /// Gets labels for todo item by user id.
        /// Returns null if no user or item is found.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="itemId">Item Id</param>
        /// <returns>List of Labels Dto</returns>
        List<Label> GetItemLabels(int userId, int itemId);

        /// <summary>
        /// Creates a label for todo item.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="createLabelDto">label for todo item to be created</param>
        /// <returns>Label dto</returns>
        Label CreateLabel(int userId, CreateLabelDto createLabelDto);

        /// <summary>
        /// Updates label for a todo item.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="updateObj">label todo item to be updated</param>
        /// <returns>Updated label</returns>
        Label UpdateLabel(int userId, UpdateLabelDto updateObj);

        /// <summary>
        /// Deletes a label for todo item.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="deleteDto">Delete Dto</param>
        /// <returns>True if delete was successful</returns>
        bool DeleteLabel(int userId, DeleteLabelDto deleteDto);
    }
}
