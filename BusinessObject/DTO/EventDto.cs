using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject.DTO
{
    public class EventDto
    {
        public Guid EventId { get; set; }

        [Required]
        public string EventName { get; set; }

        [Required]
        [RegularExpression(@"^E\d{4}$", ErrorMessage = "Event Code must be in the format Exxxx where x is a number.")]
        public string EventCode { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public string Description { get; set; }

        [Required]
        [Range(0, 10000, ErrorMessage = "Fee must be between 0 and 10000.")]
        public decimal Fee { get; set; }

        [Required]
        public Guid HallId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Number of tickets must be at least 1.")]
        public int NumberOfTickets { get; set; }
    }
}
