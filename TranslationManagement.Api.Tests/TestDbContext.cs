using Microsoft.EntityFrameworkCore;
using TranslationManagement.Dal;

namespace TranslationManagement.Api.Tests;

public class TestDbContext<TContext> : IDisposable where TContext : DbContext
{
    public DbContextOptions<TContext> Options { get; }
    public TContext ActDb { get; private set; }
    public TContext ArrangeDb { get; private set; }
    public TContext AssertDb { get; private set; }

    public TestDbContext(Func<DbContextOptions<TContext>, TContext> ctor)
    {
        Options = new DbContextOptionsBuilder<TContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        ArrangeDb = ctor(Options);
        ActDb = ctor(Options);
        AssertDb = ctor(Options);
    }

    public void Dispose()
    {
        ArrangeDb.Dispose();
        ActDb.Dispose();
        AssertDb.Dispose();
    }
}

public sealed class TestAppDbContext : TestDbContext<AppDbContext>
{
    public TestAppDbContext() : base(options => new AppDbContext(options))
    {
    }
}