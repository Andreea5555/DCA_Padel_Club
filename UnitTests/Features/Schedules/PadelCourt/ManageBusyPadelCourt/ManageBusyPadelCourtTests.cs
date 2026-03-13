using DCA_Padel_Club.Core.Domain.Aggregates.Schedule;

namespace UnitTests.Features.Schedule.ManageBusyPadelCourt;

public class ManageBusyPadelCourtTests
{
    private PadelCourt CreatePadelCourt()
    {
        var result = CourtId.CreateCourtId("S1");
        return new PadelCourt(result.value);
    }

    [Fact]
    public void PadelCourtIsOccupied()
    {
        var padelCourt = CreatePadelCourt();
        padelCourt.MarkAsOccupied();
        Assert.True(padelCourt.isOccupied);
    }

    [Fact]
    public void PadelCourtIsNotOccupied()
    {
        var padelCourt = CreatePadelCourt();
        padelCourt.MarkAsAvailable();
        Assert.False(padelCourt.isOccupied);
    }

    [Fact]
    public void VIPAccessIsEnabled()
    {
        var padelCourt = CreatePadelCourt();
        padelCourt.EnableVIPAccess();
        Assert.True(padelCourt.isVIPEnabled);
    }

    public void VIPAccessIsNotEnabled()
    {
        var padelCourt = CreatePadelCourt();
        padelCourt.DisableVIPAccess();
        Assert.False(padelCourt.isVIPEnabled);
    }
}