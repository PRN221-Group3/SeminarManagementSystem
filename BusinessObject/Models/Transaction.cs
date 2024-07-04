using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class Transaction
{
    public Guid TransactionId { get; set; }

    public DateTime? CreationDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public decimal? DepositAmount { get; set; }

    public string? TransactionStatus { get; set; }

    public string? OrderId { get; set; }

    public Guid? WalletId { get; set; }

    public virtual Wallet? Wallet { get; set; }
}
