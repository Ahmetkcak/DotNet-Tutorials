using Microsoft.EntityFrameworkCore;

namespace HangfireDemo.Context;

public sealed class ApplicationDbContext : DbContext
{ 
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    
}
