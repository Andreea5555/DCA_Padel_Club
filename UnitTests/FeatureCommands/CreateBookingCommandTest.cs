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
        string date = "2021-01-01";
        string startTime ="16:00";
        string endTime = "17:00";

        
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
        Assert.Equal(DateOnly.Parse(date), result.value.Date);
        Assert.Equal(TimeOnly.Parse(startTime), result.value.StartTime);
        Assert.Equal(TimeOnly.Parse(endTime), result.value.EndTime);
    }

    [Fact]
    public void Create_InvalidScheduleId_ThrowsFormatException()
    {
        
        string scheduleId = "not-a-guid";
        int bookerId = 123456;
        string courtId = "D1";
        string date = "2021-01-01";
        string startTime ="16:00";
        string endTime = "17:00";

        
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