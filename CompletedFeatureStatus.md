# Completed Feature Status

## 1. Foundation & Bases
* [x] ValueObject base
* [x] Entity base
* [x] Aggregate base

## 2. UC1 - Register Player
### Domain Implementation
* [x] Value - ViaId
* [x] Value - Email
* [x] Value - Password
* [x] Aggregate - Player properties & constructor

### Unit Tests (TDD)
* [x] S1 - Successful registration
* [x] F1 - Failure: Empty password returns Result.Failure

## 3. UC2 - Manage Player Status & Profile
### Domain Implementation
* [x] BlacklistPlayer() & UnblacklistPlayer() logic
* [x] QuarantinePlayer() logic
* [x] VIP Logic (Renew, Revoke, IsEligible)
* [x] Profile Logic (ChangePassword, ChangeEmail)

### Unit Tests (TDD)
* [x] S1 - Blacklisting & Quarantining state changes
* [x] S2 - VIP status management (Renew/Revoke)
* [x] S3 - VIP Court Eligibility evaluation
* [x] S4 - Profile updates (Email, Password Result checks)
* [x] S5 - IsEligibleToBook() evaluation logic (DONE!)


## 4. UC3 - Create New Daily Schedule
### Domain Implementation
* [x] Happening through constructor of Schedule

### Unit Tests (TDD)
* [x] S1 - HasNonEmptyId (new Guid assigned on creation)
* [x] S1 - IsDraft on creation
* [x] S1 - HasEmptyCourts on creation
* [x] S1 - DefaultStartTimeIs15 (15:00)
* [x] S1 - DefaultEndTimeIs22 (22:00)
* [x] S1 - DefaultDateIsToday
* [x] S1 - IsNotDeleted on creation

## 5. UC4 - Update Time And Date Of Schedule
### Domain Implementation
* [x] ICurrentDate domain contract (DateOnly Now)
* [x] Date validation: past-date check via ICurrentDate
* [x] Date validation: non-draft rejection
* [x] Time validation: start > end rejection
* [x] Time validation: duration < 60 min rejection
* [x] Time validation: invalid minutes (:00/:30 only)
* [x] Collecting multiple errors per call
* [x] Updating fields and returning success

### Unit Tests (TDD)
* [x] UpdateScheduleDate_Schedule_NotDraft
* [x] UpdateScheduleDate_FutureDate_OnDraftSchedule_ReturnsSuccess
* [x] UpdateScheduleDate_DateInPast_ReturnsFailure
* [x] UpdateScheduleDate_NotDraftAndDateInPast_ReturnsBothErrors
* [x] UpdateScheduleTime_StartTime_Bigger_Than_EndTime
* [x] UpdateScheduleTime_Duration_IsLessThan_60_Minutes
* [x] UpdateScheduleTime_Schedule_NotDraft
* [x] UpdateSchedule_Minutes_Are_Invalid
* [x] UpdateSchedule_Return_Multiple_Errors
* [x] UpdateSchedule_Success

## 6. UC5 - Add Courts To Daily Schedule
### Domain Implementation
* [x] CourtId value object (S/D prefix, 1-10 number, 2-3 chars)
* [x] AddCourt() with duplicate check and past-date guard
* [ ] F3 - Deleted schedule check not implemented

### Unit Tests (TDD)
* [ ] No tests yet — all scenarios untested

## 7. UC6 - Activate Daily Schedule
### Domain Implementation
* [x] ActivateSchedule() — requires courts, future date, draft state
* [ ] F3 - Deleted schedule check not implemented
* [ ] F5 - Date conflict (needs repository layer)

### Unit Tests (TDD)
* [x] S1 - ActivateSchedule_WhenValid
* [x] F1 - ActivateSchedule_Schedule_Has_Zero_Courts
* [x] F2 - ActivateSchedule_While_Schedule_DateTime_Has_Passed
* [x] F6 - ActivateSchedule_Schedule_Already_Active

## 8. UC7 - Player Makes A Booking
### Domain Implementation
* [x] BookingSlot value object (1-3 hour range, :00/:30 minutes)
* [x] CreateBooking() — deleted/draft/court/slot/overlap/duplicate-player checks
* [ ] F13/F14 - Player quarantine/blacklist not checked
* [ ] F18 - "Hole <1 hour" rule not implemented
* [ ] F19 - Booking before current time (needs ICurrentDate)

### Unit Tests (TDD)
* [x] S1 - CreateBooking_WhenScheduleActiveAndCourtExists_ReturnsSuccessAndStoresBooking
* [x] S2 - CreateBooking_WhenSameTimeSlotOnDifferentCourt_ReturnsSuccess
* [x] S3 - CreateBooking_TwoBookingsOnDifferentCourts_BothStored
* [x] F1 - CreateBooking_WhenScheduleIsDeleted_ReturnsFailure
* [x] F2 - CreateBooking_WhenScheduleIsDraft_ReturnsFailure
* [x] F4 - CreateBooking_WhenCourtNotInSchedule_ReturnsFailure
* [x] F5 - CreateBooking_WhenSlotStartsBeforeScheduleStart_ReturnsFailure
* [x] F8 - CreateBooking_WhenSlotEndsAfterScheduleEnd_ReturnsFailure
* [x] F9 - BookingSlot invalid minutes tests
* [x] F10 - BookingSlot Create_WhenDurationIsLessThan1Hour_ReturnsFailure
* [x] F11 - CreateBooking_WhenOverlapsExistingBookingOnSameCourt_ReturnsFailure
* [x] F12 - BookingSlot Create_WhenDurationIsMoreThan3Hours_ReturnsFailure
* [x] F17 - CreateBooking_WhenPlayerAlreadyHasBookingOnSameDate_ReturnsFailure

## 9. UC8 - Cancel A Booking
### Domain Implementation
* [x] CancelBooking() — basic status transition
* [ ] F2 - 1-hour-before cancellation cutoff not implemented
* [ ] F1 - Past booking check not implemented

### Unit Tests (TDD)
* [x] S1 - Basic status transition test (partial)

## 10. UC9 - Remove Court From Daily Schedule
### Domain Implementation
* [x] RemoveCourt() — basic removal, today's-date guard
* [ ] F1/F2/F4/F5 - Most validations not implemented

### Unit Tests (TDD)
* [ ] No tests yet

## 11. UC10 - Delete Daily Schedule
### Domain Implementation
* [x] RemoveSchedule() — past/today/already-deleted guards, clears courts
* [ ] F1 - No schedule found (service layer)

### Unit Tests (TDD)
* [x] S1 - RemoveSchedule_WhenDateIsInFuture_ReturnsSuccess_AndMarksDeleted
* [x] F2 - RemoveSchedule_WhenDateIsInPast_ReturnsFailure
* [x] F3 - RemoveSchedule_WhenAlreadyDeleted_ReturnsFailure
* [x] F4 - RemoveSchedule_WhenDateIsToday_ReturnsFailure
