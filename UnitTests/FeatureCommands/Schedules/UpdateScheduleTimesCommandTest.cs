using DCA_Padel_Club.Core.Application.Commands.ScheduleCommands;

namespace UnitTests.FeatureCommands.Schedules;

public class UpdateScheduleTimesCommandTest
{
    [Fact]
    public void Create_ValidInput_ReturnsSuccess()
    {
        string scheduleId = Guid.NewGuid().ToString();
        string startTime = "15:00";
        string endTime = "16:00";

        var result = UpdateScheduleTimeCommand.Create(scheduleId, startTime, endTime);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.value);
        Assert.Equal(TimeOnly.Parse(startTime), result.value.StartTime);
        Assert.Equal(TimeOnly.Parse(endTime), result.value.EndTime);
    }

    [Fact]
    public void Create_InvalidScheduleId_ThrowsFormatException()
    {
        string scheduleId = "not-a-guid";
        string startTime = "15:00";
        string endTime = "16:00";

        Assert.Throws<FormatException>(() =>
            UpdateScheduleTimeCommand.Create(scheduleId, startTime, endTime));
    }
}