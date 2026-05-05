## DailyScheduleBookingOverview
Input:
- ScheduleId

Output:
- Date
- StartTime
- EndTime
- Courts: CourtId
- Bookings: BookingId, CourtId, StartTime, EndTime

## PlayerPage
Input:
- PlayerId

Output:
- PlayerId
- FirstName
- LastName
- Email
- ProfilePicture
- UpcomingBookingsCount
- UpcomingBookings:
  - BookingId
  - ScheduleId
  - Date
  - StartTime
  - EndTime
  - CourtId
- PastBookings:
  - BookingId
  - ScheduleId
  - Date
  - StartTime
  - EndTime
  - CourtId

## ScheduleOverview
Input:
- Year
- Month

Output:
- ScheduleId
- Date
- Status

## ManagerScheduleOverview
Input:
- Year
- Month

Output:
- ScheduleId
- Date
- StartTime
- EndTime

## Booking Details View
Input:
- BookingId

Output:
- BookingId
- ScheduleId
- Date
- StartTime
- EndTime
- CourtId
- BookerId
- BookerName
- BookerEmail
- Status

