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
    }
}
