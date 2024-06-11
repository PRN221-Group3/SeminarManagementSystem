using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class User
{
    public Guid UserId { get; set; }

    public string? LastName { get; set; }

    public string? FirstName { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Username { get; set; }

    public Guid? RoleId { get; set; }

    public bool? IsDeleted { get; set; }

    public string? QrCode { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
