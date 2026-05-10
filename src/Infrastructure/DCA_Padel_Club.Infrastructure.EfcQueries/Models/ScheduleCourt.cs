using System;
using System.Collections.Generic;

namespace DCA_Padel_Club.Infrastructure.EfcQueries.Models;

public partial class ScheduleCourt
{
    public string CourtNumber { get; set; } = null!;

    public string ScheduleId { get; set; } = null!;

    public int IsOccupied { get; set; }

    public virtual Schedule Schedule { get; set; } = null!;
}
