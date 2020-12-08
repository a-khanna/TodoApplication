using System;
using System.Collections.Generic;
using System.Linq;
using Todo.Core.Helpers;
using Todo.Core.Models.Sql;

namespace Todo.Data
{
    /// <summary>
    /// Database seeder to be used at the application startup
    /// </summary>
    public static class DbSeeder
    {
        /// <summary>
        /// Extension method to seed database.
        /// Does nothing if there is already some data in the database.
        /// </summary>
        /// <param name="dbContext"></param>
        public static void Seed(this TodoDbContext dbContext)
        {
            dbContext.Database.EnsureCreated();

            if (dbContext.TodoLists.Any())
                return;

            var saltAndHash = CryptographyHelper.GetSaltAndHash("asdf");
            var user = new User
            {
                FirstName = "Anirudh",
                LastName = "Khanna",
                Username = "anirudh",
                PasswordSalt = saltAndHash.Item1,
                PasswordHash = saltAndHash.Item2
            };

            var todoLists = new List<TodoList>
            {
                new TodoList
                {
                    Name = "Shopping list",
                    Items = new List<TodoItem>
                    {
                        new TodoItem
                        {
                            Description = "Milk",
                        },
                        new TodoItem
                        {
                            Description = "Eggs",
                        },
                        new TodoItem
                        {
                            Description = "Bread",
                        },
                        new TodoItem
                        {
                            Description = "Pasta",
                        },
                        new TodoItem
                        {
                            Description = "Chocolate",
                        }
                    }
                },
                new TodoList
                {
                    Name = "Office work",
                    Items = new List<TodoItem>
                    {
                        new TodoItem
                        {
                            Description = "Write API",
                        },
                        new TodoItem
                        {
                            Description = "Code review",
                        },
                        new TodoItem
                        {
                            Description = "Update sprint cycle",
                        },
                        new TodoItem
                        {
                            Description = "Slap a fish",
                        },
                        new TodoItem
                        {
                            Description = "Code coverage",
                        }
                    }
                }
            };

            // update metadata for all entities
            foreach (var list in todoLists)
            {
                list.LastModified = DateTime.Now;
                list.User = user;

                foreach (var item in list.Items)
                {
                    item.LastModified = DateTime.Now;
                    item.User = user;
                }
            }

            dbContext.Users.Add(user);
            dbContext.TodoLists.AddRange(todoLists);

            dbContext.SaveChanges();
        }
    }
}
