using AspNetSemester.Models;
using AspNetSemester.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace AspNetSemester.Contexts;

public class DocumentContext : DbContext
{
    public DocumentContext(DbContextOptions<DocumentContext> options) : base(options) { }
    public DbSet<Document> Documents { get; set; }
    public DbSet<DocumentAccess> DocumentAccesses { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost:5555;Username=postgres;Password=1;Database=orishw");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DocumentAccess>().ToTable("DocumentAccess"); 
        modelBuilder.Entity<Document>().ToTable("Document");

    }
}