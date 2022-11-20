using System.Threading;
using System.Threading.Tasks;
using External.ThirdParty.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using Polly;
using TranslationManagement.Dal;
using TranslationManagement.Dal.Model;

namespace TranslationManagement.Api.TranslationJob;

public class CreateJob
{
    public class Request : IRequest
    {
        public string CustomerName { get; set; }
        public string OriginalContent { get; set; }
    }

    public class Handler : IRequestHandler<Request>
    {
        private const double PricePerCharacter = 0.01;
        private readonly INotificationService _notificationService;
        private readonly ILogger<CreateJob> _logger;
        private readonly AppDbContext _context;

        private static readonly IAsyncPolicy<bool> _retryPolicy = Policy<bool>
            .HandleResult(b => !b)
            .RetryForeverAsync();

        public Handler(INotificationService notificationService, ILogger<CreateJob> logger, AppDbContext context)
        {
            _notificationService = notificationService;
            _logger = logger;
            _context = context;
        }

        public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
        {
            var job = new Dal.Model.TranslationJob()
            {
                CustomerName = request.CustomerName,
                Status = JobStatus.New,
                OriginalContent = request.OriginalContent,
            };
            job.SetPrice(PricePerCharacter);

            _context.TranslationJobs.Add(job);
            await _context.SaveChangesAsync(cancellationToken);

            await _retryPolicy
                .ExecuteAsync((_) => _notificationService.SendNotification($"Job created: {job.Id}"), cancellationToken);
            _logger.LogInformation("New job notification sent");

            return Unit.Value;
        }
    }
}