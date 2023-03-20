using Microsoft.EntityFrameworkCore;
using TestTaskApi.Models.Entities;

namespace TestTaskApi.Models.DbContext;

public class ApplicationDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<JobTitle> JobTitles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
    }

    [Obsolete("Obsolete")]
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<JobTitle>()
            .HasCheckConstraint("CT_Restrict_value_in_boundaries",
                "`grade` >=1 AND `grade` <= 15");
    }
}