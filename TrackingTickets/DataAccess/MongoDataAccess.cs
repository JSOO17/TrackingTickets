using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrackingTickets.Config;

namespace TrackingTickets.DataAccess
{
    public class MongoDataAccess
    {
        private readonly IMongoCollection<BsonDocument> _collection;

        public MongoDataAccess(IOptions<CosmosDbConfig> options)
        {
            var config = options.Value;
            var client = new MongoClient(config.ConnectionString);
            var database = client.GetDatabase(config.DatabaseName);
            _collection = database.GetCollection<BsonDocument>(config.CollectionName);
        }

        public async Task InsertDocumentsAsync(List<BsonDocument> documents) => await _collection.InsertManyAsync(documents);

        public async Task<List<BsonDocument>> GetDocumentsByTicketId(int ticketId)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("ticketId", ticketId);
            var result = await _collection.FindAsync(filter).ConfigureAwait(false);

            return await result.ToListAsync();
        }
    }
}
