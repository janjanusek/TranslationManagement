using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TranslationManagement.Dal;

namespace TranslationManagement.Api.TranslationJob;

public class GetJobs
{
    public class Request : IRequest<Response>
    {
    }

    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly ILogger<GetJobs> _logger;
        private readonly AppDbContext _context;

        public Handler(ILogger<GetJobs> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            return new Response()
            {
                Jobs = await _context.TranslationJobs.ToListAsync(cancellationToken)
            };
        }
    }

    public class Response
    {
        public List<Dal.Model.TranslationJob> Jobs { get; init; }
    }
}