using Castle.Core.Logging;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using TranslationManagement.Api.Exceptions;
using TranslationManagement.Api.Tests.Resources;
using TranslationManagement.Api.TranslationJob;
using TranslationManagement.Dal.Model;
using Xunit;

namespace TranslationManagement.Api.Tests.TranslationJob;

public class UpdateJobStatusTests : TestBase
{
    private readonly UpdateJobStatus.Handler _sut;

    public UpdateJobStatusTests()
    {
        _sut = new UpdateJobStatus.Handler(Substitute.For<ILogger<UpdateJobStatus>>(), Db.ActDb);
    }

    [Theory]
    [InlineData(JobStatus.New, JobStatus.Completed)]
    [InlineData(JobStatus.Completed, JobStatus.Inprogress)]
    [InlineData(JobStatus.None, JobStatus.New)]
    public async Task GivenTranslatorsAndJobStatusesWithValidStatus_WhenJobStatusFound_StatusIsUpdated(JobStatus from, JobStatus to)
    {
        var sutRequest = new UpdateJobStatus.Request()
        {
            Id = 10,
            TranslatorId = 11,
            Status = to
        };
        
        Db.ArrangeDb.Translators.AddTranslator(sutRequest.TranslatorId);
        Db.ArrangeDb.TranslationJobs.AddTranslationJob(sutRequest.Id, from);
        await Db.ArrangeDb.SaveChangesAsync();

        var response = await _sut.Handle(sutRequest, CancellationToken.None);

        var asserJob = await Db.AssertDb.TranslationJobs.SingleOrDefaultAsync(j => j.Id == sutRequest.Id);
        asserJob.Should().NotBeNull();
        asserJob.Status.Should().Be(to);
    }
    
    [Theory]
    [InlineData(JobStatus.Inprogress, JobStatus.None)]
    [InlineData(JobStatus.None, JobStatus.None)]
    public async Task GivenTranslatorsAndJobStatusesWithNotValidStatus_WhenJobStatusFound_StatusIsNotUpdated(JobStatus from, JobStatus to)
    {
        var sutRequest = new UpdateJobStatus.Request()
        {
            Id = 10,
            TranslatorId = 11,
            Status = to
        };
        
        Db.ArrangeDb.Translators.AddTranslator(sutRequest.TranslatorId);
        Db.ArrangeDb.TranslationJobs.AddTranslationJob(sutRequest.Id, from);
        await Db.ArrangeDb.SaveChangesAsync();

        var response = await _sut.Handle(sutRequest, CancellationToken.None);

        var asserJob = await Db.AssertDb.TranslationJobs.SingleOrDefaultAsync(j => j.Id == sutRequest.Id);
        asserJob.Should().NotBeNull();
        asserJob.Status.Should().Be(from);
    }
    
    [Fact]
    public async Task GivenTranslatorsAndJobStatusesWithValidStatus_WhenTranslatorNotFound_NotFoundExceptionIsThrown()
    {
        var sutRequest = new UpdateJobStatus.Request()
        {
            Id = 10,
            TranslatorId = 11,
            Status = JobStatus.Completed
        };
        
        Db.ArrangeDb.TranslationJobs.AddTranslationJob(sutRequest.Id);
        await Db.ArrangeDb.SaveChangesAsync();

        await Assert.ThrowsAsync<NotFoundException>(() => _sut.Handle(sutRequest, CancellationToken.None));
    }
}