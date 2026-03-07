using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;
using UnitTests.Features.Schedules.Booking.Helpers;

namespace UnitTests.Features.Schedules.Booking.ManageBookingStatus;

public class ManageBookingStatusAggregateTests
{
    [Fact]
    public void Confirm_FromPending_SetsStatusToConfirmed()
    {
        var booking = BookingTestHelper.CreateBooking(status: BookingStatus.Pending);

        var result = booking.ConfirmBooking();

        Assert.False(result.IsFailure);
        Assert.Equal(BookingStatus.Confirmed, BookingTestHelper.GetStatus(booking));
    }

    [Fact]
    public void Confirm_FromConfirmed_ReturnsFailure()
    {
        var booking = BookingTestHelper.CreateBooking(status: BookingStatus.Confirmed);

        var result = booking.ConfirmBooking();

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Booking.Confirm.InvalidStatus");
        Assert.Equal(BookingStatus.Confirmed, BookingTestHelper.GetStatus(booking));
    }

    [Fact]
    public void Confirm_FromCancelled_ReturnsFailure()
    {
        var booking = BookingTestHelper.CreateBooking(status: BookingStatus.Cancelled);

        var result = booking.ConfirmBooking();

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Booking.Confirm.InvalidStatus");
        Assert.Equal(BookingStatus.Cancelled, BookingTestHelper.GetStatus(booking));
    }

    [Fact]
    public void Confirm_FromCompleted_ReturnsFailure()
    {
        var booking = BookingTestHelper.CreateBooking(status: BookingStatus.Completed);

        var result = booking.ConfirmBooking();

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Booking.Confirm.InvalidStatus");
        Assert.Equal(BookingStatus.Completed, BookingTestHelper.GetStatus(booking));
    }

    [Fact]
    public void Cancel_FromPending_SetsStatusToCancelled()
    {
        var booking = BookingTestHelper.CreateBooking(status: BookingStatus.Pending);

        var result = booking.CancelBooking();

        Assert.False(result.IsFailure);
        Assert.Equal(BookingStatus.Cancelled, BookingTestHelper.GetStatus(booking));
    }

    [Fact]
    public void Cancel_FromConfirmed_SetsStatusToCancelled()
    {
        var booking = BookingTestHelper.CreateBooking(status: BookingStatus.Confirmed);

        var result = booking.CancelBooking();

        Assert.False(result.IsFailure);
        Assert.Equal(BookingStatus.Cancelled, BookingTestHelper.GetStatus(booking));
    }

    [Fact]
    public void Cancel_FromCancelled_ReturnsFailure()
    {
        var booking = BookingTestHelper.CreateBooking(status: BookingStatus.Cancelled);

        var result = booking.CancelBooking();

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Booking.Cancel.InvalidStatus");
        Assert.Equal(BookingStatus.Cancelled, BookingTestHelper.GetStatus(booking));
    }

    [Fact]
    public void Cancel_FromCompleted_ReturnsFailure()
    {
        var booking = BookingTestHelper.CreateBooking(status: BookingStatus.Completed);

        var result = booking.CancelBooking();

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Booking.Cancel.InvalidStatus");
        Assert.Equal(BookingStatus.Completed, BookingTestHelper.GetStatus(booking));
    }

    [Fact]
    public void Complete_FromConfirmed_SetsStatusToCompleted()
    {
        var booking = BookingTestHelper.CreateBooking(status: BookingStatus.Confirmed);

        var result = booking.CompleteBooking();

        Assert.False(result.IsFailure);
        Assert.Equal(BookingStatus.Completed, BookingTestHelper.GetStatus(booking));
    }

    [Fact]
    public void Complete_FromPending_ReturnsFailure()
    {
        var booking = BookingTestHelper.CreateBooking(status: BookingStatus.Pending);

        var result = booking.CompleteBooking();

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Booking.Complete.InvalidStatus");
        Assert.Equal(BookingStatus.Pending, BookingTestHelper.GetStatus(booking));
    }

    [Fact]
    public void Complete_FromCancelled_ReturnsFailure()
    {
        var booking = BookingTestHelper.CreateBooking(status: BookingStatus.Cancelled);

        var result = booking.CompleteBooking();

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Booking.Complete.InvalidStatus");
        Assert.Equal(BookingStatus.Cancelled, BookingTestHelper.GetStatus(booking));
    }

    [Fact]
    public void Complete_FromCompleted_ReturnsFailure()
    {
        var booking = BookingTestHelper.CreateBooking(status: BookingStatus.Completed);

        var result = booking.CompleteBooking();

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Booking.Complete.InvalidStatus");
        Assert.Equal(BookingStatus.Completed, BookingTestHelper.GetStatus(booking));
    }
}
