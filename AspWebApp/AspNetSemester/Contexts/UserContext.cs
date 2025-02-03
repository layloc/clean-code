using AspNetSemester.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetSemester.Contexts;

public class UserContext : DbContext
{
    public UserContext(DbContextOptions<UserContext> options) : base(options) {}
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost:5555;Username=postgres;Password=1;Database=orishw");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("Users"); 
    }
}