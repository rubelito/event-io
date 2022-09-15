using System.Reflection;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
using Scheduler.Entity;

public class SchedulerDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserPicture> UserPictures { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<RepeatEdit> RepeatEdits { get; set; }
    public DbSet<Group> Groups { get; set; }

    public DbSet<UserContact> Contacts { get; set; }
    public DbSet<UserSchedule> Meetings { get; set; }
    public DbSet<GroupSchedule> GroupsMeetings { get; set; }
    public DbSet<UserGroup> UsersGroups { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string DbPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        optionsBuilder.UseSqlite(@"Data Source=" + DbPath + @"/Db/testDatabase.db", options =>
        {
            options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
        });
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Appointment>().HasKey(e => e.Id);
        modelBuilder.Entity<RepeatEdit>().HasKey(e => e.Id);
        modelBuilder.Entity<User>().HasKey(e => e.Id);
        modelBuilder.Entity<UserPicture>().HasKey(e => e.UserId);
        modelBuilder.Entity<Group>().HasKey(e => e.Id);

        modelBuilder.Entity<UserContact>().HasKey(uc => new { uc.UserId, uc.ContactId });
        modelBuilder.Entity<UserSchedule>().HasKey(us => new { us.ParticipantId, us.MeetingId });
        modelBuilder.Entity<GroupSchedule>().HasKey(gs => new { gs.ParticipantId, gs.MeetingId });
        modelBuilder.Entity<UserGroup>().HasKey(ug => new { ug.UserId, ug.GroupId });

        //Relationships

        //UserPicture one to one or zero
        modelBuilder.Entity<User>()
            .HasOne(u => u.Picture)
            .WithOne(p => p.User)
            .HasForeignKey<UserPicture>(e => e.UserId);

        modelBuilder.Entity<Appointment>()
            .HasOne(r => r.Creator)
            .WithMany(a => a.Appointments);

        modelBuilder.Entity<Appointment>()
            .HasMany(a => a.GroupsParticipants);

        //UserContact Many to Many
        modelBuilder.Entity<UserContact>()
            .HasOne<User>(uc => uc.Contact)
            .WithMany(w => w.ContactsOf)
            .HasForeignKey(f => f.ContactId);

        modelBuilder.Entity<UserContact>()
            .HasOne<User>(uc => uc.User)
            .WithMany(w => w.Contacts)
            .HasForeignKey(f => f.UserId);

        ///UserGroup Many to Many
        modelBuilder.Entity<UserGroup>()
            .HasOne<User>(u => u.User)
            .WithMany(w => w.Groups)
            .HasForeignKey(f => f.UserId);

        modelBuilder.Entity<UserGroup>()
            .HasOne<Group>(u => u.Group)
            .WithMany(w => w.Members)
            .HasForeignKey(f => f.GroupId);

        ////UserSchedule Many to Many
        modelBuilder.Entity<UserSchedule>()
            .HasOne<User>(u => u.Participant)
            .WithMany(w => w.Meetings)
            .HasForeignKey(f => f.ParticipantId);

        modelBuilder.Entity<UserSchedule>()
            .HasOne<Appointment>(a => a.Meeting)
            .WithMany(w => w.Meetings)
            .HasForeignKey(f => f.MeetingId);

        ///GroupSchedule Many to Many
        modelBuilder.Entity<GroupSchedule>()
            .HasOne<Group>(g => g.Participant)
            .WithMany(w => w.Meetings)
            .HasForeignKey(f => f.ParticipantId);

        modelBuilder.Entity<GroupSchedule>()
            .HasOne<Appointment>(a => a.Meeting)
            .WithMany(w => w.GroupsParticipants)
            .HasForeignKey(f => f.MeetingId);
    }
}