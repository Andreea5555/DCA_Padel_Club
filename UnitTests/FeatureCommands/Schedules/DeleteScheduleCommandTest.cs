using DCA_Padel_Club.Core.Application.Commands.ScheduleCommands;

namespace UnitTests.FeatureCommands.Schedules;

public class DeleteScheduleCommandTest
{
    [Fact]
    public void Create_ValidInput_ReturnsSuccess()
    {
        string scheduleId = Guid.NewGuid().ToString();

        var result = DeleteScheduleCommand.Create(scheduleId);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.value);
    }

    [Fact]
    public void Create_InvalidScheduleId_ThrowsFormatException()
    {
        string scheduleId = "not-a-guid";

        Assert.Throws<FormatException>(() =>
            DeleteScheduleCommand.Create(scheduleId));
    }
}