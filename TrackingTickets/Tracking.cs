using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using TrackingTickets.Models;
using TrackingTickets.Repositories.Interfaces;

namespace TrackingTickets
{
    public class Tracking
    {
        private readonly ITrackingRepository _trackingRepository;
        public Tracking(ITrackingRepository trackingRepository) => _trackingRepository = trackingRepository;

        [FunctionName("Tracking")]
        public async Task Run([EventHubTrigger("tracking", Connection = "EventHubConnection")] EventData[] events, ILogger log)
        {
            var exceptions = new List<Exception>();
            var models = new List<EventTrackingModel>();

            foreach (EventData eventData in events)
            {
                try
                {
                    var eventBodyBytes = eventData.EventBody.ToArray();

                    var model = Encoding.UTF8.GetString(eventBodyBytes);

                    var eventModel = JsonSerializer.Deserialize<EventTrackingModel>(model) ?? throw new Exception();

                    models.Add(eventModel);
                    log.LogInformation($"Recieved event: {eventBodyBytes}");
                    await Task.Yield();
                }
                catch (Exception e)
                {
                    exceptions.Add(e);
                }
            }

            // Once processing of the batch is complete, if any messages in the batch failed processing throw an exception so that there is a record of the failure.

            if (exceptions.Count > 1)
                throw new AggregateException(exceptions);

            if (exceptions.Count == 1)
                throw exceptions.Single();

            await _trackingRepository.Insert(models);
        }
    }
}
