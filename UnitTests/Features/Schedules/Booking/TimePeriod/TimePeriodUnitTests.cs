namespace UnitTests.Features.Schedules.Booking.TimePeriod;

public class TimePeriodUnitTests
{
    [Fact]
    public void Create_WithUtcStartAndUtcEnd_AndStartBeforeEnd_ReturnsInstance()
    {
        var start = new DateTime(2026, 1, 1, 10, 0, 0, DateTimeKind.Utc);
        var end = start.AddHours(1);

        var result = DCA_Padel_Club.Core.Domain.Aggregates.Schedules.TimePeriod.Create(start, end);

        Assert.False(result.IsFailure);
        Assert.NotNull(result.value);
    }

    [Fact]
    public void Create_WithStartLocalTime_ReturnsFailure()
    {
        var start = new DateTime(2026, 1, 1, 10, 0, 0, DateTimeKind.Local);
        var end = new DateTime(2026, 1, 1, 11, 0, 0, DateTimeKind.Utc);

        var result = DCA_Padel_Club.Core.Domain.Aggregates.Schedules.TimePeriod.Create(start, end);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "TimePeriod.Start.NotUtc");
    }

    [Fact]
    public void Create_WithEndLocalTime_ReturnsFailure()
    {
        var start = new DateTime(2026, 1, 1, 10, 0, 0, DateTimeKind.Utc);
        var end = new DateTime(2026, 1, 1, 11, 0, 0, DateTimeKind.Local);

        var result = DCA_Padel_Club.Core.Domain.Aggregates.Schedules.TimePeriod.Create(start, end);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "TimePeriod.End.NotUtc");
    }

    [Fact]
    public void Create_WithStartEqualsEnd_ReturnsFailure()
    {
        var start = new DateTime(2026, 1, 1, 10, 0, 0, DateTimeKind.Utc);
        var end = start;

        var result = DCA_Padel_Club.Core.Domain.Aggregates.Schedules.TimePeriod.Create(start, end);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "TimePeriod.Range.Invalid");
    }

    [Fact]
    public void Create_WithStartAfterEnd_ReturnsFailure()
    {
        var start = new DateTime(2026, 1, 1, 11, 0, 0, DateTimeKind.Utc);
        var end = new DateTime(2026, 1, 1, 10, 0, 0, DateTimeKind.Utc);

        var result = DCA_Padel_Club.Core.Domain.Aggregates.Schedules.TimePeriod.Create(start, end);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "TimePeriod.Range.Invalid");
    }

    [Fact]
    public void Create_WithOneTickDifference_AllowsBoundary()
    {
        var start = new DateTime(2026, 1, 1, 10, 0, 0, DateTimeKind.Utc);
        var end = start.AddTicks(1);

        var result = DCA_Padel_Club.Core.Domain.Aggregates.Schedules.TimePeriod.Create(start, end);

        Assert.False(result.IsFailure);
        Assert.NotNull(result.value);
    }

    [Fact]
    public void Equals_WithSameStartAndEnd_ReturnsTrue()
    {
        var start = new DateTime(2026, 1, 1, 10, 0, 0, DateTimeKind.Utc);
        var end = start.AddHours(1);
        var leftResult = DCA_Padel_Club.Core.Domain.Aggregates.Schedules.TimePeriod.Create(start, end);
        var rightResult = DCA_Padel_Club.Core.Domain.Aggregates.Schedules.TimePeriod.Create(start, end);

        Assert.False(leftResult.IsFailure);
        Assert.False(rightResult.IsFailure);

        var left = leftResult.value;
        var right = rightResult.value;

        Assert.Equal(left, right);
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }

    [Fact]
    public void Equals_WithDifferentStartOrEnd_ReturnsFalse()
    {
        var start = new DateTime(2026, 1, 1, 10, 0, 0, DateTimeKind.Utc);
        var end = start.AddHours(1);
        var leftResult = DCA_Padel_Club.Core.Domain.Aggregates.Schedules.TimePeriod.Create(start, end);
        var rightResult = DCA_Padel_Club.Core.Domain.Aggregates.Schedules.TimePeriod.Create(start.AddMinutes(1), end.AddMinutes(1));

        Assert.False(leftResult.IsFailure);
        Assert.False(rightResult.IsFailure);

        var left = leftResult.value;
        var right = rightResult.value;

        Assert.NotEqual(left, right);
    }
}
