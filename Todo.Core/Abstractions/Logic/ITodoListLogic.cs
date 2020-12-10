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
    }
}
