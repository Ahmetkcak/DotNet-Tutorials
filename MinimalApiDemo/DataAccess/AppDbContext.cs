using Microsoft.EntityFrameworkCore;
using MinimalApiDemo.Models.Entities;

namespace MinimalApiDemo.DataAccess;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<User> Users { get; set; }
}
