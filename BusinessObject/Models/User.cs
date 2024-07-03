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

    public string? Password { get; set; }

    public Guid? RoleId { get; set; }

    public bool? IsDeleted { get; set; }

    public bool? IsActivated { get; set; }

    public string? VerifyToken { get; set; }

    public DateTime? IssueTokenDate { get; set; }

    public string? QrCode { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual Role? Role { get; set; }

    public virtual Sponsor? Sponsor { get; set; }

    public virtual Wallet? Wallet { get; set; }
}
