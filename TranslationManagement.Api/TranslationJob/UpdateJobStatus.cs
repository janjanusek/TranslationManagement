using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TranslationManagement.Api.Exceptions;
using TranslationManagement.Dal;
using TranslationManagement.Dal.Model;

namespace TranslationManagement.Api.TranslationJob;

public class UpdateJobStatus
{
    public class Request : IRequest<Response>
    {
        public int Id { get; set; }
        public int TranslatorId { get; set; }
        public JobStatus Status { get; set; }
    }

    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly ILogger<UpdateJobStatus> _logger;
        private readonly AppDbContext _context;

        public Handler(ILogger<UpdateJobStatus> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Job status update request received: {request.Status} for job {request.Id} by translator {request.TranslatorId}");

            if (request.Status == JobStatus.None)
                return new Response() { Answer = "invalid status" };

            var translator = await _context
                .Translators
                .SingleOrDefaultAsync(t => t.Id == request.TranslatorId, cancellationToken);

            if (translator is null)
                throw new NotFoundException(new { request.TranslatorId });

            var job = await _context
                .TranslationJobs
                .SingleAsync(j => j.Id == request.Id, cancellationToken);

            if (job.TryChangeStatus(request.Status))
            {
                await _context.SaveChangesAsync(cancellationToken);
                return new Response()
                {
                    Answer = "updated"
                };
            }

            return new Response()
            {
                Answer = "invalid status change"
            };
        }
    }

    public class Response
    {
        public string Answer { get; init; } // for this would be better to add some error codes but I'm doing it for free :D
    }
}