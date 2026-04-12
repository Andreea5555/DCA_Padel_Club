using DCA_Padel_Club.Core.Application.Commands.ScheduleCommands;

namespace UnitTests.FeatureCommands.Schedules;

public class AddCourtCommandTest
{
    [Fact]
    public void Create_ValidInput_ReturnsSuccess()
    {
        string scheduleId = Guid.NewGuid().ToString();
        string courtId = "D2";

        var result = AddCourtCommand.Create(scheduleId,courtId);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.value);
        Assert.Equal(courtId, result.value.CourtId.GetValue());
    }
    
    [Fact]
    public void Create_InvalidScheduleId_ThrowsFormatException()
    {
        string scheduleId = "not-a-guid";
        string courtId = "D3";

        Assert.Throws<FormatException>(() =>
            AddCourtCommand.Create(scheduleId,courtId));
    }
    
}