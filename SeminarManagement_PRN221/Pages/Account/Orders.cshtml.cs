using BusinessObject.DTO;
using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SeminarManagement_PRN221.Pages.Account
{
    public class OrdersModel : PageModel
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IFeedBackRepository _feedbackRepository;

        public OrdersModel(IBookingRepository bookingRepository, IFeedBackRepository feedbackRepository)
        {
            _bookingRepository = bookingRepository;
            _feedbackRepository = feedbackRepository;
        }

        public IEnumerable<BookingDto> Bookings { get; set; }

        [BindProperty]
        public string FeedbackContent { get; set; }

        [BindProperty]
        public Guid BookingId { get; set; }

        [BindProperty]
        public Guid EventId { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var nameIdentifier = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (nameIdentifier != null)
            {
                var userId = Guid.Parse(nameIdentifier.Value);
                Bookings = await _bookingRepository.GetBookingsOfUser(userId);
            }
            else
            {
                return RedirectToPage("/Account/Login");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostSubmitFeedbackAsync()
        {
            var nameIdentifier = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (nameIdentifier == null)
            {
                return RedirectToPage("/Account/Login");
            }

            var userId = Guid.Parse(nameIdentifier.Value);

            if (EventId == Guid.Empty || string.IsNullOrEmpty(FeedbackContent))
            {
                TempData["ErrorMessage"] = "Invalid feedback submission.";
                return RedirectToPage();
            }

            if (await _feedbackRepository.FeedbackExistsAsync(EventId, userId))
            {
                TempData["ErrorMessage"] = "You have already submitted feedback for this event.";
                return RedirectToPage();
            }

            var feedback = new Feedback
            {
                FeedbackId = Guid.NewGuid(),
                FeedBackContent = FeedbackContent,
                CreatedDate = DateTime.Now,
                FinishDate = DateTime.Now.AddDays(7), // Example finish date, adjust as necessary
                EventId = EventId,
                UserId = userId
            };

            try
            {
                await _feedbackRepository.AddAsync(feedback);
                TempData["SuccessMessage"] = "Feedback submitted successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error submitting feedback: {ex.Message}";
            }

            return RedirectToPage();
        }
    }
}