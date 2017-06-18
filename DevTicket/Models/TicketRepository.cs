using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DevTicket.Models
{
    public class TicketNotFoundException : Exception
    {
    }

    public class TicketRepository : ITicketRepository
    {
        private DevTicketContext _db = new DevTicketContext();

        public TicketRepository()
        {
            _db = new DevTicketContext();
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public IEnumerable<Ticket> GetAll()
        {
            return _db.Tickets.Where(t => t.Closed == null);
        }

        public async Task<Ticket> GetById(int id)
        {
            return await _db.Tickets.FindAsync(id);
        }

        public async Task<Ticket> Add(Ticket ticket)
        {
            ticket.Created = DateTime.Now;
            _db.Tickets.Add(ticket);

            await _db.SaveChangesAsync();
            return ticket;
        }

        public async Task Update(Ticket ticket)
        {
            _db.Entry(ticket).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task Pickup(int id, User user)
        {
            Ticket ticket = await _db.Tickets.FindAsync(id);
            if (ticket == null)
                throw new TicketNotFoundException();

            ticket.PickedUp = DateTime.Now;
            ticket.PickedUpBy = user.Name;
            _db.Entry(ticket).State = EntityState.Modified;

            await _db.SaveChangesAsync();
        }

        public async Task<Ticket> Delete(int id)
        {
            Ticket ticket = await _db.Tickets.FindAsync(id);
            if (ticket == null)
                throw new TicketNotFoundException();

            ticket.Closed = DateTime.Now;
            _db.Entry(ticket).State = EntityState.Modified;

            await _db.SaveChangesAsync();
            return ticket;
        }

    }
}