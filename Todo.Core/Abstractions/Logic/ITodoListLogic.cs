using System.Collections.Generic;
using Todo.Core.Models.Dtos;
using Todo.Core.Models.Request;
using Todo.Core.Models.Response;

namespace Todo.Core.Abstractions.Logic
{
    /// <summary>
    /// Logic layer for Todo lists
    /// </summary>
    public interface ITodoListLogic
    {
        /// <summary>
        /// Gets todo lists by user id.
        /// Returns empty list if no user is found.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="pagingParams">Pagin params</param>
        /// <returns>Todo lists Dto</returns>
        PagedResult<TodoListDto> GetLists(int userId, PagingParameters pagingParam);

        /// <summary>
        /// Gets todo list by list id.
        /// Returns null if no list is found.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="listId">List id</param>
        /// <returns>Todo list Dto</returns>
        TodoListDetailedDto GetList(int userId, int listId);

        /// <summary>
        /// Creates a todo list.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="createDto">todo List to be created</param>
        /// <returns>Todo list dto</returns>
        TodoListDto CreateList(int userId, CreateTodoListDto createDto);

        /// <summary>
        /// Updates a todo list.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="updateObj">todo List to be updated</param>
        /// <returns>Updated Todo list dto</returns>
        TodoListDto UpdateList(int userId, TodoListDto updateObj);

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
        List<LabelDto> GetListLabels(int userId, int listId);

        /// <summary>
        /// Creates a label for todo list.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="createLabelDto">label for todo List to be created</param>
        /// <returns>Label dto</returns>
        LabelDto CreateLabel(int userId, CreateOrDeleteLabelDto createLabelDto);

        /// <summary>
        /// Updates label for a todo list.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="updateObj">todo List to be updated</param>
        /// <returns>Updated Todo list dto</returns>
        LabelDto UpdateLabel(int userId, UpdateLabelDto updateObj);

        /// <summary>
        /// Deletes a label for todo list.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="deleteDto">Delete Dto</param>
        /// <returns>True if delete was successful</returns>
        bool DeleteLabel(int userId, CreateOrDeleteLabelDto deleteDto);
    }
}
