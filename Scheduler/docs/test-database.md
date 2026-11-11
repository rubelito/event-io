# testDatabase.db snapshot

Source: `Db/testDatabase.db` (106,496 bytes). SHA-256: `aae728045acb9717d15d2ad129319f614bad69cae03bf9a66dcc6848624fa6f2`.

The source was opened read-only. SQLite integrity check: **ok**; foreign-key violations: **0**. There are 10 application tables, 87 rows, 6 explicit indexes, and no views, triggers or EF migration-history table. All application columns are NOT NULL. Dates/times are stored as TEXT; booleans and enums as INTEGER.

| Table | Rows | Purpose |
| --- | ---: | --- |
| Users | 14 | Accounts and profile fields |
| Groups | 17 | Groups owned by users |
| Appointments | 18 | Appointments (10), tasks (5), reminders (3) |
| Contacts | 5 | Directed user-to-user contact links |
| Meetings | 30 | User attendees linked to appointments |
| GroupsMeetings | 0 | Group attendees linked to appointments |
| UsersGroups | 3 | Group membership |
| UserPictures | 0 | Optional per-user image BLOBs |
| RepeatEdits | 0 | Overrides/deletions of recurring occurrences |
| ExceptionLogs | 0 | Application exception history |

## Seed date adjustment

The current seeder and both the source and runtime databases now use November 2026 for all 18 appointment start dates. Relative to the original May snapshot, this is a six-month shift preserving year, day and time; YearMonth is 11/2026. End dates are derived as Date + EndDateSpan, shifted by the same number of months, and EndDateSpan is recalculated as shifted end date minus shifted start date. All endpoints fit in November, so the spans remain unchanged: Team Building (ID 5) runs November 17-19; Pest Control (ID 6) runs November 22-24; the other 16 entries have span 0. EndTime is a time of day and remains unchanged. The unused OnDate sentinel is retained.

## Data observations

- Users have IDs 1?14 and are all active. Password values are preserved exactly as stored.
- Groups have IDs 1?17, are all active, and are owned by user 4 (john). The names skip Team 11; this is preserved.
- All 18 calendar entries start in May 2026. Five tasks repeat weekly; all entries have IsDone = 0. Original titles, spelling, times, colors, details and repeat limits are retained.
- All 5 contact links belong to user 4, targeting users 5, 8, 10, 12 and 13. Contacts are directional, not automatically mutual.
- Group 1 contains users 1, 2 and 3. There are 30 individual attendee links, no group invitations, no avatars, no repeat overrides and no exception logs.
- AUTOINCREMENT counters: Users = 14; Groups = 17; Appointments = 18. The snapshot restores sqlite_sequence as well as application rows.

## Schema

### Users

| Column | SQLite type | Primary-key position |
| --- | --- | ---: |
| Id | INTEGER | 1 |
| UserName | TEXT | ? |
| Password | TEXT | ? |
| Email | TEXT | ? |
| FirstName | TEXT | ? |
| MiddleName | TEXT | ? |
| LastName | TEXT | ? |
| LastLoggin | TEXT | ? |
| Active | INTEGER | ? |

No declared foreign keys.

### Groups

| Column | SQLite type | Primary-key position |
| --- | --- | ---: |
| Id | INTEGER | 1 |
| GroupName | TEXT | ? |
| Description | TEXT | ? |
| OwnerId | INTEGER | ? |
| Active | INTEGER | ? |

- OwnerId ? Users.Id; ON DELETE CASCADE.

### Appointments

| Column | SQLite type | Primary-key position |
| --- | --- | ---: |
| Id | INTEGER | 1 |
| Location | TEXT | ? |
| Title | TEXT | ? |
| Color | TEXT | ? |
| Details | TEXT | ? |
| YearMonth | TEXT | ? |
| Date | TEXT | ? |
| Time | TEXT | ? |
| EndDateSpan | INTEGER | ? |
| EndTime | TEXT | ? |
| CreatorId | INTEGER | ? |
| isRepeat | INTEGER | ? |
| RepeatSelection | INTEGER | ? |
| RepeatEnd | INTEGER | ? |
| After | INTEGER | ? |
| OnDate | TEXT | ? |
| Type | INTEGER | ? |
| HasNoWeekEnds | INTEGER | ? |
| IsDone | INTEGER | ? |

- CreatorId ? Users.Id; ON DELETE CASCADE.

### Contacts

| Column | SQLite type | Primary-key position |
| --- | --- | ---: |
| UserId | INTEGER | 1 |
| ContactId | INTEGER | 2 |

- UserId ? Users.Id; ON DELETE CASCADE.
- ContactId ? Users.Id; ON DELETE CASCADE.

### Meetings

| Column | SQLite type | Primary-key position |
| --- | --- | ---: |
| ParticipantId | INTEGER | 1 |
| MeetingId | INTEGER | 2 |

- ParticipantId ? Users.Id; ON DELETE CASCADE.
- MeetingId ? Appointments.Id; ON DELETE CASCADE.

### GroupsMeetings

