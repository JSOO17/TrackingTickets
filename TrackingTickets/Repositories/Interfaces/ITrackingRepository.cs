using System.Collections.Generic;
using System.Threading.Tasks;
using TrackingTickets.Models;

namespace TrackingTickets.Repositories.Interfaces
{
    public interface ITrackingRepository
    {
        Task Insert(List<EventTrackingModel> trackingModels);
        Task<List<EventTrackingModel>> GetByTicketId(int ticketId);
    }
}
