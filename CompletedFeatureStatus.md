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


## 4.UC3 - Create New Daily Schedule
### Domain Implementation
* [x] Happening through constructor of Schedule

### Unit Tests (TDD) 
* [x] no need since it should not be able to fail

## 5.UC4 - Update Time And Date Of Schedule
### Domain Implementation
* [x] Checking Date & Time Validation 
* [x] Catching list of errors if relevant
* [x] Returning errors if relevant
* [x] Updating the fields inside Schedule
* [x] Returning success result if relevant

### Unit Tests (TDD)
* [x] Update Schedule Date Schedule_NotDraft (Done?)
* [x] Update 