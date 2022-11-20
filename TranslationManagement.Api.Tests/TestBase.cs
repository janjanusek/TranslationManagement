namespace TranslationManagement.Api.Tests;

public abstract class TestBase : IDisposable
{
    public TestAppDbContext Db { get; }
    
    public TestBase()
    {
        Db = new TestAppDbContext();
    }
    
    public void Dispose()
    {
        Db.Dispose();
    }
}