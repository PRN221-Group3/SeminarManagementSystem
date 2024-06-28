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
        public WalletModel(IWalletRepository walletRepository, IUserRepository userRepository)
        {
            _walletRepository = walletRepository;
            _userRepository = userRepository;

        }
        public Wallet Wallet { get; set; }
        public async Task<IActionResult> OnGet(string username)
        {
            var user = await _userRepository.GetUserByUsername(username);
            Wallet = await _walletRepository.GetByIdAsync(user.UserId);
            if(Wallet == null)
            {
                Wallet = new()
                {
                    Balance = 0,
                    WalletId = user.UserId
                };
                await _walletRepository.AddAsync(Wallet);
            }
            return Page();
        }
    }
}
