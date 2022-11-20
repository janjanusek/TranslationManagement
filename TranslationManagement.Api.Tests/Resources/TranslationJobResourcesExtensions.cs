using Microsoft.EntityFrameworkCore;
using TranslationManagement.Dal.Model;

namespace TranslationManagement.Api.Tests.Resources;

public static class TranslationJobResourcesExtensions
{
    public static void AddTranslationJob(this DbSet<Dal.Model.TranslationJob> dbSet, int id = 10, JobStatus jobStatus = JobStatus.New)
    {
        dbSet.Add(new Dal.Model.TranslationJob(id, "Some customer", jobStatus, "ASD DSA", null, 0));
    }
}

public static class TranslatorModelResourcesExtensions
{
    public static void AddTranslator(this DbSet<TranslatorModel> dbSet, int id = 11)
    {
        dbSet.Add(new TranslatorModel(id));
    }
}