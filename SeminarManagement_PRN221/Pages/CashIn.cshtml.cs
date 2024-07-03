using BusinessObject.DTO;
using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories;
using Repositories.Interfaces;

namespace SeminarManagement_PRN221.Pages
{
    public class CashInModel : PageModel
    {
        private readonly IVnPayRepository _vnpayRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly ITransactionRepository _transactionRepository;

        public CashInModel(IVnPayRepository vnpayRepository, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, IWalletRepository walletRepository, ITransactionRepository transactionRepository)
        {
            _vnpayRepository = vnpayRepository;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _walletRepository = walletRepository;
            _transactionRepository = transactionRepository;
        }

        [BindProperty]
        public double Amount { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var orderId = new Random().Next(1000000000, int.MaxValue).ToString();

            var model = new VnPaymentRequestModel
            {
                OrderId = orderId, 
                FullName = User.Identity.Name,
                Description = "Cash in wallet",
                Amount = Amount,
                CreatedDate = DateTime.Now
            };

            var paymentUrl = _vnpayRepository.CreatePaymentUrl(_httpContextAccessor.HttpContext, model);

            var user = await _userRepository.GetUserByUsername(User.Identity.Name);
            var wallet = await _walletRepository.GetByIdAsync(user.UserId);

            var transaction = new Transaction
            {
                TransactionId = Guid.NewGuid(),
                OrderId = orderId,
                CreationDate = DateTime.Now,
                DepositAmount = (decimal)model.Amount,
                TransactionStatus = "PENDING",
                WalletId = wallet.WalletId
            };

            await _transactionRepository.AddAsync(transaction);

            return Redirect(paymentUrl);
        }
    }
}
