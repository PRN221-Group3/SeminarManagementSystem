using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
    public class WalletDto
    {
        public Guid WalletId { get; set; }

        public decimal? Balance { get; set; }

        public ICollection<TransactionDto> Transactions { get; set; }
    }
}
