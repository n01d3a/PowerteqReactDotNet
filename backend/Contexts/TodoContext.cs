using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Contexts
{
    public class TodoContext : DbContext
    {
        #region Constructors

        public TodoContext(DbContextOptions contextOptions) : base(contextOptions)
        {

        }

        #endregion Constructors

        #region Properties

        public DbSet<TodoItem> TodoItems { get; set; }

        #endregion Properties
    }
}
