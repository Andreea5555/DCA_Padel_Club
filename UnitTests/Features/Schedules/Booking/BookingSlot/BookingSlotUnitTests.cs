using BookingSlotType = DCA_Padel_Club.Core.Domain.Aggregates.Schedules.BookingSlot;

namespace UnitTests.Features.Schedules.Booking.BookingSlot;

public class BookingSlotUnitTests
{
    private static readonly DateOnly TestDate = new(2026, 1, 15);

    [Fact]
    public void Create_WithValidData_ReturnsSuccess()
    {
        var start = new TimeOnly(14, 0);
        var end = new TimeOnly(16, 0);

        var result = BookingSlotType.Create(TestDate, start, end);

        Assert.False(result.IsFailure);
        Assert.NotNull(result.value);
    }

    [Fact]
    public void Create_WhenStartEqualsEnd_ReturnsFailure()
    {
        var time = new TimeOnly(14, 0);
        var result = BookingSlotType.Create(TestDate, time, time);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "BookingSlot.Range.Invalid");
    }

    [Fact]
    public void Create_WhenStartAfterEnd_ReturnsFailure()
    {
        var start = new TimeOnly(16, 0);
        var end = new TimeOnly(14, 0);
        var result = BookingSlotType.Create(TestDate, start, end);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "BookingSlot.Range.Invalid");
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
        var start = new TimeOnly(14, minutes);
        var end = new TimeOnly(15, 0);
        var result = BookingSlotType.Create(TestDate, start, end);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "BookingSlot.Start.InvalidMinutes");
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
        var start = new TimeOnly(14, 0);
        var end = new TimeOnly(15, minutes);
        var result = BookingSlotType.Create(TestDate, start, end);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "BookingSlot.End.InvalidMinutes");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(30)]
    public void Create_WhenBothMinutesAre00Or30_ReturnsSuccess(int startMinutes)
    {
        var start = new TimeOnly(14, startMinutes);
        var end = new TimeOnly(15, 30);
        var result = BookingSlotType.Create(TestDate, start, end);

        Assert.False(result.IsFailure);
    }

    [Theory]
    [InlineData(30)]
    public void Create_WhenDurationIsLessThan1Hour_ReturnsFailure(int durationMinutes)
    {
        var start = new TimeOnly(14, 0);
        var end = start.Add(TimeSpan.FromMinutes(durationMinutes));
        var result = BookingSlotType.Create(TestDate, start, end);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "BookingSlot.Duration.TooShort");
    }

    [Theory]
    [InlineData(210)]
    [InlineData(240)]
    public void Create_WhenDurationIsMoreThan3Hours_ReturnsFailure(int durationMinutes)
    {
        var start = new TimeOnly(14, 0);
        var end = start.Add(TimeSpan.FromMinutes(durationMinutes));
        var result = BookingSlotType.Create(TestDate, start, end);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "BookingSlot.Duration.TooLong");
    }

    [Theory]
    [InlineData(60)]
    [InlineData(90)]
    [InlineData(120)]
    [InlineData(150)]
    [InlineData(180)]
    public void Create_WhenDurationIsBetween1And3Hours_ReturnsSuccess(int durationMinutes)
    {
        var start = new TimeOnly(14, 0);
        var end = start.Add(TimeSpan.FromMinutes(durationMinutes));
        var result = BookingSlotType.Create(TestDate, start, end);

        Assert.False(result.IsFailure);
    }

    [Fact]
    public void ValidateFitsWithin_WhenSlotWithinWindow_ReturnsNoErrors()
    {
        var slot = CreateSlot(14, 0, 16, 0);
        var scheduleDate = TestDate;
        var scheduleStart = new TimeOnly(9, 0);
        var scheduleEnd = new TimeOnly(21, 0);

        var errors = slot.ValidateFitsWithin(scheduleDate, scheduleStart, scheduleEnd);

        Assert.Empty(errors);
    }

    [Fact]
    public void ValidateFitsWithin_WhenDateDiffers_ReturnsError()
    {
        var slot = CreateSlot(14, 0, 16, 0);
        var wrongDate = TestDate.AddDays(1);
        var scheduleStart = new TimeOnly(9, 0);
        var scheduleEnd = new TimeOnly(21, 0);

        var errors = slot.ValidateFitsWithin(wrongDate, scheduleStart, scheduleEnd);

        Assert.NotEmpty(errors);
        Assert.Contains(errors, e => e.ErrorCode == "Schedule.SlotOutOfBounds");
    }

    [Fact]
    public void ValidateFitsWithin_WhenStartBeforeScheduleStart_ReturnsError()
    {
        var slot = CreateSlot(8, 0, 10, 0);
        var scheduleStart = new TimeOnly(9, 0);
        var scheduleEnd = new TimeOnly(21, 0);

        var errors = slot.ValidateFitsWithin(TestDate, scheduleStart, scheduleEnd);

        Assert.NotEmpty(errors);
        Assert.Contains(errors, e => e.ErrorCode == "Schedule.SlotOutOfBounds");
    }

    [Fact]
    public void ValidateFitsWithin_WhenEndAfterScheduleEnd_ReturnsError()
    {
        var slot = CreateSlot(20, 0, 22, 0);
        var scheduleStart = new TimeOnly(9, 0);
        var scheduleEnd = new TimeOnly(21, 0);

        var errors = slot.ValidateFitsWithin(TestDate, scheduleStart, scheduleEnd);

        Assert.NotEmpty(errors);
        Assert.Contains(errors, e => e.ErrorCode == "Schedule.SlotOutOfBounds");
    }

    [Fact]
    public void ValidateFitsWithin_WhenSlotAtWindowBoundaries_ReturnsNoErrors()
    {
        var slot = CreateSlot(9, 0, 10, 0);
        var scheduleStart = new TimeOnly(9, 0);
        var scheduleEnd = new TimeOnly(21, 0);

        var errors = slot.ValidateFitsWithin(TestDate, scheduleStart, scheduleEnd);

        Assert.Empty(errors);
    }

    [Fact]
    public void Overlaps_WhenSameDateAndOverlapping_ReturnsTrue()
    {
        var slotA = CreateSlot(14, 0, 16, 0);
        var slotB = CreateSlot(15, 0, 17, 0);

        Assert.True(slotA.Overlaps(slotB));
        Assert.True(slotB.Overlaps(slotA));
    }

    [Fact]
    public void Overlaps_WhenSameDateAndIdentical_ReturnsTrue()
    {
        var slotA = CreateSlot(14, 0, 16, 0);
        var slotB = CreateSlot(14, 0, 16, 0);

        Assert.True(slotA.Overlaps(slotB));
    }

    [Fact]
    public void Overlaps_WhenSameDateAndAdjacent_ReturnsFalse()
    {
        var slotA = CreateSlot(14, 0, 16, 0);
        var slotB = CreateSlot(16, 0, 18, 0);

        Assert.False(slotA.Overlaps(slotB));
        Assert.False(slotB.Overlaps(slotA));
    }

    [Fact]
    public void Overlaps_WhenSameDateAndNonOverlapping_ReturnsFalse()
    {
        var slotA = CreateSlot(14, 0, 16, 0);
        var slotB = CreateSlot(17, 0, 19, 0);

        Assert.False(slotA.Overlaps(slotB));
        Assert.False(slotB.Overlaps(slotA));
    }

    [Fact]
    public void Overlaps_WhenDifferentDate_ReturnsFalse()
    {
        var slotA = CreateSlot(14, 0, 16, 0);
        var slotB = BookingSlotType.Create(TestDate.AddDays(1), new TimeOnly(14, 0), new TimeOnly(16, 0)).value;

        Assert.False(slotA.Overlaps(slotB));
    }

    [Fact]
    public void Equals_WithSameDateAndTimes_ReturnsTrue()
    {
        var left = CreateSlot(10, 0, 11, 0);
        var right = CreateSlot(10, 0, 11, 0);

        Assert.Equal(left, right);
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }

    [Fact]
    public void Equals_WithDifferentDateOrTimes_ReturnsFalse()
    {
        var left = CreateSlot(10, 0, 11, 0);
        var right = CreateSlot(10, 30, 11, 30);

        Assert.NotEqual(left, right);
    }

    private static BookingSlotType CreateSlot(int startHour, int startMin, int endHour, int endMin)
    {
        var result = BookingSlotType.Create(
            TestDate,
            new TimeOnly(startHour, startMin),
            new TimeOnly(endHour, endMin));
        Assert.False(result.IsFailure);
        return result.value;
    }
}
