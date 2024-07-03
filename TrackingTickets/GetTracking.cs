using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TrackingTickets.Repositories.Interfaces;

namespace TrackingTickets
{
    public class GetTracking
    {
        private readonly ITrackingRepository _trackingRepository;
        public GetTracking(ITrackingRepository trackingRepository) => _trackingRepository = trackingRepository;

        [FunctionName("GetTracking")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "{id}")] HttpRequest req, int id,
            ILogger log)
        {
            return new OkObjectResult(await _trackingRepository.GetByTicketId(id));
        }
    }
}
