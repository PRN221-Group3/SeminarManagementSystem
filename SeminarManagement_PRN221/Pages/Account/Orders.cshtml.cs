using BusinessObject.DTO;
using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories.Interfaces;
using System.Security.Claims;

namespace SeminarManagement_PRN221.Pages.Account
{
    public class OrdersModel : PageModel
    {
        private readonly IBookingRepository _bookingRepository;
        public OrdersModel(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository; 
        }
        public IEnumerable<BookingDto> Bookings { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var nameIdentifier = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (nameIdentifier != null) {
                var userId = Guid.Parse(nameIdentifier.Value);
                Bookings = await _bookingRepository.GetBookingsOfUser(userId);
            }
            else
            {
                return RedirectToPage("/Account/Login");
            }
            return Page();
        }
    }
}
