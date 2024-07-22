using System;
using System.Threading.Tasks;
using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using Repositories.Interfaces;

namespace SeminarManagement_PRN221.Pages
{
    public class EventDetailsModel : PageModel
    {
        private readonly IEventRepository _eventRepository;

        public EventDetailsModel(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        [BindProperty(SupportsGet = true)]
        public Guid EventId { get; set; }

        public Event Event { get; set; }
        public EventSponsor Sponsor { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var allEvents = await _eventRepository.GetAllQueryableAsync();
            Event = allEvents.Include(s => s.Hall).Include(s => s.EventSponsors).FirstOrDefault(s => s.EventId == EventId);

            if (Event == null)
            {
                return NotFound();
            }

            GenerateQRCode();

            return Page();
        }

        private void GenerateQRCode()
        {
            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode($"Event: {Event.EventName}\nStart Date: {Event.StartDate}\nEnd Date: {Event.EndDate}", QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCode(qrCodeData);
            var qrCodeImage = qrCode.GetGraphic(20);

            using (var ms = new System.IO.MemoryStream())
            {
                qrCodeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                var qrCodeBase64 = Convert.ToBase64String(ms.ToArray());
                Event.QrCode = $"data:image/png;base64,{qrCodeBase64}";
            }
        }
    }
}