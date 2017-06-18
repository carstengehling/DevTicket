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
        ITicketRepository _repo;

        public TicketsController(ITicketRepository repo)
        {
            if (repo == null) throw new ArgumentNullException("repo");
            _repo = repo;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _repo.Dispose();
            base.Dispose(disposing);
        }

        // GET: api/Ticketso
        [ActionName("DefaultAction")]
        public IEnumerable<Ticket> GetTickets()
        {
            return _repo.GetAll();
        }

        // GET: api/Tickets/5
        [ActionName("DefaultAction")]
        [ResponseType(typeof(Ticket))]
        public async Task<IHttpActionResult> GetTicket(int id)
        {
            Ticket ticket = await _repo.GetById(id);
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

            Ticket t = await _repo.GetById(id);
            if (t == null)
                return NotFound();

            await _repo.Update(ticket);

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Tickets
        [ActionName("DefaultAction")]
        [ResponseType(typeof(Ticket))]
        public async Task<IHttpActionResult> PostTicket(Ticket ticket)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _repo.Add(ticket);

            return CreatedAtRoute("DefaultApi", new { id = ticket.Id }, ticket);
        }

        // POST: api/Tickets/5/Pickup
        [ResponseType(typeof(void))]
        [HttpPost]
        [ActionName("Pickup")]
        public async Task<IHttpActionResult> Pickup(int id, User user)
        {
            await _repo.Pickup(id, user);
            return StatusCode(HttpStatusCode.NoContent);
        }

        // DELETE: api/Tickets/5
        [ActionName("DefaultAction")]
        [ResponseType(typeof(Ticket))]
        public async Task<IHttpActionResult> DeleteTicket(int id)
        {
            try
            {
                Ticket ticket = await _repo.Delete(id);
                return Ok(ticket);
            }
            catch (TicketNotFoundException)
            {
                return NotFound();
            }
        }
    }
}