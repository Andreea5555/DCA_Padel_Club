using DCA_Padel_Club.Core.Application.Commands.ScheduleCommands;

namespace UnitTests.FeatureCommands.Schedules;

public class UpdateScheduleDateCommandTest
{
    [Fact]
    public void Create_ValidInput_ReturnsSuccess()
    {
        string scheduleId = Guid.NewGuid().ToString();
        string date = "2026-04-10";

        var result = UpdateScheduleDateCommand.Create(scheduleId, date);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.value);
        Assert.Equal(DateOnly.Parse(date), result.value.Date);
    }

    [Fact]
    public void Create_InvalidScheduleId_ThrowsFormatException()
    {
        string scheduleId = "not-a-guid";
        string date = "2026-04-10";

        Assert.Throws<FormatException>(() =>
            UpdateScheduleDateCommand.Create(scheduleId, date));
    }
}