using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories.Interfaces;

namespace SeminarManagement_PRN221.Pages
{
    public class PaymentCallbackModel : PageModel
    {
        private readonly IVnPayRepository _vnpayRepository;
        private readonly IUserRepository _userRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PaymentCallbackModel(IVnPayRepository vnpayRepository, IUserRepository userRepository, IWalletRepository walletRepository, ITransactionRepository transactionRepository, IHttpContextAccessor httpContextAccessor)
        {
            _vnpayRepository = vnpayRepository;
            _userRepository = userRepository;
            _walletRepository = walletRepository;
            _transactionRepository = transactionRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public string Message { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var query = _httpContextAccessor.HttpContext.Request.Query;
            var paymentResponse = _vnpayRepository.PaymentExecute(query);

            var vnpTxnRef = query["vnp_TxnRef"].ToString();

            var transaction = await _transactionRepository.GetFirstOrDefaultAsync(t => t.TransactionStatus == "PENDING" && t.OrderId == vnpTxnRef);

            if (transaction == null)
            {
                TempData["Message"] = "No pending transactions found";
                return RedirectToPage("/PaymentError");
            }

            var wallet = await _walletRepository.GetByIdAsync(transaction.WalletId ?? Guid.Empty);

            switch (paymentResponse.VnPayResponseCode)
            {
                case "00":
                    wallet.Balance = (wallet.Balance ?? 0) + (decimal)paymentResponse.Amount;
                    await _walletRepository.UpdateAsync(wallet);

                    transaction.DepositAmount = (decimal)paymentResponse.Amount;
                    transaction.TransactionStatus = "SUCCESS";
                    transaction.CreationDate = DateTime.Now; 
                    transaction.UpdateDate = DateTime.Now;
                    await _transactionRepository.UpdateAsync(transaction);

                    TempData["Message"] = "Deposit successful!";
                    return RedirectToPage("/PaymentSuccess");

                default:
                    transaction.TransactionStatus = "FAILED";
                    transaction.CreationDate = DateTime.Now;
                    transaction.UpdateDate = DateTime.Now;
                    await _transactionRepository.UpdateAsync(transaction);

                    TempData["Message"] = GetMessageFromResponseCode(paymentResponse.VnPayResponseCode);
                    return RedirectToPage("/PaymentError");
            }
        }

        private string GetMessageFromResponseCode(string responseCode)
        {
            return responseCode switch
            {
                "00" => "Giao dịch thành công",
                "01" => "Giao dịch chưa hoàn tất",
                "02" => "Giao dịch bị lỗi",
                "04" => "Giao dịch đã được thực hiện nhưng chưa thành công",
                "05" => "VNPay đang xử lý giao dịch này",
                "06" => "VNPay đã gửi yêu cầu hoàn tiền",
                "07" => "Giao dịch bị nghi ngờ gian lận",
                "09" => "Giao dịch hoàn trả bị từ chối",
                "10" => "Khách hàng xác thực thông tin thẻ/tài khoản không đúng quá 3 lần",
                "11" => "Đã hết hạn chờ thanh toán",
                "12" => "Thẻ/Tài khoản của khách hàng bị khóa",
                "13" => "Khách nhập sai mật khẩu xác thực giao dịch",
                "24" => "Khách hàng hủy giao dịch",
                "51" => "Tài khoản của quý khách không đủ số dư để thực hiện giao dịch",
                "65" => "Tài khoản của quý khách đã vượt quá hạn mức giao dịch trong ngày",
                "75" => "Ngân hàng thanh toán đang bảo trì",
                "79" => "KH nhập sai mật khẩu thanh toán quá số lần quy định",
                "99" => "Lỗi khác",
                _ => "Lỗi không xác định"
            };
        }
    }
}

