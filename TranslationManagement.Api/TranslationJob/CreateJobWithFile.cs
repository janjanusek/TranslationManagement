using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TranslationManagement.Dal;

namespace TranslationManagement.Api.TranslationJob;

public class CreateJobWithFile
{
    public class Request : IRequest
    {
        public IFormFile File { get; set; }

        public string Customer { get; set; }
    }

    public class Handler : IRequestHandler<Request>
    {
        private readonly IMediator _mediator;

        public Handler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
        {
            var readingResponse = request?.File switch
            {
                { FileName: ".txt" } => await ReadTxtFileAsync(request),
                { FileName: ".xml" } => await ReadXmlFileAsync(request),
                _ => throw new NotSupportedException("unsupported file")
            };

            await _mediator.Send(new CreateJob.Request
            {
                CustomerName = readingResponse.CustomerName,
                OriginalContent = readingResponse.Content
            }, cancellationToken);

            return Unit.Value;
        }

        private static async Task<ReadingResponse> ReadTxtFileAsync(Request request)
        {
            using var reader = new StreamReader(request.File.OpenReadStream());
            return new ReadingResponse(
                Content: await reader.ReadToEndAsync(),
                CustomerName: request.Customer
            );
        }


        private static async Task<ReadingResponse> ReadXmlFileAsync(Request request)
        {
            var response = await ReadTxtFileAsync(request);
            var xDoc = XDocument.Parse(response.Content);
            return new ReadingResponse(
                Content: xDoc.Root.Element("Content").Value,
                CustomerName: xDoc.Root.Element("Customer").Value.Trim()
            );
        }

        private record ReadingResponse(string Content, string CustomerName);
    }
}