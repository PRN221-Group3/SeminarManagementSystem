﻿using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class Event
{
    public Guid EventId { get; set; }

    public DateTime? CreationDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public string? EventName { get; set; }

    public string? EventCode { get; set; }

    public DateTime? StartDate { get; set; }

    public string? Description { get; set; }

    public DateTime? EndDate { get; set; }

    public string? QrCode { get; set; }

    public bool? IsDeleted { get; set; }

    public decimal? Fee { get; set; }

    public Guid? HallId { get; set; }

    public int? NumberOfTickets { get; set; }

    public bool? IsFeedbackOpen { get; set; }

    public virtual ICollection<EventSponsor> EventSponsors { get; set; } = new List<EventSponsor>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual Hall? Hall { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
