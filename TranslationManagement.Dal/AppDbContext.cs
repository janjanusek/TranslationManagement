using Microsoft.EntityFrameworkCore;
using TranslationManagement.Dal.Model;

namespace TranslationManagement.Dal;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<TranslationJob> TranslationJobs { get; set; }
    public DbSet<TranslatorModel> Translators { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TranslationJob>().Property(e => e.Status).HasConversion(
            v => v.ToString(),
            v => Enum.Parse<JobStatus>(v)
        );
    }
}