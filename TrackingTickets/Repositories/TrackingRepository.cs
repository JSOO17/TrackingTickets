using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TrackingTickets.DataAccess;
using TrackingTickets.Models;
using TrackingTickets.Repositories.Interfaces;

namespace TrackingTickets.Repositories
{
    public class TrackingRepository : ITrackingRepository
    {
        private readonly MongoDataAccess _mongoDataAccess;
        public TrackingRepository(MongoDataAccess mongoDataAccess)
        {
            _mongoDataAccess = mongoDataAccess;
        }

        public async Task<List<EventTrackingModel>> GetByTicketId(int ticketId)
        {
            return (await _mongoDataAccess.GetDocumentsByTicketId(ticketId)).ConvertAll(bsonDocument => new EventTrackingModel
            {
                TicketId = bsonDocument["ticketId"].ToInt32(),
                Event = bsonDocument["event"].ToString(),
                Date = Convert.ToDateTime(bsonDocument["date"]),
                UserEmail = bsonDocument["userEmail"].ToString()
            });
        }

        public async Task Insert(List<EventTrackingModel> trackingModels)
        {

            var documents = trackingModels.ConvertAll(tracking => new BsonDocument
            {
                { "ticketId", tracking.TicketId },
                { "event", tracking.Event },
                { "date", tracking.Date },
                { "userEmail", tracking.UserEmail }
            });

            await _mongoDataAccess.InsertDocumentsAsync(documents);
        }
    }
}
