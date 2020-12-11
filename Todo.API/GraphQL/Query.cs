using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Todo.Core.Abstractions.Data;
using Todo.Core.Models.Request;
using Todo.Core.Models.Response;
using Todo.Core.Models.Sql;

namespace Todo.API.GraphQL
{
    [Authorize]
    public class Query : Base
    {
        public Query(IHttpContextAccessor httpContextAccessor, ITodoListRepository todoListRepository, ITodoItemRepository todoItemRepository)
            : base(httpContextAccessor, todoListRepository, todoItemRepository)
        {
        }

        public TodoList GetList(int id)
        {
            var userId = CheckAuthentication();
            return todoListRepository.GetList(userId, id);
        }

        public PagedResult<TodoList> GetLists(PagingParameters pagingParameters)
        {
            var userId = CheckAuthentication();
            return todoListRepository.GetLists(userId, pagingParameters);
        }     
        
        public List<Label> GetListLabels(int listId)
        {
            var userId = CheckAuthentication();
            return todoListRepository.GetListLabels(userId, listId);
        }

        public TodoItem GetItem(int id)
        {
            var userId = CheckAuthentication();
            return todoItemRepository.GetItem(userId, id);
        }

        public PagedResult<TodoItem> GetItems(PagingParameters pagingParameters)
        {
            var userId = CheckAuthentication();
            return todoItemRepository.GetItems(userId, pagingParameters);
        }

        public List<Label> GetItemLabels(int itemId)
        {
            var userId = CheckAuthentication();
            return todoItemRepository.GetItemLabels(userId, itemId);
        }
    }
}
