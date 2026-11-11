using Microsoft.Data.Sqlite;

namespace Scheduler.Db;

/// <summary>
/// Recreates the schema and data captured from Db/testDatabase.db, with appointment
/// start/end dates shifted from May to November 2026 and day spans recalculated.
/// Original source SHA-256: aae728045acb9717d15d2ad129319f614bad69cae03bf9a66dcc6848624fa6f2
/// This is an explicit snapshot operation, not an automatic application startup hook.
/// </summary>
public static class TestDatabaseSeeder
{
    /// <summary>
    /// Creates the parent directory, schema and all 87 snapshot rows atomically.
    /// Accepts a missing database or an empty SQLite file. Refuses any existing
    /// user schema, even if its tables are empty, to avoid merging or overwriting data.
    /// </summary>
    /// <param name="databasePath">An explicit destination path, separate from the source snapshot.</param>
    public static void Seed(string databasePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(databasePath);
        var fullPath = Path.GetFullPath(databasePath);
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);

        using var connection = new SqliteConnection(new SqliteConnectionStringBuilder
        {
            DataSource = fullPath,
            Mode = SqliteOpenMode.ReadWriteCreate,
            ForeignKeys = true,
            Pooling = false
        }.ToString());
        connection.Open();
        using var transaction = connection.BeginTransaction();

        using (var existingSchema = connection.CreateCommand())
        {
            existingSchema.Transaction = transaction;
            existingSchema.CommandText = "SELECT COUNT(*) FROM sqlite_master WHERE name NOT LIKE 'sqlite_%';";
            if (Convert.ToInt64(existingSchema.ExecuteScalar()) != 0)
            {
                throw new InvalidOperationException(
                    "The destination already contains a schema. Seed into a new or empty database; existing data is never overwritten.");
            }
        }

        Execute(connection, transaction, SchemaSql);
        Execute(connection, transaction, DataSql);

