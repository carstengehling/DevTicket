using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTicket.Models
{
    public interface ITicketRepository : IDisposable
    {
        IEnumerable<Ticket> GetAll();
        Task<Ticket> GetById(int id);
        Task<Ticket> Add(Ticket ticket);
        Task Update(Ticket ticket);
        Task Pickup(int id, User user);
        Task<Ticket> Delete(int id);
    }
}
