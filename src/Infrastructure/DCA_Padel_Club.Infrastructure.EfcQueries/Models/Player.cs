using System;
using System.Collections.Generic;

namespace DCA_Padel_Club.Infrastructure.EfcQueries.Models;

public partial class Player
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string ProfilePicture { get; set; } = null!;

    public int Blacklisted { get; set; }

    public string? CooldownExpiresAt { get; set; }

    public string? QuarantineEndDate { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
