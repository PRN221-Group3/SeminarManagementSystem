using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject.DTO
{
    public class EventDto
    {
        public Guid EventId { get; set; }

        [Required]
        [StringLength(100)]
        public string EventName { get; set; }

        [Required]
        [RegularExpression(@"^E\d{4}$", ErrorMessage = "Event Code must be in the format Exxxx where x is a number.")]
        public string EventCode { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        [CustomDateRange("1/1/1753", "12/31/9999", ErrorMessage = "Start Date must be between 01/01/1753 and 31/12/9999")]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        [CustomDateRange("1/1/1753", "12/31/9999", ErrorMessage = "End Date must be between 01/01/1753 and 31/12/9999")]
        public DateTime EndDate { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        public decimal Fee { get; set; }

        [Required]
        public Guid HallId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Number of tickets must be at least 1.")]
        public int NumberOfTickets { get; set; }
    }

    public class CustomDateRangeAttribute : ValidationAttribute
    {
        private readonly DateTime _minDate;
        private readonly DateTime _maxDate;

        public CustomDateRangeAttribute(string minDate, string maxDate)
        {
            _minDate = DateTime.Parse(minDate, System.Globalization.CultureInfo.InvariantCulture);
            _maxDate = DateTime.Parse(maxDate, System.Globalization.CultureInfo.InvariantCulture);
        }

        public override bool IsValid(object value)
        {
            DateTime dateValue = Convert.ToDateTime(value, System.Globalization.CultureInfo.InvariantCulture);
            return dateValue >= _minDate && dateValue <= _maxDate;
        }
    }
}
