using Microsoft.EntityFrameworkCore;

namespace Backend.Database;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {
    }

    // Define your DbSets here, for example:
    // public DbSet<Item> Items { get; set; }    
}
