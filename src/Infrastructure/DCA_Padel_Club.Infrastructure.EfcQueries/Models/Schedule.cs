using System;
using System.Collections.Generic;

namespace DCA_Padel_Club.Infrastructure.EfcQueries.Models;

public partial class Schedule
{
    public string Id { get; set; } = null!;

    public string Date { get; set; } = null!;

    public string StartTime { get; set; } = null!;

    public string EndTime { get; set; } = null!;

    public int IsDraft { get; set; }

    public int IsDeleted { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<ScheduleCourt> ScheduleCourts { get; set; } = new List<ScheduleCourt>();
}
