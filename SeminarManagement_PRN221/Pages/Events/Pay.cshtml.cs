using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System.Net;
using System.Net.Mail;

namespace SeminarManagement_PRN221.Pages.Events
{
    public class PayModel : PageModel
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IEventRepository _eventRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IBookingRepository _bookingRepository;
        public PayModel(IWalletRepository walletRepository,
            IEventRepository eventRepository,
            ITransactionRepository transactionRepository,
            ITicketRepository ticketRepository,
            IBookingRepository bookingRepository)
        {
            _walletRepository = walletRepository;
            _eventRepository = eventRepository;
            _transactionRepository = transactionRepository;
            _ticketRepository = ticketRepository;
            _bookingRepository = bookingRepository;
        }
        public Guid EventId { get; set; }
        [BindProperty]
        public Event Event { get; set; }
        [BindProperty]
        public Wallet Wallet { get; set; }
        [BindProperty]
        public decimal TotalMoney { get; set; }
        [BindProperty]
        public Transaction? TransactionExist { get; set; }
        [BindProperty]
        public int Quantity { get; set; }
        [BindProperty]
        public decimal Balance { get; set; }
        public async Task<IActionResult> OnGet(Guid eventId, Guid walletId, decimal total, decimal balance, int quantity)
        {
            EventId = eventId;
            Quantity = quantity;


            var allEvents = await _eventRepository.GetAllQueryableAsync();
            Event = allEvents.Include(s => s.Hall).FirstOrDefault(s => s.EventId == EventId);
            if (Event == null)
            {
                return NotFound();
            }
            TotalMoney = total;

            Wallet = _walletRepository.GetById(walletId);

            if (Wallet.Balance < TotalMoney)
            {
                ViewData["msg$Not"] = "Not afforadble. Please cash in to your wallet to proceed payment";
            }

            //QrCodeGenerator.GenerateQRCode(Event);

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            var eventUpdate = _eventRepository.GetById(Event.EventId);
            if (eventUpdate != null)
            {

                eventUpdate.NumberOfTickets -= Quantity;

                Transaction transaction = new()
                {
                    TransactionId = Guid.NewGuid(),
                    CreationDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    TransactionStatus = "Successfull",
                    WalletId = Wallet.WalletId,
                    EventId = Event.EventId,
                    DepositAmount = Event.Fee
                };

                Ticket ticket = new()
                {
                    TicketId = Guid.NewGuid(),
                    EventId = Event.EventId,
                    Price = Event.Fee,
                    CreatedDate = transaction.CreationDate,
                    UpdatedDate = transaction.UpdateDate,
                };

                Booking booking = new()
                {
                    BookingId = Guid.NewGuid(),
                    CreatedDate = transaction.CreationDate,
                    UpdatedDate = transaction.UpdateDate,
                    UserId = Wallet.WalletId
                };

                var wallet = _walletRepository.GetById(Wallet.WalletId);
                wallet.Balance = Balance - TotalMoney;

                booking.Tickets.Add(ticket);

                await _ticketRepository.AddAsync(ticket);
                await _bookingRepository.AddAsync(booking);
                await _walletRepository.UpdateAsync(wallet);
                await _transactionRepository.AddAsync(transaction);
                await _eventRepository.UpdateAsync(eventUpdate);
                return RedirectToPage("/Index");
            }
            return Page();
        }

        private void SendVerificationEmail(string email)
        {
            var myEmail = "seminarwebapp@gmail.com";
            var myPassword = "mbyghvpzorxaihmp";

            var message = new MailMessage();
            message.From = new MailAddress(myEmail);
            message.To.Add(new MailAddress(email));
            message.Subject = "[PRN221] Email Verification";

            message.Body = @"
                <html>
                <body>
                    <h2>Welcome to Seminar Web</h2>
                    <p>Thank you for registering. Please verify your email by clicking the button below:</p>
                <ul>
                    <li><strong>Ticket ID:</strong> {ticket.TicketId}</li>
                    <li><strong>Event ID:</strong> {ticket.EventId}</li>
                    <li><strong>Price:</strong> ${ticket.Price}</li>
                    <li><strong>Created Date:</strong> {ticket.CreatedDate}</li>
                    <li><strong>Updated Date:</strong> {ticket.UpdatedDate}</li>
                </ul>
                    <br></br>
                    <p>If you did not request this email, please ignore it.</p>
                    <p>Best regards,<br>PRN221 Team</p>
                </body>
                </html>";

            message.IsBodyHtml = true;

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(myEmail, myPassword),
                EnableSsl = true
            };

            smtpClient.Send(message);
        }
    }
}

