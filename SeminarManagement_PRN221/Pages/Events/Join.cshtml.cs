using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using SeminarManagement_PRN221.Common;
using System.Security.Claims;

namespace SeminarManagement_PRN221.Pages.Events
{
    public class JoinModel : PageModel
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IEventRepository _eventRepository;
        private readonly ITransactionRepository _transactionRepository;

        public JoinModel(IWalletRepository walletRepository,
            IEventRepository eventRepository,
            ITransactionRepository transactionRepository)
        {
            _walletRepository = walletRepository;
            _eventRepository = eventRepository;
            _transactionRepository = transactionRepository;
        }
        [BindProperty(SupportsGet = true)]
        public Guid EventId { get; set; }
        [BindProperty]
        public Event Event { get; set; }
        [BindProperty]
        public Wallet Wallet { get; set; }
        [BindProperty]
        public decimal? TotalMoney { get; set; }
        [BindProperty]
        public int? Quantity { get; set; } = 1;
        [BindProperty]
        public int MaxQuantity { get; set; }
        [BindProperty]
        public decimal? Fee { get; set; }
        [BindProperty]
        public decimal? Balance { get; set; }
        [BindProperty]
        public Transaction? TransactionExist {  get; set; }
        public async Task<IActionResult> OnGet()
        {
            var allEvents = await _eventRepository.GetAllQueryableAsync();

            Event = allEvents.Include(s => s.Hall).FirstOrDefault(s => s.EventId == EventId);

            if (Event == null)
            {
                return NotFound();
            }
            var nameIdentifierClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (nameIdentifierClaim == null)
            {
                return Page();
            }
            else
            {
                var userId = Guid.Parse(nameIdentifierClaim.Value);
                Wallet = _walletRepository.GetById(userId);

                MaxQuantity = Event.NumberOfTickets ?? 0;

                if(MaxQuantity == 0)
                {
                    return RedirectToPage("/Index");
                }

                if(Quantity > MaxQuantity)
                {
                    return Page();
                }

                if (Event.Fee <= 0)
                {
                    TransactionExist = await _transactionRepository.GetByWalletId(Wallet.WalletId);
                }

                TotalMoney = Event.Fee * Quantity;

                QrCodeGenerator.GenerateQRCode(Event);

                return Page();
            }
        }

        public IActionResult OnPost()
        {
            if(Balance == null)
            {
                Balance = 0;
            }
            Fee = Event.Fee * Quantity;
            Balance -= Fee;
            return RedirectToPage("Pay", new { 
                eventId = Event.EventId,
                walletId = Wallet.WalletId,
                total = Fee,
                balance = Balance,
                quantity = Quantity
            });
        }
    }
}