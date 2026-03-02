using DCA_Padel_Club.Core.Tools.OperationResult;

namespace UnitTests.OperationResult;

public class OperationResultUnitTests
{
    [Fact]
    public void SuccessResult_ShouldHaveValueAndNoErrors()
    {
        
        var expectedValue = 42;

       
        var result = Result<int>.Success(expectedValue);

        
        Assert.False(result.IsFailure);
        Assert.Equal(expectedValue, result.value);
        Assert.Empty(result.errorMessages);
    }
    
}