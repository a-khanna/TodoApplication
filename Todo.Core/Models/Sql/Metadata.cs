using System;

namespace Todo.Core.Models.Sql
{
    public class Metadata
    {
        public User User { get; set; }

        public DateTime? LastModified { get; set; }
    }
}
