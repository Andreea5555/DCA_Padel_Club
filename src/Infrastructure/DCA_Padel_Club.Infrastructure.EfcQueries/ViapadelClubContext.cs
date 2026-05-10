using System;
using System.Collections.Generic;
using DCA_Padel_Club.Infrastructure.EfcQueries.Models;
using Microsoft.EntityFrameworkCore;

namespace DCA_Padel_Club.Infrastructure.EfcQueries;

public partial class ViapadelClubContext : DbContext
{
    public ViapadelClubContext()
    {
    }

    public ViapadelClubContext(DbContextOptions<ViapadelClubContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Player> Players { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<ScheduleCourt> ScheduleCourts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=/Users/maciej/VIA/6thSem/DCA/DCA_assignment/DCA_Padel_Club/src/Infrastructure/DCA_Padel_Club.Infrastructure.EfcDomainModelPersistence/VIAPadelClub.db");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasIndex(e => e.ScheduleId, "IX_Bookings_ScheduleId");

            entity.HasOne(d => d.Schedule).WithMany(p => p.Bookings).HasForeignKey(d => d.ScheduleId);

            entity.HasMany(d => d.Players).WithMany(p => p.Bookings)
                .UsingEntity<Dictionary<string, object>>(
                    "BookingPlayer",
                    r => r.HasOne<Player>().WithMany().HasForeignKey("PlayerId"),
                    l => l.HasOne<Booking>().WithMany().HasForeignKey("BookingId"),
                    j =>
                    {
                        j.HasKey("BookingId", "PlayerId");
                        j.ToTable("BookingPlayers");
                        j.HasIndex(new[] { "PlayerId" }, "IX_BookingPlayers_PlayerId");
                    });
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<ScheduleCourt>(entity =>
        {
            entity.HasKey(e => new { e.ScheduleId, e.CourtNumber });

            entity.HasOne(d => d.Schedule).WithMany(p => p.ScheduleCourts).HasForeignKey(d => d.ScheduleId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
