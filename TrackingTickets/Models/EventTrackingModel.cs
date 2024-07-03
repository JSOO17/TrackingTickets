using Newtonsoft.Json;
using System;

namespace TrackingTickets.Models
{
    [Serializable]
    public class EventTrackingModel
    {
        [JsonProperty("TicketId")]
        public int TicketId { get; set; }
        [JsonProperty("Event")]
        public string Event { get; set; }
        [JsonProperty("Date")]
        public DateTime Date { get; set; }
        [JsonProperty("UserEmail")]
        public string UserEmail { get; set; }
    }
}
