using System.Linq;
using Microsoft.EntityFrameworkCore;
using Todo.Core.Abstractions.Data;
using Todo.Core.Models.Request;
using Todo.Core.Models.Response;
using Todo.Core.Models.Sql;

namespace Todo.Data.Repositories
{
    /// <summary>
    /// Repository for handling todo lists
    /// </summary>
    public class TodoListRepository : ITodoListRepository
    {
        private readonly TodoDbContext dbContext;

        public TodoListRepository(TodoDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Gets todo lists by user id.
        /// Returns null if no user is found.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="pagingParams">Pagin params</param>
        /// <returns>Todo list entities</returns>
        public PagedResult<TodoList> GetLists(int userId, PagingParameters pagingParams)
        {
            if (!dbContext.Users.Any(u => u.Id == userId))
                return null;

            var dbLists = dbContext.TodoLists
                .Include(d => d.Labels).AsNoTracking()
                .Where(t => t.User.Id == userId);

            if (!string.IsNullOrWhiteSpace(pagingParams.Search))
                dbLists = dbLists.Where(d => d.Name.ToLower().Contains(pagingParams.Search.ToLower()));

            var count = dbLists.Count();

            var result = dbLists
                .Skip(pagingParams.Skip)
                .Take(pagingParams.Take)
                .ToList();

            return new PagedResult<TodoList>
            {
                PageContent = result,
                StartIndex = pagingParams.Skip,
                Total = count
            };
        }

        /// <summary>
        /// Gets todo lists by list id.
        /// Returns null if list is not found.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="listId">List id</param>
        /// <returns>Todo list entity</returns>
        public TodoList GetList(int userId, int listId)
        {
            return dbContext.TodoLists
                .AsNoTracking()
                .Include(t => t.Items)
                .Include(t => t.Labels)
                .FirstOrDefault(t => t.User.Id == userId && t.Id == listId);
        }      
    }
}
