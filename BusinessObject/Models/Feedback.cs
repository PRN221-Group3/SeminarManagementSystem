using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class Feedback
{
    public Guid FeedbackId { get; set; }

    public string? FeedBackContent { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? FinishDate { get; set; }

    public Guid? EventId { get; set; }

    public Guid? UserId { get; set; }

    public virtual Event? Event { get; set; }

    public virtual User? User { get; set; }
}
