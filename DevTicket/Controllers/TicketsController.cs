using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using DevTicket.Models;

namespace DevTicket.Controllers
{
    public class TicketsController : ApiController
    {
        private DevTicketContext db = new DevTicketContext();

        // GET: api/Ticketso
        [ActionName("DefaultAction")]
        public IQueryable<Ticket> GetTickets()
        {
            return db.Tickets.Where(t => t.Closed == null);
        }

        // GET: api/Tickets/5
        [ActionName("DefaultAction")]
        [ResponseType(typeof(Ticket))]
        public async Task<IHttpActionResult> GetTicket(int id)
        {
            Ticket ticket = await db.Tickets.FindAsync(id);
            if (ticket == null)
                return NotFound();

            return Ok(ticket);
        }

        //// PUT: api/Tickets/5
        [ActionName("DefaultAction")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTicket(int id, Ticket ticket)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != ticket.Id)
                return BadRequest();

            db.Entry(ticket).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketExists(id))
                    return NotFound();
                else
                    throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Tickets
        [ActionName("DefaultAction")]
        [ResponseType(typeof(Ticket))]
        public async Task<IHttpActionResult> PostTicket(Ticket ticket)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ticket.Created = DateTime.Now;
            db.Tickets.Add(ticket);

            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = ticket.Id }, ticket);
        }

        // DELETE: api/Tickets/5
        [ActionName("DefaultAction")]
        [ResponseType(typeof(Ticket))]
        public async Task<IHttpActionResult> DeleteTicket(int id)
        {
            Ticket ticket = await db.Tickets.FindAsync(id);
            if (ticket == null)
                return NotFound();

            ticket.Closed = DateTime.Now;
            db.Entry(ticket).State = EntityState.Modified;

            await db.SaveChangesAsync();

            return Ok(ticket);
        }

        // POST: api/Tickets/5/Pickup
        [ResponseType(typeof(void))]
        [HttpPost]
        [ActionName("Pickup")]
        public async Task<IHttpActionResult> Pickup(int id, User user)
        {
            Ticket ticket = await db.Tickets.FindAsync(id);
            if (ticket == null)
                return NotFound();

            ticket.PickedUp = DateTime.Now;
            ticket.PickedUpBy = user.Name;
            db.Entry(ticket).State = EntityState.Modified;

            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }

        private bool TicketExists(int id)
        {
            return db.Tickets.Count(e => e.Id == id) > 0;
        }
    }
}