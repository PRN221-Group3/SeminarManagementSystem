using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories.Interfaces;

namespace SeminarManagement_PRN221.Pages.Account
{
    public class WalletModel : PageModel
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITransactionRepository _transactionRepository;
        public WalletModel(IWalletRepository walletRepository, IUserRepository userRepository, ITransactionRepository transactionRepository)
        {
            _walletRepository = walletRepository;
            _userRepository = userRepository;
            _transactionRepository = transactionRepository;
        }

        //public bool IsDialogVisible { get; set; }
        //public bool DontRemindMeAgain { get; set; }
        public Wallet Wallet { get; set; }
        public List<Transaction> Transactions { get; set; }

        public async Task<IActionResult> OnGet(string username)
        {
            var user = await _userRepository.GetUserByUsername(username);
            Wallet = await _walletRepository.GetByIdAsync(user.UserId);
            if (Wallet == null)
            {
                Wallet = new()
                {
                    Balance = 0,
                    WalletId = user.UserId
                };
                await _walletRepository.AddAsync(Wallet);
            }
            Transactions = (await _transactionRepository.GetTransactionsByWalletIdAsync(Wallet.WalletId)).ToList();

            //IsDialogVisible = !Request.Cookies.TryGetValue("dontRemindMeAgain", out var dontRemindMeAgain) || dontRemindMeAgain != "true";
            return Page();
        }
    }
}
