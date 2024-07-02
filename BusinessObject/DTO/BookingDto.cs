using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
    public class BookingDto
    {
        public Guid BookingId { get; set; }
        public Guid EventId { get; set; }
        public string? EventName { get; set; }
        public string? EventCode { get; set; }
        public DateTime? EventStartDate { get; set; }
        public DateTime? EventEndDate { get; set; }
        public decimal? EventFee { get; set; }
        public int? TotalTicket { get; set; }
        public decimal? TotalAmount { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? TransactionStatus { get; set; }
    }
}
