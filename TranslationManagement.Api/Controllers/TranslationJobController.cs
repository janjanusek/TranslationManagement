using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using External.ThirdParty.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TranslationManagement.Api.Controlers;
using TranslationManagement.Api.TranslationJob;

namespace TranslationManagement.Api.Controllers
{
    [ApiController]
    [Route("api/jobs/[action]")]
    public class TranslationJobController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TranslationJobController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<GetJobs.Response>> GetJobs(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetJobs.Request(), cancellationToken);
            return Ok(result);
        }
        
        [HttpPost]
        public async Task<ActionResult> CreateJob([FromBody]CreateJob.Request job, CancellationToken cancellationToken)
        {
            await _mediator.Send(job, cancellationToken);
            return this.NoContent();
        }

        [HttpPost]
        public async Task<ActionResult> CreateJobWithFile([FromBody]CreateJobWithFile.Request request, CancellationToken cancellationToken)
        {
            await _mediator.Send(request, cancellationToken);
            return this.NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<UpdateJobStatus.Response>> UpdateJobStatus([FromBody]UpdateJobStatus.Request request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }
    }
}