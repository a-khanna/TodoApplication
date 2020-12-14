using System.Collections.Generic;
using Todo.Core.Models.Dtos;
using Todo.Core.Models.Request;
using Todo.Core.Models.Response;
using Todo.Core.Models.Sql;

namespace Todo.Core.Abstractions.Data
{
    /// <summary>
    /// Repository for handling todo lists
    /// </summary>
    public interface ITodoListRepository
    {
        /// <summary>
        /// Gets todo lists by user id.
        /// Returns null if no user is found.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="pagingParams">Pagin params</param>
        /// <returns>Todo list entities</returns>
        PagedResult<TodoList> GetLists(int userId, PagingParameters pagingParams);

        /// <summary>
        /// Gets todo lists by list id.
        /// Returns null if list is not found.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="listId">List id</param>
        /// <returns>Todo list entity</returns>
        TodoList GetList(int userId, int listId);

        /// <summary>
        /// Creates a list.
        /// Returns null if user is not found.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="createDto">todo List to be created</param>
        /// <returns>Todo list</returns>
        TodoList CreateList(int userId, CreateTodoListDto createDto);

        /// <summary>
        /// Updates a todo list.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="updateObj">todo List to be updated</param>
        /// <returns>Updated Todo list dto</returns>
        TodoList UpdateList(int userId, TodoListDto updateDto);

        /// <summary>
        /// Deletes a todo list.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="listId">Id of todo List to be deleted</param>
        /// <returns>True if delete was successful</returns>
        bool DeleteList(int userId, int listId);

        /// <summary>
        /// Gets labels for todo lists by user id.
        /// Returns null if no user or list is found.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="listId">List Id</param>
        /// <returns>List of Labels Dto</returns>
        List<Label> GetListLabels(int userId, int listId);

        /// <summary>
        /// Creates a label for todo list.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="createLabelDto">label for todo List to be created</param>
        /// <returns>Label dto</returns>
        Label CreateLabel(int userId, CreateLabelDto createLabelDto);

        /// <summary>
        /// Updates label for a todo list.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="updateObj">todo List to be updated</param>
        /// <returns>Updated Todo list dto</returns>
        Label UpdateLabel(int userId, UpdateLabelDto updateObj);

        /// <summary>
        /// Deletes a label for todo list.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="deleteDto">Delete Dto</param>
        /// <returns>True if delete was successful</returns>
        bool DeleteLabel(int userId, DeleteLabelDto deleteDto);
    }
}
