using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
    public class TransactionDto
    {
        public Guid TransactionId { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public decimal? DepositAmount { get; set; }

        public string? TransactionStatus { get; set; }

        public Guid? UserId { get; set; }

        public Guid? WalletId { get; set; }

        public UserDto? User { get; set; }
        public WalletDto? Wallet { get; set; }
    }
}
