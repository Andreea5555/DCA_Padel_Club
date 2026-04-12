using DCA_Padel_Club.Core.Application.Commands.ScheduleCommands;

namespace UnitTests.FeatureCommands.Schedule;

public class CreateScheduleCommandTest
{

    [Fact]
    public void Create_ReturnsSuccess()
    {
       
        var result = CreateScheduleCommand.Create();

        
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.value);
    }
    
}
