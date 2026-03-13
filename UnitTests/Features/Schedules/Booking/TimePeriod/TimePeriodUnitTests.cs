using TP = DCA_Padel_Club.Core.Domain.Aggregates.Schedules.TimePeriod;

namespace UnitTests.Features.Schedules.Booking.TimePeriod;

public class TimePeriodUnitTests
{
    [Fact]
    public void Create_WithUtcStartAndUtcEnd_AndStartBeforeEnd_ReturnsInstance()
    {
        var start = new DateTime(2026, 1, 1, 10, 0, 0, DateTimeKind.Utc);
        var end = start.AddHours(1);

        var result = TP.Create(start, end);

        Assert.False(result.IsFailure);
        Assert.NotNull(result.value);
    }

    [Fact]
    public void Create_WithStartLocalTime_ReturnsFailure()
    {
        var start = new DateTime(2026, 1, 1, 10, 0, 0, DateTimeKind.Local);
        var end = new DateTime(2026, 1, 1, 11, 0, 0, DateTimeKind.Utc);

        var result = TP.Create(start, end);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "TimePeriod.Start.NotUtc");
    }

    [Fact]
    public void Create_WithEndLocalTime_ReturnsFailure()
    {
        var start = new DateTime(2026, 1, 1, 10, 0, 0, DateTimeKind.Utc);
        var end = new DateTime(2026, 1, 1, 11, 0, 0, DateTimeKind.Local);

        var result = TP.Create(start, end);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "TimePeriod.End.NotUtc");
    }

    [Fact]
    public void Create_WithStartEqualsEnd_ReturnsFailure()
    {
        var start = new DateTime(2026, 1, 1, 10, 0, 0, DateTimeKind.Utc);
        var end = start;

        var result = TP.Create(start, end);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "TimePeriod.Range.Invalid");
    }

    [Fact]
    public void Create_WithStartAfterEnd_ReturnsFailure()
    {
        var start = new DateTime(2026, 1, 1, 11, 0, 0, DateTimeKind.Utc);
        var end = new DateTime(2026, 1, 1, 10, 0, 0, DateTimeKind.Utc);

        var result = TP.Create(start, end);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "TimePeriod.Range.Invalid");
    }


    [Theory]
    [InlineData(1)]
    [InlineData(15)]
    [InlineData(29)]
    [InlineData(31)]
    [InlineData(45)]
    [InlineData(59)]
    public void Create_WhenStartMinutesAreNot00Or30_ReturnsFailure(int minutes)
    {
        var start = new DateTime(2026, 1, 1, 14, minutes, 0, DateTimeKind.Utc);
        var end = new DateTime(2026, 1, 1, 15, 0, 0, DateTimeKind.Utc);

        var result = TP.Create(start, end);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "TimePeriod.Start.InvalidMinutes");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(15)]
    [InlineData(29)]
    [InlineData(31)]
    [InlineData(45)]
    [InlineData(59)]
    public void Create_WhenEndMinutesAreNot00Or30_ReturnsFailure(int minutes)
    {
        var start = new DateTime(2026, 1, 1, 14, 0, 0, DateTimeKind.Utc);
        var end = new DateTime(2026, 1, 1, 15, minutes, 0, DateTimeKind.Utc);

        var result = TP.Create(start, end);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "TimePeriod.End.InvalidMinutes");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(30)]
    public void Create_WhenBothMinutesAre00Or30_ReturnsSuccess(int startMinutes)
    {
        var start = new DateTime(2026, 1, 1, 14, startMinutes, 0, DateTimeKind.Utc);
        var end = start.AddHours(1);

        var result = TP.Create(start, end);

        Assert.False(result.IsFailure);
    }

    [Theory]
    [InlineData(30)]
    public void Create_WhenDurationIsLessThan1Hour_ReturnsFailure(int durationMinutes)
    {
        var start = new DateTime(2026, 1, 1, 14, 0, 0, DateTimeKind.Utc);
        var end = start.AddMinutes(durationMinutes);

        var result = TP.Create(start, end);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "TimePeriod.Duration.TooShort");
    }

    [Theory]
    [InlineData(210)]
    [InlineData(240)]
    public void Create_WhenDurationIsMoreThan3Hours_ReturnsFailure(int durationMinutes)
    {
        var start = new DateTime(2026, 1, 1, 14, 0, 0, DateTimeKind.Utc);
        var end = start.AddMinutes(durationMinutes);

        var result = TP.Create(start, end);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "TimePeriod.Duration.TooLong");
    }

    [Theory]
    [InlineData(60)]
    [InlineData(90)]
    [InlineData(120)]
    [InlineData(150)]
    [InlineData(180)]
    public void Create_WhenDurationIsBetween1And3Hours_ReturnsSuccess(int durationMinutes)
    {
        var start = new DateTime(2026, 1, 1, 14, 0, 0, DateTimeKind.Utc);
        var end = start.AddMinutes(durationMinutes);

        var result = TP.Create(start, end);

        Assert.False(result.IsFailure);
    }

    [Fact]
    public void Equals_WithSameStartAndEnd_ReturnsTrue()
    {
        var start = new DateTime(2026, 1, 1, 10, 0, 0, DateTimeKind.Utc);
        var end = start.AddHours(1);
        var leftResult = TP.Create(start, end);
        var rightResult = TP.Create(start, end);

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
        var leftResult = TP.Create(start, end);
        var rightResult = TP.Create(start.AddMinutes(30), end.AddMinutes(30));

        Assert.False(leftResult.IsFailure);
        Assert.False(rightResult.IsFailure);

        var left = leftResult.value;
        var right = rightResult.value;

        Assert.NotEqual(left, right);
    }
}
