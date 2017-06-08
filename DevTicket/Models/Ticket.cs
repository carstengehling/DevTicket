using System;
using System.ComponentModel.DataAnnotations;

namespace DevTicket.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        [Required]
        public string User { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime PickedUp { get; set; }
        public DateTime Closed { get; set; }
    }
}