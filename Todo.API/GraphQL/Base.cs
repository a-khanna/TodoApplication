using System;
using Microsoft.AspNetCore.Http;
using Todo.Core;
using Todo.Core.Abstractions.Data;

namespace Todo.API.GraphQL
{
    public class Base
    {
        protected readonly IHttpContextAccessor httpContextAccessor;
        protected readonly ITodoListRepository todoListRepository;
        protected readonly ITodoItemRepository todoItemRepository;

        public Base(IHttpContextAccessor httpContextAccessor, ITodoListRepository todoListRepository, ITodoItemRepository todoItemRepository)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.todoListRepository = todoListRepository;
            this.todoItemRepository = todoItemRepository;
        }

        protected int CheckAuthentication()
        {
            var userIdClaim = httpContextAccessor.HttpContext.Items[Constants.UserIdClaim]?.ToString();
            if (string.IsNullOrEmpty(userIdClaim))
            {
                throw new UnauthorizedAccessException("User is not authenticated!");
            }
            return int.Parse(userIdClaim);
        }
    }
}
