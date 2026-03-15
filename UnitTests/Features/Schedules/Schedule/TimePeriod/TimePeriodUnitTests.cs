using TP = DCA_Padel_Club.Core.Domain.Aggregates.Schedules.TimePeriod;

namespace UnitTests.Features.Schedules.Schedule.TimePeriod;

public class TimePeriodUnitTests
{
    private static readonly DateTime BaseDate = new(2026, 1, 15, 0, 0, 0, DateTimeKind.Utc);

    [Fact]
    public void Create_When30MinuteSpan_ReturnsSuccess()
    {
        var start = BaseDate.AddHours(12);
        var end = start.AddMinutes(30);

        var result = TP.Create(start, end);

        Assert.False(result.IsFailure);
    }

    [Fact]
    public void Create_When2HourSpan_ReturnsSuccess()
    {
        var start = BaseDate.AddHours(12);
        var end = start.AddHours(2);

        var result = TP.Create(start, end);

        Assert.False(result.IsFailure);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(15)]
    [InlineData(59)]
    public void Create_WhenStartMinutesNot00Or30_ReturnsFailure(int minutes)
    {
        var start = BaseDate.AddHours(12).AddMinutes(minutes);
        var end = start.AddHours(1);

        var result = TP.Create(start, end);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "TimePeriod.Start.InvalidMinutes");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(59)]
    public void Create_WhenEndMinutesNot00Or30_ReturnsFailure(int minutes)
    {
        var start = BaseDate.AddHours(12);
        var end = BaseDate.AddHours(13).AddMinutes(minutes);

        var result = TP.Create(start, end);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "TimePeriod.End.InvalidMinutes");
    }

    [Fact]
    public void Create_WhenStartEqualsEnd_ReturnsFailure()
    {
        var start = BaseDate.AddHours(12);

        var result = TP.Create(start, start);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "TimePeriod.Range.Invalid");
    }

    [Fact]
    public void Create_WhenStartAfterEnd_ReturnsFailure()
    {
        var start = BaseDate.AddHours(14);
        var end = BaseDate.AddHours(12);

        var result = TP.Create(start, end);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "TimePeriod.Range.Invalid");
    }

    [Fact]
    public void Equals_WithSameTimes_ReturnsTrue()
    {
        var start = BaseDate.AddHours(12);
        var end = start.AddHours(2);
        var left = TP.Create(start, end).value;
        var right = TP.Create(start, end).value;

        Assert.Equal(left, right);
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }

    [Fact]
    public void Equals_WithDifferentTimes_ReturnsFalse()
    {
        var start = BaseDate.AddHours(12);
        var end = start.AddHours(2);
        var left = TP.Create(start, end).value;
        var right = TP.Create(start.AddMinutes(30), end.AddMinutes(30)).value;

        Assert.NotEqual(left, right);
    }
}
