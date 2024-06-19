using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
    public   class EventDto
    {
        [Required(ErrorMessage = "Event Name is required")]
        public string EventName { get; set; }
        [Required(ErrorMessage = "Event Code is required")]
        public string EventCode { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [Required(ErrorMessage = "Fee is required")]
        public decimal Fee { get; set; }
        public Guid? HallId { get; set; }
        [Required(ErrorMessage = "Status is required")]
        public string Status { get; set; }
        public bool IsDeleted { get; set; }
    }
}
