using System;
using System.Collections.Generic;

namespace SeminarManagement_PRN221.Models;

public partial class Survey
{
    public Guid SurveyId { get; set; }

    public string? FeedBackContent { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? FinishDate { get; set; }

    public Guid? EventId { get; set; }

    public virtual Event? Event { get; set; }
}
