using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;
using UnitTests.Features.Schedules.Booking.Helpers;

namespace UnitTests.Features.Schedules.Booking.BookingLifecycle;

public class BookingLifecycleAggregateTests
{
    [Fact]
    public void Pending_Confirm_Complete_WorksEndToEnd()
    {
        var booking = BookingTestHelper.CreateBooking(status: BookingStatus.Pending);

        var confirmResult = booking.ConfirmBooking();
        var completeResult = booking.CompleteBooking();

        Assert.False(confirmResult.IsFailure);
        Assert.False(completeResult.IsFailure);
        Assert.Equal(BookingStatus.Completed, BookingTestHelper.GetStatus(booking));
    }

    [Fact]
    public void Pending_Confirm_Cancel_ThenComplete_ReturnsFailureAndStaysCancelled()
    {
        var booking = BookingTestHelper.CreateBooking(status: BookingStatus.Pending);

        var confirmResult = booking.ConfirmBooking();
        var cancelResult = booking.CancelBooking();
        var completeResult = booking.CompleteBooking();

        Assert.False(confirmResult.IsFailure);
        Assert.False(cancelResult.IsFailure);
        Assert.True(completeResult.IsFailure);
        Assert.Equal(BookingStatus.Cancelled, BookingTestHelper.GetStatus(booking));
    }

    [Fact]
    public void Pending_Cancel_ThenConfirm_ReturnsFailureAndStaysCancelled()
    {
        var booking = BookingTestHelper.CreateBooking(status: BookingStatus.Pending);

        var cancelResult = booking.CancelBooking();
        var confirmResult = booking.ConfirmBooking();

        Assert.False(cancelResult.IsFailure);
        Assert.True(confirmResult.IsFailure);
        Assert.Equal(BookingStatus.Cancelled, BookingTestHelper.GetStatus(booking));
    }

    [Fact]
    public void Pending_Confirm_Twice_SecondCallReturnsFailureAndStaysConfirmed()
    {
        var booking = BookingTestHelper.CreateBooking(status: BookingStatus.Pending);

        var firstResult = booking.ConfirmBooking();
        var secondResult = booking.ConfirmBooking();

        Assert.False(firstResult.IsFailure);
        Assert.True(secondResult.IsFailure);
        Assert.Equal(BookingStatus.Confirmed, BookingTestHelper.GetStatus(booking));
    }
}
