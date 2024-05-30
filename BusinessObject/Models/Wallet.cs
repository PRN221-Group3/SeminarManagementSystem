using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class Wallet
{
    public Guid WalletId { get; set; }

    public decimal? Balance { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
