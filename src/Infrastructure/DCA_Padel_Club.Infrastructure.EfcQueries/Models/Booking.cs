using System;
using System.Collections.Generic;

namespace DCA_Padel_Club.Infrastructure.EfcQueries.Models;

public partial class Booking
{
    public string BookingId { get; set; } = null!;

    public string ScheduleId { get; set; } = null!;

    public int BookerId { get; set; }

    public string CourtNumber { get; set; } = null!;

    public string SlotDate { get; set; } = null!;

    public string SlotStartTime { get; set; } = null!;

    public string SlotEndTime { get; set; } = null!;

    public virtual Schedule Schedule { get; set; } = null!;

    public virtual ICollection<Player> Players { get; set; } = new List<Player>();
}
