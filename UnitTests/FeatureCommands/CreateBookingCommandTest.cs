using DCA_Padel_Club.Core.Application.Commands.ScheduleCommands;
using Xunit;

namespace UnitTests.FeatureCommands;

public class CreateBookingCommandTest
{
    [Fact]
    public void Create_ValidInput_ReturnsSuccess()
    {
        
        string scheduleId = Guid.NewGuid().ToString();
        int bookerId = 123456;
        string courtId = "D1";
        DateOnly date = new DateOnly(2026, 4, 10);
        TimeOnly startTime = new TimeOnly(16, 0);
        TimeOnly endTime = new TimeOnly(17, 0);

        
        var result = CreateBookingCommand.Create(
            scheduleId,
            bookerId,
            courtId,
            date,
            startTime,
            endTime);

        
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.value);
        Assert.Equal(bookerId, result.value.BookerId.Value);
        Assert.Equal(courtId, result.value.CourtId.GetValue());
        Assert.Equal(date, result.value.Date);
        Assert.Equal(startTime, result.value.StartTime);
        Assert.Equal(endTime, result.value.EndTime);
    }

    [Fact]
    public void Create_InvalidScheduleId_ThrowsFormatException()
    {
        
        string scheduleId = "not-a-guid";
        int bookerId = 123456;
        string courtId = "D1";
        DateOnly date = new DateOnly(2026, 4, 10);
        TimeOnly startTime = new TimeOnly(16, 0);
        TimeOnly endTime = new TimeOnly(17, 0);

        
        Assert.Throws<FormatException>(() =>
            CreateBookingCommand.Create(
                scheduleId,
                bookerId,
                courtId,
                date,
                startTime,
                endTime));
    }
}