        using (var check = connection.CreateCommand())
        {
            check.Transaction = transaction;
            check.CommandText = "PRAGMA foreign_key_check;";
            using var reader = check.ExecuteReader();
            if (reader.Read())
            {
                throw new InvalidOperationException("Snapshot contains an invalid foreign key. Seeding was rolled back.");
            }
        }
        transaction.Commit();
    }

    private static void Execute(SqliteConnection connection, SqliteTransaction transaction, string sql)
    {
        using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = sql;
        command.ExecuteNonQuery();
    }

    private const string SchemaSql = """"
        CREATE TABLE "Users" (
            "Id" INTEGER NOT NULL CONSTRAINT "PK_Users" PRIMARY KEY AUTOINCREMENT,
            "UserName" TEXT NOT NULL,
            "Password" TEXT NOT NULL,
            "Email" TEXT NOT NULL,
            "FirstName" TEXT NOT NULL,
            "MiddleName" TEXT NOT NULL,
            "LastName" TEXT NOT NULL,
            "LastLoggin" TEXT NOT NULL,
            "Active" INTEGER NOT NULL
        );
        
        CREATE TABLE "Groups" (
            "Id" INTEGER NOT NULL CONSTRAINT "PK_Groups" PRIMARY KEY AUTOINCREMENT,
            "GroupName" TEXT NOT NULL,
            "Description" TEXT NOT NULL,
            "OwnerId" INTEGER NOT NULL,
            "Active" INTEGER NOT NULL,
            CONSTRAINT "FK_Groups_Users_OwnerId" FOREIGN KEY ("OwnerId") REFERENCES "Users" ("Id") ON DELETE CASCADE
        );
        
        CREATE TABLE "Appointments" (
            "Id" INTEGER NOT NULL CONSTRAINT "PK_Appointments" PRIMARY KEY AUTOINCREMENT,
            "Location" TEXT NOT NULL,
            "Title" TEXT NOT NULL,
            "Color" TEXT NOT NULL,
            "Details" TEXT NOT NULL,
            "YearMonth" TEXT NOT NULL,
            "Date" TEXT NOT NULL,
            "Time" TEXT NOT NULL,
            "EndDateSpan" INTEGER NOT NULL,
            "EndTime" TEXT NOT NULL,
            "CreatorId" INTEGER NOT NULL,
            "isRepeat" INTEGER NOT NULL,
            "RepeatSelection" INTEGER NOT NULL,
            "RepeatEnd" INTEGER NOT NULL,
            "After" INTEGER NOT NULL,
            "OnDate" TEXT NOT NULL,
            "Type" INTEGER NOT NULL,
            "HasNoWeekEnds" INTEGER NOT NULL,
            "IsDone" INTEGER NOT NULL,
            CONSTRAINT "FK_Appointments_Users_CreatorId" FOREIGN KEY ("CreatorId") REFERENCES "Users" ("Id") ON DELETE CASCADE
        );
        
        CREATE TABLE "Contacts" (
            "UserId" INTEGER NOT NULL,
            "ContactId" INTEGER NOT NULL,
            CONSTRAINT "PK_Contacts" PRIMARY KEY ("UserId", "ContactId"),
            CONSTRAINT "FK_Contacts_Users_ContactId" FOREIGN KEY ("ContactId") REFERENCES "Users" ("Id") ON DELETE CASCADE,
            CONSTRAINT "FK_Contacts_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
        );
        
        CREATE TABLE "Meetings" (
            "ParticipantId" INTEGER NOT NULL,
            "MeetingId" INTEGER NOT NULL,
            CONSTRAINT "PK_Meetings" PRIMARY KEY ("ParticipantId", "MeetingId"),
            CONSTRAINT "FK_Meetings_Appointments_MeetingId" FOREIGN KEY ("MeetingId") REFERENCES "Appointments" ("Id") ON DELETE CASCADE,
            CONSTRAINT "FK_Meetings_Users_ParticipantId" FOREIGN KEY ("ParticipantId") REFERENCES "Users" ("Id") ON DELETE CASCADE
        );
        
        CREATE TABLE "GroupsMeetings" (
            "ParticipantId" INTEGER NOT NULL,
            "MeetingId" INTEGER NOT NULL,
            CONSTRAINT "PK_GroupsMeetings" PRIMARY KEY ("ParticipantId", "MeetingId"),
            CONSTRAINT "FK_GroupsMeetings_Appointments_MeetingId" FOREIGN KEY ("MeetingId") REFERENCES "Appointments" ("Id") ON DELETE CASCADE,
            CONSTRAINT "FK_GroupsMeetings_Groups_ParticipantId" FOREIGN KEY ("ParticipantId") REFERENCES "Groups" ("Id") ON DELETE CASCADE
        );
        
        CREATE TABLE "UsersGroups" (
            "UserId" INTEGER NOT NULL,
            "GroupId" INTEGER NOT NULL,
            CONSTRAINT "PK_UsersGroups" PRIMARY KEY ("UserId", "GroupId"),
            CONSTRAINT "FK_UsersGroups_Groups_GroupId" FOREIGN KEY ("GroupId") REFERENCES "Groups" ("Id") ON DELETE CASCADE,
            CONSTRAINT "FK_UsersGroups_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
        );
        
        CREATE TABLE "UserPictures" (
            "UserId" INTEGER NOT NULL CONSTRAINT "PK_UserPictures" PRIMARY KEY,
            "Picture" BLOB NOT NULL,
            CONSTRAINT "FK_UserPictures_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
        );
        
        CREATE TABLE "RepeatEdits" (
            "Id" INTEGER NOT NULL CONSTRAINT "PK_RepeatEdits" PRIMARY KEY AUTOINCREMENT,
            "AppointmentId" INTEGER NOT NULL,
            "OriginalDate" TEXT NOT NULL,
            "EditedDate" TEXT NOT NULL,
            "Title" TEXT NOT NULL,
            "Location" TEXT NOT NULL,
            "Color" TEXT NOT NULL,
            "Details" TEXT NOT NULL,
            "Time" TEXT NOT NULL,
            "EndDateSpan" INTEGER NOT NULL,
            "EndTime" TEXT NOT NULL,
            "IsDeleted" INTEGER NOT NULL,
            "IsDone" INTEGER NOT NULL
        );
        
        CREATE TABLE "ExceptionLogs" (
            "Id" INTEGER NOT NULL CONSTRAINT "PK_ExceptionLogs" PRIMARY KEY AUTOINCREMENT,
            "UserName" TEXT NOT NULL,
            "Action" TEXT NOT NULL,
            "Message" TEXT NOT NULL,
            "Source" TEXT NOT NULL,
            "StackTrace" TEXT NOT NULL,
            "InnerMessage" TEXT NOT NULL,
            "InnerSource" TEXT NOT NULL,
            "InnerStackTrace" TEXT NOT NULL,
            "LogDateTime" TEXT NOT NULL
        );
        
        CREATE INDEX "IX_Appointments_CreatorId" ON "Appointments" ("CreatorId");
        
        CREATE INDEX "IX_Contacts_ContactId" ON "Contacts" ("ContactId");
        
        CREATE INDEX "IX_GroupsMeetings_MeetingId" ON "GroupsMeetings" ("MeetingId");
        
        CREATE INDEX "IX_Groups_OwnerId" ON "Groups" ("OwnerId");
        
        CREATE INDEX "IX_Meetings_MeetingId" ON "Meetings" ("MeetingId");
        
        CREATE INDEX "IX_UsersGroups_GroupId" ON "UsersGroups" ("GroupId");
        """";

    // Literal SQL preserves stored date strings, empty values and explicit IDs
    // without conversions through the current EF model. Appointment YearMonth/Date
    // target November 2026. EndDateSpan = shifted end date minus shifted start date;
    // all shifted endpoints fit in November, so the original day spans are retained.
    // No source DB is read at runtime.
    private const string DataSql = """"
        -- Users: 14 rows
        INSERT INTO "Users" ("Id", "UserName", "Password", "Email", "FirstName", "MiddleName", "LastName", "LastLoggin", "Active") VALUES (1, 'kimb', 'kimb1030', 'Kimb@gmail.com', 'Kimberly', 'Padua', 'Pangan', '0001-01-01 00:00:00', 1);
        INSERT INTO "Users" ("Id", "UserName", "Password", "Email", "FirstName", "MiddleName", "LastName", "LastLoggin", "Active") VALUES (10, 'jasm', 'jasm1030', 'jasm@gmail.com', 'Jasmin', 'Lees', 'Lees', '0001-01-01 00:00:00', 1);
        INSERT INTO "Users" ("Id", "UserName", "Password", "Email", "FirstName", "MiddleName", "LastName", "LastLoggin", "Active") VALUES (11, 'susa', 'susa1030', 'susa@gmail.com', 'Susanne', 'Bastviken', 'Bastviken', '0001-01-01 00:00:00', 1);
        INSERT INTO "Users" ("Id", "UserName", "Password", "Email", "FirstName", "MiddleName", "LastName", "LastLoggin", "Active") VALUES (12, 'sofi', 'sofi1030', 'sofie@gmail.com', 'Sofie', 'Elliot', 'Lein', '0001-01-01 00:00:00', 1);
        INSERT INTO "Users" ("Id", "UserName", "Password", "Email", "FirstName", "MiddleName", "LastName", "LastLoggin", "Active") VALUES (13, 'juli', 'juli1030', 'juli@gmail.com', 'Julia', 'Lawrence', 'Corrs', '0001-01-01 00:00:00', 1);
        INSERT INTO "Users" ("Id", "UserName", "Password", "Email", "FirstName", "MiddleName", "LastName", "LastLoggin", "Active") VALUES (14, 'sabr', 'sabr1030', 'sabr@gmail.com', 'Sabrina', 'Lee', 'De Mayo', '0001-01-01 00:00:00', 1);
        INSERT INTO "Users" ("Id", "UserName", "Password", "Email", "FirstName", "MiddleName", "LastName", "LastLoggin", "Active") VALUES (2, 'hana', 'hana1030', 'palmer@gmail.com', 'Hannah', 'Palmer', 'Palmer', '0001-01-01 00:00:00', 1);
        INSERT INTO "Users" ("Id", "UserName", "Password", "Email", "FirstName", "MiddleName", "LastName", "LastLoggin", "Active") VALUES (3, 'soph', 'soph1030', 'sophie@gmail.com', 'Sophie', 'Mudd', 'Mudd', '0001-01-01 00:00:00', 1);
        INSERT INTO "Users" ("Id", "UserName", "Password", "Email", "FirstName", "MiddleName", "LastName", "LastLoggin", "Active") VALUES (4, 'john', 'john1030', 'john@gmail.com', 'John', 'Simoni', 'Smith', '0001-01-01 00:00:00', 1);
        INSERT INTO "Users" ("Id", "UserName", "Password", "Email", "FirstName", "MiddleName", "LastName", "LastLoggin", "Active") VALUES (5, 'geor', 'geor1030', 'george@gmail.com', 'George', 'Baily', 'Leyne', '0001-01-01 00:00:00', 1);
        INSERT INTO "Users" ("Id", "UserName", "Password", "Email", "FirstName", "MiddleName", "LastName", "LastLoggin", "Active") VALUES (6, 'patr', 'patr1030', 'patricia@gmail.com', 'Patricia', 'Teodoro', 'Demaso', '0001-01-01 00:00:00', 1);
        INSERT INTO "Users" ("Id", "UserName", "Password", "Email", "FirstName", "MiddleName", "LastName", "LastLoggin", "Active") VALUES (7, 'mich', 'mich1030', 'mich@gmail.com', 'Michael', 'Kapitan', 'Torres', '0001-01-01 00:00:00', 1);
        INSERT INTO "Users" ("Id", "UserName", "Password", "Email", "FirstName", "MiddleName", "LastName", "LastLoggin", "Active") VALUES (8, 'jose', 'jose1030', 'jose@gmail.com', 'Joseph', 'Gatchalian', 'Videla', '0001-01-01 00:00:00', 1);
        INSERT INTO "Users" ("Id", "UserName", "Password", "Email", "FirstName", "MiddleName", "LastName", "LastLoggin", "Active") VALUES (9, 'emma', 'emma1030', 'emma@gmail.com', 'Emma', 'Kotos', 'Smith', '0001-01-01 00:00:00', 1);
        
        -- Groups: 17 rows
        INSERT INTO "Groups" ("Id", "GroupName", "Description", "OwnerId", "Active") VALUES (1, 'Event-io Dev Team', 'Group of Dev 1', 4, 1);
        INSERT INTO "Groups" ("Id", "GroupName", "Description", "OwnerId", "Active") VALUES (10, 'Team 10', 'Team 10', 4, 1);
        INSERT INTO "Groups" ("Id", "GroupName", "Description", "OwnerId", "Active") VALUES (11, 'Team 12', 'Team 12', 4, 1);
        INSERT INTO "Groups" ("Id", "GroupName", "Description", "OwnerId", "Active") VALUES (12, 'Team 13', 'Team 13', 4, 1);
        INSERT INTO "Groups" ("Id", "GroupName", "Description", "OwnerId", "Active") VALUES (13, 'Team 14', 'Team 14', 4, 1);
        INSERT INTO "Groups" ("Id", "GroupName", "Description", "OwnerId", "Active") VALUES (14, 'Team 15', 'Team 15', 4, 1);
        INSERT INTO "Groups" ("Id", "GroupName", "Description", "OwnerId", "Active") VALUES (15, 'Team 16', 'Team 16', 4, 1);
        INSERT INTO "Groups" ("Id", "GroupName", "Description", "OwnerId", "Active") VALUES (16, 'Team 17', 'Team 17', 4, 1);
        INSERT INTO "Groups" ("Id", "GroupName", "Description", "OwnerId", "Active") VALUES (17, 'Team 18', 'Team 18', 4, 1);
        INSERT INTO "Groups" ("Id", "GroupName", "Description", "OwnerId", "Active") VALUES (2, 'Team 2', 'Team 2', 4, 1);
        INSERT INTO "Groups" ("Id", "GroupName", "Description", "OwnerId", "Active") VALUES (3, 'Team 3', 'Team 3', 4, 1);
        INSERT INTO "Groups" ("Id", "GroupName", "Description", "OwnerId", "Active") VALUES (4, 'Team 4', 'Team 4', 4, 1);
        INSERT INTO "Groups" ("Id", "GroupName", "Description", "OwnerId", "Active") VALUES (5, 'Team 5', 'Team 5', 4, 1);
        INSERT INTO "Groups" ("Id", "GroupName", "Description", "OwnerId", "Active") VALUES (6, 'Team 6', 'Team 6', 4, 1);
        INSERT INTO "Groups" ("Id", "GroupName", "Description", "OwnerId", "Active") VALUES (7, 'Team 7', 'Team 7', 4, 1);
        INSERT INTO "Groups" ("Id", "GroupName", "Description", "OwnerId", "Active") VALUES (8, 'Team 8', 'Team 8', 4, 1);
        INSERT INTO "Groups" ("Id", "GroupName", "Description", "OwnerId", "Active") VALUES (9, 'Team 9', 'Team 9', 4, 1);
        
        -- Appointments: 18 rows
        INSERT INTO "Appointments" ("Id", "Location", "Title", "Color", "Details", "YearMonth", "Date", "Time", "EndDateSpan", "EndTime", "CreatorId", "isRepeat", "RepeatSelection", "RepeatEnd", "After", "OnDate", "Type", "HasNoWeekEnds", "IsDone") VALUES (1, 'Apple premium reseller', 'Macbook pro applecare appointment', 'gray', 'In hac habitasse platea dictumst. Suspendisse laoreet porta sem non maximus. Pellentesque ante felis, feugiat sit amet congue sed, dignissim consectetur orci. Vestibulum a euismod felis, ac dapibus leo. Maecenas odio leo, imperdiet congue tortor a, aliquam tristique massa. Curabitur vehicula tellus ut est dictum pharetra. Nullam id arcu id justo faucibus ultrices id vitae dui. Integer luctus, ligula nec dignissim ornare, nisi erat pretium quam, sed dapibus justo justo sed nibh. Donec et quam arcu. In hac habitasse platea dictumst. Mauris at nulla dignissim velit elementum euismod', '11/2026', '2026-11-05 00:00:00', '11:00 AM', 0, '12:01 PM', 2, 0, 0, 0, 0, '0001-01-01 00:00:00', 0, 0, 0);
        INSERT INTO "Appointments" ("Id", "Location", "Title", "Color", "Details", "YearMonth", "Date", "Time", "EndDateSpan", "EndTime", "CreatorId", "isRepeat", "RepeatSelection", "RepeatEnd", "After", "OnDate", "Type", "HasNoWeekEnds", "IsDone") VALUES (10, '18Th Floor - 14 Room', 'Transition discussion', 'yellow', 'Viverra efficitur, ligula elit maximus arcu, eu dignissim tortor libero eget massa. Duis gravida consequat elit. Praesent non enim et nibh luctus vestibulum id eget ante. Nunc ullamcorper auctor sapien, eget pharetra urna volutpat aliquet. Donec est augue, suscipit at bibendum sed, porta ac ante. Vivamu', '11/2026', '2026-11-02 00:00:00', '9:30 AM', 0, '10:30 AM', 4, 0, 0, 0, 0, '0001-01-01 00:00:00', 0, 0, 0);
        INSERT INTO "Appointments" ("Id", "Location", "Title", "Color", "Details", "YearMonth", "Date", "Time", "EndDateSpan", "EndTime", "CreatorId", "isRepeat", "RepeatSelection", "RepeatEnd", "After", "OnDate", "Type", "HasNoWeekEnds", "IsDone") VALUES (11, '', 'Upper body', 'yellow', 'Use dumbbells and resistance band', '11/2026', '2026-11-08 00:00:00', '7:30 AM', 0, '8:30 AM', 4, 1, 2, 1, 30, '0001-01-01 00:00:00', 1, 0, 0);
        INSERT INTO "Appointments" ("Id", "Location", "Title", "Color", "Details", "YearMonth", "Date", "Time", "EndDateSpan", "EndTime", "CreatorId", "isRepeat", "RepeatSelection", "RepeatEnd", "After", "OnDate", "Type", "HasNoWeekEnds", "IsDone") VALUES (12, '', 'Lower body', 'yellow', 'Use dumbbells and resistance band', '11/2026', '2026-11-09 00:00:00', '7:30 AM', 0, '8:30 AM', 4, 1, 2, 1, 30, '0001-01-01 00:00:00', 1, 0, 0);
        INSERT INTO "Appointments" ("Id", "Location", "Title", "Color", "Details", "YearMonth", "Date", "Time", "EndDateSpan", "EndTime", "CreatorId", "isRepeat", "RepeatSelection", "RepeatEnd", "After", "OnDate", "Type", "HasNoWeekEnds", "IsDone") VALUES (13, '', 'Pay electric bill', 'orange', '', '11/2026', '2026-11-07 00:00:00', '9:30 AM', 0, '9:30 AM', 4, 0, 0, 0, 0, '0001-01-01 00:00:00', 2, 0, 0);
        INSERT INTO "Appointments" ("Id", "Location", "Title", "Color", "Details", "YearMonth", "Date", "Time", "EndDateSpan", "EndTime", "CreatorId", "isRepeat", "RepeatSelection", "RepeatEnd", "After", "OnDate", "Type", "HasNoWeekEnds", "IsDone") VALUES (14, '', 'Pay internet bill', 'orange', '', '11/2026', '2026-11-08 00:00:00', '10:00 AM', 0, '10:00 AM', 4, 0, 0, 0, 0, '0001-01-01 00:00:00', 2, 0, 0);
        INSERT INTO "Appointments" ("Id", "Location", "Title", "Color", "Details", "YearMonth", "Date", "Time", "EndDateSpan", "EndTime", "CreatorId", "isRepeat", "RepeatSelection", "RepeatEnd", "After", "OnDate", "Type", "HasNoWeekEnds", "IsDone") VALUES (15, '', 'Rest Day', 'green', '', '11/2026', '2026-11-10 00:00:00', '10:00 AM', 0, '10:00 AM', 4, 1, 2, 1, 30, '0001-01-01 00:00:00', 1, 0, 0);
        INSERT INTO "Appointments" ("Id", "Location", "Title", "Color", "Details", "YearMonth", "Date", "Time", "EndDateSpan", "EndTime", "CreatorId", "isRepeat", "RepeatSelection", "RepeatEnd", "After", "OnDate", "Type", "HasNoWeekEnds", "IsDone") VALUES (16, '', 'Family Reunion', 'gray', '', '11/2026', '2026-11-20 00:00:00', '9:00 AM', 0, '10:00 AM', 4, 0, 0, 0, 0, '0001-01-01 00:00:00', 2, 0, 0);
        INSERT INTO "Appointments" ("Id", "Location", "Title", "Color", "Details", "YearMonth", "Date", "Time", "EndDateSpan", "EndTime", "CreatorId", "isRepeat", "RepeatSelection", "RepeatEnd", "After", "OnDate", "Type", "HasNoWeekEnds", "IsDone") VALUES (17, '', 'Upper Legs', 'purple', 'Use dumbbells and resistance band', '11/2026', '2026-11-11 00:00:00', '7:30 AM', 0, '8:30 AM', 4, 1, 2, 1, 30, '0001-01-01 00:00:00', 1, 0, 0);
        INSERT INTO "Appointments" ("Id", "Location", "Title", "Color", "Details", "YearMonth", "Date", "Time", "EndDateSpan", "EndTime", "CreatorId", "isRepeat", "RepeatSelection", "RepeatEnd", "After", "OnDate", "Type", "HasNoWeekEnds", "IsDone") VALUES (18, '', 'Lower Legs', 'purple', 'Use dumbbells and resistance band', '11/2026', '2026-11-12 00:00:00', '7:30 AM', 0, '8:30 AM', 4, 1, 2, 1, 30, '0001-01-01 00:00:00', 1, 0, 0);
        INSERT INTO "Appointments" ("Id", "Location", "Title", "Color", "Details", "YearMonth", "Date", "Time", "EndDateSpan", "EndTime", "CreatorId", "isRepeat", "RepeatSelection", "RepeatEnd", "After", "OnDate", "Type", "HasNoWeekEnds", "IsDone") VALUES (2, 'https://zoom.us/j/5551112222', 'Discuss UI related changes', 'red', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque sed mattis velit, quis laoreet arcu. Sed convallis lacus libero, ac rutrum arcu sollicitudin sed.', '11/2026', '2026-11-02 00:00:00', '1:30 PM', 0, '2:30 PM', 4, 0, 0, 0, 0, '0001-01-01 00:00:00', 0, 0, 0);
        INSERT INTO "Appointments" ("Id", "Location", "Title", "Color", "Details", "YearMonth", "Date", "Time", "EndDateSpan", "EndTime", "CreatorId", "isRepeat", "RepeatSelection", "RepeatEnd", "After", "OnDate", "Type", "HasNoWeekEnds", "IsDone") VALUES (3, 'https://zoom.us/j/55511135232', 'Meeting with Business and Users', 'red', 'Praesent ac justo nisl. Aenean eu purus dictum, faucibus purus nec, sagittis nunc. Aliquam ultrices blandit sem vitae sollicitudin. In bibendum ligula eget condimentum ullamcorper. Nam ut ante sagittis, luctus odio et, aliquet leo. Phasellus eu felis aliquet, iaculis lectus ut, facilisis est. Duis sagittis quam a nulla pulvinar, ut luctus ligula dapibus. Duis consequat tortor dolor, sed dapibus nisl volutpat eget.  Etiam vitae vehicula urna, eu maximus nisi. Phasellus blandit, sapien a gravida elementum, lectus massa pellentesque ipsum, et fringilla lorem risus quis est. Cras sit amet dui sapien. In a felis sodales, mattis metus vitae, imperdiet ante. Sed elementum odio id fermentum rhoncus. Ut tincidunt bibendum ex feugiat interdum. Aliquam rhoncus dolor leo, ac sagittis enim dictum tincidunt.', '11/2026', '2026-11-10 00:00:00', '9:10 AM', 0, '10:10 AM', 4, 0, 0, 0, 0, '0001-01-01 00:00:00', 0, 0, 0);
        INSERT INTO "Appointments" ("Id", "Location", "Title", "Color", "Details", "YearMonth", "Date", "Time", "EndDateSpan", "EndTime", "CreatorId", "isRepeat", "RepeatSelection", "RepeatEnd", "After", "OnDate", "Type", "HasNoWeekEnds", "IsDone") VALUES (4, 'Pepper Lunch - Eastwood Cyber & Fashion Mall', 'Lunch Meeting', 'green', 'Aliquam cursus congue arcu at consequat. Suspendisse potenti. Nunc vestibulum, ante sit amet luctus porta, velit neque vehicula nunc, sed dapibus arcu dui id est. Fusce feugiat ligula id ullamcorper dapibus. Nam interdum justo metus, nec aliquet nisi mattis at. Phasellus aliquam interdum neque et malesuada. Pellentesque ac nisi sit amet lorem rutrum tristique nec sed lorem. Morbi euismod maximus ullamcorper. Pellentesque non laoreet justo. Pellentesque viverra nisi placerat urna facilisis, nec facilisis felis accumsan. Curabitur id turpis urna. Fusce elementum urna vitae leo pharetra iaculis. Sed nec nisl at eros tristique condimentum. Fusce ultricies dolor eu porta volutpat.', '11/2026', '2026-11-11 00:00:00', '12:10 PM', 0, '1:10 PM', 4, 0, 0, 0, 0, '0001-01-01 00:00:00', 0, 0, 0);
        INSERT INTO "Appointments" ("Id", "Location", "Title", "Color", "Details", "YearMonth", "Date", "Time", "EndDateSpan", "EndTime", "CreatorId", "isRepeat", "RepeatSelection", "RepeatEnd", "After", "OnDate", "Type", "HasNoWeekEnds", "IsDone") VALUES (5, 'KDR Adventure Camp, Orani, Bataan', 'Team Building', 'blue', 'Nullam eget sapien imperdiet, pretium neque in, facilisis ex. Nam quis mollis felis. Aenean malesuada lacinia nulla ut aliquet. Praesent ex purus, pharetra non euismod quis, facilisis quis tortor. Ut malesuada quis lacus id efficitur. Aliquam egestas tortor vel urna scelerisque gravida. Phasellus eget quam magna. Cras metus libero, convallis in mattis eget, feugiat ac ligula. Pellentesque feugiat eget ligula ac euismod.  Integer condimentum dolor quis lobortis ornare. Integer odio lorem, mattis id dapibus eget, facilisis et eros. Vivamus in tristique dolor. Morbi posuere faucibus tellus, eget venenatis urna consectetur et. Maecenas interdum neque et scelerisque egestas. Proin faucibus, ex ut varius tristique, arcu elit luctus ligula, eget hendrerit nibh dui sed ipsum. Donec gravida condimentum massa, ultricies dignissim risus vehicula quis. Morbi tortor arcu, luctus facilisis scelerisque varius, suscipit a ante. Praesent convallis velit nec lobortis venenatis.', '11/2026', '2026-11-17 00:00:00', '6:05 PM', 2, '7:05 PM', 4, 0, 0, 0, 0, '0001-01-01 00:00:00', 0, 0, 0);
        INSERT INTO "Appointments" ("Id", "Location", "Title", "Color", "Details", "YearMonth", "Date", "Time", "EndDateSpan", "EndTime", "CreatorId", "isRepeat", "RepeatSelection", "RepeatEnd", "After", "OnDate", "Type", "HasNoWeekEnds", "IsDone") VALUES (6, 'LockHeed Martin - 3Rd buiding.', 'Pest Control', 'red', 'Cras laoreet interdum fermentum. In gravida nulla eget lacus tincidunt interdum. In sed sapien sed neque rhoncus egestas maximus tincidunt tortor. Nullam ut leo eros. In vitae odio id ante consequat gravida et sed orci. Phasellus in aliquet enim, non sagittis massa. Curabitur sit amet maximus erat. Aliquam tempus nunc sit amet blandit rhoncus. Etiam at urna vitae neque eleifend condimentum in eu ante. Integer non justo feugiat, imperdiet tellus sit amet, porttitor lectus. Donec at tincidunt enim, at varius orci. Donec dapibus elit vel ullamcorper pretium. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Vestibulum id dolor nisl. Aenean et gravida quam. In ac arcu urna.', '11/2026', '2026-11-22 00:00:00', '6:00 PM', 2, '7:00 PM', 2, 0, 0, 0, 0, '0001-01-01 00:00:00', 0, 0, 0);
        INSERT INTO "Appointments" ("Id", "Location", "Title", "Color", "Details", "YearMonth", "Date", "Time", "EndDateSpan", "EndTime", "CreatorId", "isRepeat", "RepeatSelection", "RepeatEnd", "After", "OnDate", "Type", "HasNoWeekEnds", "IsDone") VALUES (7, 'https://zoom.us/j/5553232122', 'Deployment stratetegy', 'blue', 'turpis at, eleifend massa. Curabitur ac elit eros. Vivamus nulla ipsum, tristique sed ultricies a, fermentum et orci. Sed odio magna, posuere eget orci at, dictum finibus est. Nam eleifend nunc velit, et maximus mauris hendrerit at. Etiam aliquam et mauris a egestas. Aliquam nisl augue, efficitur ut dolor eu, volutpat dapibus eros. In fringilla ipsum ac blandit auctor. Pellentesque malesuada dui sit amet tincidunt aliquam. Sed eu tristique ligula, nec auctor leo.', '11/2026', '2026-11-28 00:00:00', '3:30 PM', 0, '4:30 PM', 4, 0, 0, 0, 0, '0001-01-01 00:00:00', 0, 0, 0);
        INSERT INTO "Appointments" ("Id", "Location", "Title", "Color", "Details", "YearMonth", "Date", "Time", "EndDateSpan", "EndTime", "CreatorId", "isRepeat", "RepeatSelection", "RepeatEnd", "After", "OnDate", "Type", "HasNoWeekEnds", "IsDone") VALUES (8, 'https://zoom.us/j/555113223523', 'Discuss team building', 'gray', 'efficitur vulputate mauris. Etiam convallis lacus mattis enim gravida dapibus. Cras ut mauris enim. Aliquam nec lorem risus. Interdum et malesuada fames ac ante ipsum primis in faucibus. Sed vitae placerat massa. Quisque pulvinar rutrum erat vitae fringilla. Nunc fringilla vulputate diam nec aliquet. Pellentesque sed facilisis tellus, eu varius neque. Suspendisse tincidunt vestibulum justo, a porta est maximus at. Nullam at laoreet est. Interdum', '11/2026', '2026-11-09 00:00:00', '2:00 PM', 0, '3:00 PM', 7, 0, 0, 0, 0, '0001-01-01 00:00:00', 0, 0, 0);
        INSERT INTO "Appointments" ("Id", "Location", "Title", "Color", "Details", "YearMonth", "Date", "Time", "EndDateSpan", "EndTime", "CreatorId", "isRepeat", "RepeatSelection", "RepeatEnd", "After", "OnDate", "Type", "HasNoWeekEnds", "IsDone") VALUES (9, '5th Floor - Resilient room', 'Onboarding Meeting', 'orange', 'Ut gravida a nisl in tincidunt. In pretium velit urna, quis placerat nisl lacinia a. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; Vestibulum ultrices enim eget elit fringilla eleifend. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse potenti. Donec egestas justo placerat lorem tincidunt volutpat.', '11/2026', '2026-11-05 00:00:00', '8:30 AM', 0, '9:30 AM', 4, 0, 0, 0, 0, '0001-01-01 00:00:00', 0, 0, 0);
        
        -- Contacts: 5 rows
        INSERT INTO "Contacts" ("UserId", "ContactId") VALUES (4, 10);
        INSERT INTO "Contacts" ("UserId", "ContactId") VALUES (4, 12);
        INSERT INTO "Contacts" ("UserId", "ContactId") VALUES (4, 13);
        INSERT INTO "Contacts" ("UserId", "ContactId") VALUES (4, 5);
        INSERT INTO "Contacts" ("UserId", "ContactId") VALUES (4, 8);
        
        -- Meetings: 30 rows
        INSERT INTO "Meetings" ("ParticipantId", "MeetingId") VALUES (2, 1);
        INSERT INTO "Meetings" ("ParticipantId", "MeetingId") VALUES (2, 10);
        INSERT INTO "Meetings" ("ParticipantId", "MeetingId") VALUES (2, 2);
        INSERT INTO "Meetings" ("ParticipantId", "MeetingId") VALUES (2, 3);
        INSERT INTO "Meetings" ("ParticipantId", "MeetingId") VALUES (2, 4);
        INSERT INTO "Meetings" ("ParticipantId", "MeetingId") VALUES (2, 5);
        INSERT INTO "Meetings" ("ParticipantId", "MeetingId") VALUES (2, 6);
        INSERT INTO "Meetings" ("ParticipantId", "MeetingId") VALUES (2, 7);
        INSERT INTO "Meetings" ("ParticipantId", "MeetingId") VALUES (2, 8);
        INSERT INTO "Meetings" ("ParticipantId", "MeetingId") VALUES (2, 9);
        INSERT INTO "Meetings" ("ParticipantId", "MeetingId") VALUES (4, 1);
        INSERT INTO "Meetings" ("ParticipantId", "MeetingId") VALUES (4, 10);
        INSERT INTO "Meetings" ("ParticipantId", "MeetingId") VALUES (4, 2);
        INSERT INTO "Meetings" ("ParticipantId", "MeetingId") VALUES (4, 3);
        INSERT INTO "Meetings" ("ParticipantId", "MeetingId") VALUES (4, 4);
        INSERT INTO "Meetings" ("ParticipantId", "MeetingId") VALUES (4, 5);
        INSERT INTO "Meetings" ("ParticipantId", "MeetingId") VALUES (4, 6);
        INSERT INTO "Meetings" ("ParticipantId", "MeetingId") VALUES (4, 7);
        INSERT INTO "Meetings" ("ParticipantId", "MeetingId") VALUES (4, 8);
        INSERT INTO "Meetings" ("ParticipantId", "MeetingId") VALUES (4, 9);
        INSERT INTO "Meetings" ("ParticipantId", "MeetingId") VALUES (7, 1);
        INSERT INTO "Meetings" ("ParticipantId", "MeetingId") VALUES (7, 10);
        INSERT INTO "Meetings" ("ParticipantId", "MeetingId") VALUES (7, 2);
        INSERT INTO "Meetings" ("ParticipantId", "MeetingId") VALUES (7, 3);
        INSERT INTO "Meetings" ("ParticipantId", "MeetingId") VALUES (7, 4);
        INSERT INTO "Meetings" ("ParticipantId", "MeetingId") VALUES (7, 5);
        INSERT INTO "Meetings" ("ParticipantId", "MeetingId") VALUES (7, 6);
        INSERT INTO "Meetings" ("ParticipantId", "MeetingId") VALUES (7, 7);
        INSERT INTO "Meetings" ("ParticipantId", "MeetingId") VALUES (7, 8);
        INSERT INTO "Meetings" ("ParticipantId", "MeetingId") VALUES (7, 9);
        
        -- GroupsMeetings: 0 rows
        
        -- UsersGroups: 3 rows
        INSERT INTO "UsersGroups" ("UserId", "GroupId") VALUES (1, 1);
        INSERT INTO "UsersGroups" ("UserId", "GroupId") VALUES (2, 1);
        INSERT INTO "UsersGroups" ("UserId", "GroupId") VALUES (3, 1);
        
        -- UserPictures: 0 rows
        
        -- RepeatEdits: 0 rows
        
        -- ExceptionLogs: 0 rows
        
        -- Restore the original AUTOINCREMENT high-water marks, including deleted IDs.
        DELETE FROM sqlite_sequence;
        INSERT INTO sqlite_sequence (name, seq) VALUES ('Appointments', 18);
        INSERT INTO sqlite_sequence (name, seq) VALUES ('Groups', 17);
        INSERT INTO sqlite_sequence (name, seq) VALUES ('Users', 14);
        """";
}
