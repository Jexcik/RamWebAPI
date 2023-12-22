using Microsoft.EntityFrameworkCore;
using RamWebAPI.Models;
using System.Collections.Generic;

namespace RamWebAPI
{
    public class DatabaseContext:DbContext
    {
        public DbSet<User> Users { get; set; }
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            //Создаем базу данных при первом вызове
            Database.EnsureCreated();
        }
    }
}
