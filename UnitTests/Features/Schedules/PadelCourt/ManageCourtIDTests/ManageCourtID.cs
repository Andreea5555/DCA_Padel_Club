using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;

namespace UnitTests.Features.Schedules.PadelCourt.ManageCourtIDTests;

public class ManageCourtID
{
    [Fact]
    public void CreateCourtId_WithValidSinglesFormat_ReturnsSuccess()
    {
        var result = CourtId.CreateCourtId("S1");

        Assert.False(result.IsFailure);
        Assert.Equal("S1", result.value.GetValue());
    }

    [Fact]
    public void CreateCourtId_WithValidDoublesFormat_ReturnsSuccess()
    {
        var result = CourtId.CreateCourtId("D10");

        Assert.False(result.IsFailure);
        Assert.Equal("D10", result.value.GetValue());
    }

    [Fact]
    public void CreateCourtId_WithLowercase_NormalizesToUppercase()
    {
        var result = CourtId.CreateCourtId("s5");

        Assert.False(result.IsFailure);
        Assert.Equal("S5", result.value.GetValue());
    }

    [Fact]
    public void CreateCourtId_WithInvalidPrefix_ReturnsFailure()
    {
        var result = CourtId.CreateCourtId("X1");

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "CourtId.InvalidPrefix");
    }

    [Fact]
    public void CreateCourtId_WithNumberAbove10_ReturnsFailure()
    {
        var result = CourtId.CreateCourtId("S11");

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "CourtId.InvalidNumber");
    }

    [Fact]
    public void CreateCourtId_WithNumberZero_ReturnsFailure()
    {
        var result = CourtId.CreateCourtId("S0");

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "CourtId.InvalidNumber");
    }

    [Fact]
    public void CreateCourtId_WithTooShortName_ReturnsFailure()
    {
        var result = CourtId.CreateCourtId("S");

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "CourtId.InvalidLength");
    }

    [Fact]
    public void CreateCourtId_WithTooLongName_ReturnsFailure()
    {
        var result = CourtId.CreateCourtId("S123");

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "CourtId.InvalidLength");
    }

    [Fact]
    public void CreateCourtId_WithEmptyName_ReturnsFailure()
    {
        var result = CourtId.CreateCourtId("");

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "CourtId.Empty");
    }
}
