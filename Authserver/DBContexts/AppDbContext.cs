using Microsoft.EntityFrameworkCore;
using Authserver.Models;

namespace Authserver.DBContexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
}