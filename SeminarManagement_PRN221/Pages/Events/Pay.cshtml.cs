using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using QRCoder;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SeminarManagement_PRN221.Pages.Events
{
    public class PayModel : PageModel
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IEventRepository _eventRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHubContext<SeminarHub> _hubContext;

        public PayModel(IWalletRepository walletRepository,
                        IEventRepository eventRepository,
                        ITransactionRepository transactionRepository,
                        ITicketRepository ticketRepository,
                        IBookingRepository bookingRepository,
                        IUserRepository userRepository,
                        IHubContext<SeminarHub> hubContext)
        {
            _walletRepository = walletRepository;
            _eventRepository = eventRepository;
            _transactionRepository = transactionRepository;
            _ticketRepository = ticketRepository;
            _bookingRepository = bookingRepository;
            _userRepository = userRepository;
            _hubContext = hubContext;
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
            TotalMoney = total;
            Balance = balance;

            var eventsQueryable = await _eventRepository.GetAllQueryableAsync();
            Event = await eventsQueryable
                .Include(e => e.Hall)
                .FirstOrDefaultAsync(e => e.EventId == EventId);

            if (Event == null)
            {
                return NotFound();
            }

            Wallet = await _walletRepository.GetByIdAsync(walletId);

            if (Wallet.Balance < TotalMoney)
            {
                ViewData["msg$Not"] = "Not affordable. Please cash in to your wallet to proceed with payment";
            }

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                var eventUpdate = await _eventRepository.GetByIdAsync(Event.EventId);
                if (eventUpdate != null)
                {
                    if (eventUpdate.NumberOfTickets >= Quantity)
                    {
                        eventUpdate.NumberOfTickets -= Quantity;
                    }
                    else
                    {
                        ViewData["msgTicketError"] = "Out of tickets";
                        await _hubContext.Clients.All.SendAsync("ReceiveTicketUpdate", "out_of_tickets");
                        return Page();
                    }

                    Transaction transaction = new()
                    {
                        TransactionId = Guid.NewGuid(),
                        CreationDate = DateTime.Now,
                        UpdateDate = DateTime.Now,
                        TransactionStatus = "SUCCESS",
                        WalletId = Wallet.WalletId,
                        DepositAmount = TotalMoney,
                        OrderId = "#buyticket"
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
                        UserId = Wallet.WalletId,
                        TotalAmount = TotalMoney,
                        TotalTicket = Quantity
                    };

                    var wallet = await _walletRepository.GetByIdAsync(Wallet.WalletId);
                    wallet.Balance = Balance - TotalMoney;

                    booking.Tickets.Add(ticket);
                    await _ticketRepository.AddAsync(ticket);
                    await _bookingRepository.AddAsync(booking);
                    await _walletRepository.UpdateAsync(wallet);
                    await _transactionRepository.AddAsync(transaction);
                    await _eventRepository.UpdateAsync(eventUpdate);

                    var user = await _userRepository.GetByIdAsync(Wallet.WalletId);
                    if (user != null)
                    {
                        SendTicketEmail(user.Email, ticket, eventUpdate, Quantity, TotalMoney); // Pass the `eventUpdate` object
                    }

                    return RedirectToPage("/Events/SuccessPay", new { booking.CreatedDate, booking.BookingId });
                }
            }
            catch (Exception ex)
            {
                return RedirectToPage("/Events/FailedPay", new { ErrorMessage = ex.Message, CreatedDate = DateTime.Now });
            }
            return Page();
        }

        private void SendTicketEmail(string email, Ticket ticket, Event @event, int quantity, decimal totalAmount)
        {
            var myEmail = "seminarwebapp@gmail.com";
            var myPassword = "mbyghvpzorxaihmp";

            var message = new MailMessage
            {
                From = new MailAddress(myEmail),
                Subject = "[PRN221] Your Event Ticket",
                IsBodyHtml = true,
                Body = $@"
                <html>
                <body>
                    <h2>Thank you for your purchase!</h2>
                    <p>Here are your ticket details:</p>
                    <ul>
                        <li><strong>Ticket ID:</strong> {ticket.TicketId}</li>
                        <li><strong>Event Name:</strong> {@event.EventName}</li>
                        <li><strong>Event Code:</strong> {@event.EventCode}</li>
                        <li><strong>Price per Ticket:</strong> {ticket.Price}</li>
                        <li><strong>Quantity:</strong> {quantity}</li>
                        <li><strong>Total Amount:</strong> {totalAmount}</li>
                        <li><strong>Event Location:</strong> {@event.Hall?.HallName}</li>
                    </ul>
                    <p>Attached is your ticket with a QR code.</p>
                    <p>Best regards,<br>PRN221 Team</p>
                </body>
                </html>"
            };

            message.To.Add(new MailAddress(email));

            var qrCodeImage = GenerateQrCode(ticket, @event, quantity, totalAmount);
            var imageAttachment = new Attachment(new MemoryStream(qrCodeImage), "ticket_qr_code.png", "image/png");
            message.Attachments.Add(imageAttachment);

            using var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(myEmail, myPassword),
                EnableSsl = true
            };

            smtpClient.Send(message);
        }

        private byte[] GenerateQrCode(Ticket ticket, Event @event, int quantity, decimal totalAmount)
        {
            var qrCodeText = $"Ticket ID: {ticket.TicketId}\n" +
                             $"Event Name: {@event.EventName}\n" +
                             $"Event Code: {@event.EventCode}\n" +
                             $"Price per Ticket: {ticket.Price}\n" +
                             $"Quantity: {quantity}\n" +
                             $"Total Amount: {totalAmount}\n" +
                             $"Event Location: {@event.Hall?.HallName}";

            using var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(qrCodeText, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new QRCode(qrCodeData);
            using var qrCodeImage = qrCode.GetGraphic(20);

            using var ms = new MemoryStream();
            qrCodeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
        }
    }
}