| Column | SQLite type | Primary-key position |
| --- | --- | ---: |
| ParticipantId | INTEGER | 1 |
| MeetingId | INTEGER | 2 |

- ParticipantId ? Groups.Id; ON DELETE CASCADE.
- MeetingId ? Appointments.Id; ON DELETE CASCADE.

### UsersGroups

| Column | SQLite type | Primary-key position |
| --- | --- | ---: |
| UserId | INTEGER | 1 |
| GroupId | INTEGER | 2 |

- UserId ? Users.Id; ON DELETE CASCADE.
- GroupId ? Groups.Id; ON DELETE CASCADE.

### UserPictures

| Column | SQLite type | Primary-key position |
| --- | --- | ---: |
| UserId | INTEGER | 1 |
| Picture | BLOB | ? |

- UserId ? Users.Id; ON DELETE CASCADE.

### RepeatEdits

| Column | SQLite type | Primary-key position |
| --- | --- | ---: |
| Id | INTEGER | 1 |
| AppointmentId | INTEGER | ? |
| OriginalDate | TEXT | ? |
| EditedDate | TEXT | ? |
| Title | TEXT | ? |
| Location | TEXT | ? |
| Color | TEXT | ? |
| Details | TEXT | ? |
| Time | TEXT | ? |
| EndDateSpan | INTEGER | ? |
| EndTime | TEXT | ? |
| IsDeleted | INTEGER | ? |
| IsDone | INTEGER | ? |

No declared foreign keys.

### ExceptionLogs

| Column | SQLite type | Primary-key position |
| --- | --- | ---: |
| Id | INTEGER | 1 |
| UserName | TEXT | ? |
| Action | TEXT | ? |
| Message | TEXT | ? |
| Source | TEXT | ? |
| StackTrace | TEXT | ? |
| InnerMessage | TEXT | ? |
| InnerSource | TEXT | ? |
| InnerStackTrace | TEXT | ? |
| LogDateTime | TEXT | ? |

No declared foreign keys.

All declared foreign keys use ON DELETE CASCADE. RepeatEdits.AppointmentId is a logical reference only: the database does not declare a foreign-key constraint for it. UserPictures.UserId is both its primary key and a foreign key, allowing at most one image per user.

## Indexes and constraints

- CREATE INDEX "IX_Appointments_CreatorId" ON "Appointments" ("CreatorId");
- CREATE INDEX "IX_Contacts_ContactId" ON "Contacts" ("ContactId");
- CREATE INDEX "IX_GroupsMeetings_MeetingId" ON "GroupsMeetings" ("MeetingId");
- CREATE INDEX "IX_Groups_OwnerId" ON "Groups" ("OwnerId");
- CREATE INDEX "IX_Meetings_MeetingId" ON "Meetings" ("MeetingId");
- CREATE INDEX "IX_UsersGroups_GroupId" ON "UsersGroups" ("GroupId");

Composite primary keys additionally create SQLite autoindexes. The schema does not declare unique constraints on usernames, email addresses or group names; uniqueness is checked by application code where implemented.

## November application verification

Updated both Db/testDatabase.db and bin/Debug/net8.0/Db/testDatabase.db in one SQLite transaction. Verified 18 November events in each database, independently recalculated durations, and confirmed every other field/table is unchanged. Integrity and foreign-key checks passed. The running application uses the updated runtime copy on its next request.

## Earlier October seed verification

The updated C# seeder was compiled and run against temporary missing/empty databases. All 18 seeded entries have October 2026 Date/YearMonth values; end-day spans were independently recalculated from shifted start/end dates. Every other value, table and ID counter was compared to the source and remained unchanged. SQLite integrity and foreign-key checks passed; the original database checksum is unchanged.

## Original snapshot verification

The C# class was compiled and executed against two temporary destinations: a missing database in a missing directory, and a pre-existing empty file. Both matched the source across all 10 tables plus sqlite_sequence, every row value and SQLite storage type, all schema objects and indexes (DDL line endings normalized), integrity_check and foreign_key_check. A repeated call was rejected and the destination hash remained unchanged. The original database SHA-256 remained unchanged.

## Use the C# seeder

The self-contained class is `Services/TestDatabaseSeeder.cs`. It uses Microsoft.Data.Sqlite, already available through the application's EF Core SQLite dependency. It embeds the original DDL and all literal row values; it does not need the original database file at runtime.

```csharp
using Scheduler.Db;

TestDatabaseSeeder.Seed(@"C:\data\event-io\Db\testDatabase.db");
```

Choose the destination explicitly. The application currently reads `Db/testDatabase.db` relative to its executable; the source copy in the repository is a different path. The seeder accepts a missing database or an empty SQLite file, creates parent directories, enables foreign keys, and uses one transaction for schema and data. An initialized destination is rejected without changing its contents. It does not reset a database or merge records, and no automatic startup call has been added.

This reproduces schema, values, indexes and ID counters with the appointment date adjustment described above; SQLite file bytes/page layout may differ. Changes to future EF models will not change this fixed historical snapshot.
