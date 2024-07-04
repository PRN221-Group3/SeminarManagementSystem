using BusinessObject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

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
    public Dictionary<string, decimal> MonthlyRevenue { get; set; }
    public Dictionary<string, int> MonthlyTicketsSold { get; set; }

    public void OnGet()
    {
        // Fetch bookings
        var bookings = _bookingRepository.GetAllQueryableAsync().Result
            .Where(b => b.CreatedDate.HasValue)
            .ToList();

        // Calculate total revenue and tickets sold
        Revenue = bookings.Sum(b => b.TotalAmount) ?? 0;
        TicketsSold = bookings.Sum(b => b.TotalTicket) ?? 0;
        var eventQueryable = _eventRepository.GetAllQueryableAsync().Result;
        TicketsAvailable = eventQueryable.Where(s => s.EndDate >= DateTime.Now).Select(s => s.NumberOfTickets).Sum();

        // Group data by month
        MonthlyRevenue = bookings
            .GroupBy(b => b.CreatedDate.Value.ToString("MM/dd/yyyy"))
            .ToDictionary(g => g.Key, g => g.Sum(b => b.TotalAmount) ?? 0);

        MonthlyTicketsSold = bookings
            .GroupBy(b => b.CreatedDate.Value.ToString("MM/dd/yyyy"))
            .ToDictionary(g => g.Key, g => g.Sum(b => b.TotalTicket) ?? 0);

        // Fetch other data
        Transactions = _transactionRepository.GetAllQueryableAsync().Result.Include(s => s.Wallet.WalletNavigation).OrderByDescending(s => s.CreationDate).ToList();
        SponsorProducts = _eventSponsorRepository.GetAllQueryableAsync().Result.Include(s => s.Sponsor).OrderByDescending(s => s.EventId).ToList();

        // Get sponsor status data
        SponsorStatusData = _eventSponsorRepository.GetAllQueryableAsync().Result.OrderByDescending(s => s.EventId)
            .GroupBy(s => s.Status ?? "Not Invited")
            .ToDictionary(g => g.Key, g => g.Count());
    }
}
