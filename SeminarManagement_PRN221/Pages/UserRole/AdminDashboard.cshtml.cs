using BusinessObject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.Interfaces;
using Newtonsoft.Json;

namespace SeminarManagement_PRN221.Pages.UserRole.AdminDashboard;

[Authorize(Roles = "Operator")]
public class IndexModel : PageModel
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IEventSponsorRepository _eventSponsorRepository;
    private readonly ISponsorRepository _sponsorRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUserRepository _userRepository;

    public IndexModel(IBookingRepository bookingRepository, ITransactionRepository transactionRepository,
        IEventRepository eventRepository, IEventSponsorRepository eventSponsorRepository,
        ISponsorRepository sponsorRepository, IUserRepository userRepository)
    {
        _bookingRepository = bookingRepository;
        _transactionRepository = transactionRepository;
        _eventRepository = eventRepository;
        _eventSponsorRepository = eventSponsorRepository;
        _sponsorRepository = sponsorRepository;
        _userRepository = userRepository;
    }

    public int? TicketsSold { get; set; }
    public int? TicketsAvailable { get; set; }
    public decimal Revenue { get; set; }
    public List<Transaction> Transactions { get; set; }
    public List<EventSponsor> SponsorProducts { get; set; }
    public Dictionary<string, int> SponsorStatusData { get; set; }

    public void OnGet()
    {
        // Mock lay so ve da ban Booking Ticket sau
        TicketsSold = 34;
        var eventQueryable = _eventRepository.GetAllQueryableAsync().Result;
        TicketsAvailable = eventQueryable.Select(s => s.NumberOfTickets).Sum();
        // Mock lay Total Amount of Booking Ticket sau
        Revenue = 8000000;
        Transactions = _transactionRepository.GetAllQueryableAsync().Result.Include(s => s.Event).ToList();
        SponsorProducts = _eventSponsorRepository.GetAllQueryableAsync().Result.Include(s => s.Sponsor).ToList();

        // Get sponsor status data
        SponsorStatusData = _eventSponsorRepository.GetAllQueryableAsync().Result
            .GroupBy(s => s.Status ?? "Not Invited")
            .ToDictionary(g => g.Key, g => g.Count());
    }
}
