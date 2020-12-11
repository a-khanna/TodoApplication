using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Todo.Core;
using Todo.Core.Abstractions.Data;
using Todo.Core.Helpers;
using Todo.Core.Models.Dtos;
using Todo.Core.Models.Sql;

namespace Todo.API.GraphQL
{
    public class Mutation : Base
    {
        private readonly IUserRepository userRepository;
        private readonly IConfiguration configuration;

        public Mutation(IHttpContextAccessor httpContextAccessor, ITodoListRepository todoListRepository, ITodoItemRepository todoItemRepository, IUserRepository userRepository, IConfiguration configuration)
            : base(httpContextAccessor, todoListRepository, todoItemRepository)
        {
            this.userRepository = userRepository;
            this.configuration = configuration;
        }

        public TodoList CreateList(CreateTodoListDto createListDto)
        {
            var userId = CheckAuthentication();
            return todoListRepository.CreateList(userId, createListDto);
        }

        public TodoItem CreateItem(CreateTodoItemDto createItemDto)
        {
            var userId = CheckAuthentication();
            return todoItemRepository.CreateItem(userId, createItemDto);
        }

        public TodoList UpdateList(TodoListDto updateListDto)
        {
            var userId = CheckAuthentication();
            return todoListRepository.UpdateList(userId, updateListDto);
        }

        public TodoItem UpdateItem(TodoItemDto updateItemDto)
        {
            var userId = CheckAuthentication();
            return todoItemRepository.UpdateItem(userId, updateItemDto);
        }

        public bool DeleteList(int listId)
        {
            var userId = CheckAuthentication();
            return todoListRepository.DeleteList(userId, listId);
        }

        public bool DeleteItem(int itemId)
        {
            var userId = CheckAuthentication();
            return todoItemRepository.DeleteItem(userId, itemId);
        }

        public Label AssignLabelToList(CreateOrDeleteLabelDto createLabelDto)
        {
            var userId = CheckAuthentication();
            return todoListRepository.CreateLabel(userId, createLabelDto);
        }

        public Label AssignLabelToItem(CreateOrDeleteLabelDto createLabelDto)
        {
            var userId = CheckAuthentication();
            return todoItemRepository.CreateLabel(userId, createLabelDto);
        }

        public Label UpdateListLabel(UpdateLabelDto updateLabelDto)
        {
            var userId = CheckAuthentication();
            return todoListRepository.UpdateLabel(userId, updateLabelDto);
        }

        public Label UpdateItemLabel(UpdateLabelDto updateLabelDto)
        {
            var userId = CheckAuthentication();
            return todoItemRepository.UpdateLabel(userId, updateLabelDto);
        }

        public bool DeleteListLabel(CreateOrDeleteLabelDto deleteDto)
        {
            var userId = CheckAuthentication();
            return todoListRepository.DeleteLabel(userId, deleteDto);
        }

        public bool DeleteItemLabel(CreateOrDeleteLabelDto deleteDto)
        {
            var userId = CheckAuthentication();
            return todoItemRepository.DeleteLabel(userId, deleteDto);
        }

        public async Task<bool> RegisterUser(string username, string password, string firstName, string lastName)
        {
            var saltAndHash = CryptographyHelper.GetSaltAndHash(password);
            return await userRepository.CreateUser(username, saltAndHash.Item1, saltAndHash.Item2, firstName, lastName);
        }

        public string VerifyLogin(string username, string password)
        {
            var userFromDb = userRepository.GetUserByUsername(username);
            if (userFromDb == null)
                return null;

            var passwordIsCorrect = CryptographyHelper.CompareHashes(password, userFromDb.PasswordHash, userFromDb.PasswordSalt);
            if (!passwordIsCorrect)
                return null;

            var jwtKey = configuration.GetValue<string>(Constants.AppSettingsJwtKey);
            var jwtIssuer = configuration.GetValue<string>(Constants.AppSettingsJwtIssuer);
            return CryptographyHelper.GenerateJSONWebToken(jwtKey, jwtIssuer, userFromDb.Id.ToString(), userFromDb.Username);
        }
    }
}
