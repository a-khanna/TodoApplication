using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Todo.Core.Helpers;
using Todo.Core.Models.Sql;
using Todo.Data;

namespace Todo.UnitTests.Data
{
    public class TodoTestsBase
    {
        protected TodoDbContext SetupDatabase(string dbname)
        {
            var builder = new DbContextOptionsBuilder<TodoDbContext>()
                .UseInMemoryDatabase(databaseName: dbname);

            var dbContext = new TodoDbContext(builder.Options);

            var saltAndHash = CryptographyHelper.GetSaltAndHash("asdf");
            var user = new User
            {
                FirstName = "testname",
                Username = "testuser",
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
                            Labels = new List<Label>
                            {
                                new Label
                                {
                                    Name = "Today"
                                }
                            }
                        },
                        new TodoItem
                        {
                            Description = "Eggs",
                        },
                        new TodoItem
                        {
                            Description = "Bread",
                        }
                    },
                    Labels = new List<Label>
                    {
                        new Label
                        {
                            Name = "Important"
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
                            Labels = new List<Label>
                            {
                                new Label
                                {
                                    Name = "Today"
                                }
                            }
                        },
                        new TodoItem
                        {
                            Description = "Code review",
                        },
                        new TodoItem
                        {
                            Description = "Update sprint cycle",
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

            return dbContext;
        }
    }
}
