using System.Globalization;
using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;
using Scheduler.Entity;
using Scheduler.Services;

namespace Scheduler.Test;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        Assert.Pass();
    }

    [Test]
    public void SeedDate()
    {
        CreateUser();
        AddMoreUser();
        AddMoreContacts();
        AddGroups();
        CreateEvents();
        AddTaskAndReminders();
        //AddRepeats();
        //AddRepeatEdits();

        Assert.Pass();
    }

    public void CreateUser()
    {
        //var con = new MySqlConnection("server=127.0.0.1;uid=root;pwd=root1030;database=Scheduler");
        //var dbContext = new SchedulerDbContext(con, false);
        //dbContext.Database.CreateIfNotExists();
        //con.Open();

        var dbContext = new SchedulerDbContext();
        dbContext.Database.EnsureCreated();

        User u = new User();
        u.UserName = "rube";
        u.FirstName = "Rubelito";
        u.LastName = "Isiderio";
        u.MiddleName = "Reyes";
        u.Email = "rube@gmail.com";
        u.Password = "rube1030";
        u.Active = true;

        User u1 = new User();
        u1.UserName = "hana";
        u1.FirstName = "Hannah";
        u1.LastName = "Palmer";
        u1.MiddleName = "Palmer";
        u1.Email = "palmer@gmail.com";
        u1.Password = "hana1030";
        u1.Active = true;

        User u2 = new User();
        u2.UserName = "soph";
        u2.FirstName = "Sophie";
        u2.LastName = "Mudd";
        u2.MiddleName = "Mudd";
        u2.Email = "sophie@gmail.com";
        u2.Password = "soph1030";
        u2.Active = true;

        User u3 = new User();
        u3.UserName = "john";
        u3.FirstName = "John";
        u3.LastName = "Smith";
        u3.MiddleName = "Simoni";
        u3.Email = "john@gmail.com";
        u3.Password = "john1030";
        u3.Active = true;

        dbContext.Users.Add(u);
        dbContext.Users.Add(u1);
        dbContext.Users.Add(u2);
        dbContext.Users.Add(u3);
        dbContext.SaveChanges();

        Group g = new Group();
        g.GroupName = "Event-io Dev Team";
        g.Description = "Group of Dev 1";
        g.Active = true;
        g.Owner = u3;
        
        dbContext.Groups.Add(g);
        dbContext.SaveChanges();

        UserGroup ug = new UserGroup();
        ug.User = u;
        ug.Group = g;
        
        dbContext.UsersGroups.Add(ug);
        dbContext.SaveChanges();

        //con.Close();
    }

    public void AddMoreUser()
    {
        //var con = new MySqlConnection("server=127.0.0.1;uid=root;pwd=root1030;database=Scheduler");
        //var dbContext = new SchedulerDbContext(con, false);
        //dbContext.Database.CreateIfNotExists();
        //con.Open();

        var dbContext = new SchedulerDbContext();

        User u = new User();
        u.UserName = "geor";
        u.FirstName = "George";
        u.LastName = "Leyne";
        u.MiddleName = "Baily";
        u.Email = "george@gmail.com";
        u.Password = "geor1030";
        u.Active = true;

        User u1 = new User();
        u1.UserName = "patr";
        u1.FirstName = "Patricia";
        u1.LastName = "Demaso";
        u1.MiddleName = "Teodoro";
        u1.Email = "patricia@gmail.com";
        u1.Password = "patr1030";
        u1.Active = true;

        User u2 = new User();
        u2.UserName = "mich";
        u2.FirstName = "Michael";
        u2.LastName = "Torres";
        u2.MiddleName = "Kapitan";
        u2.Email = "mich@gmail.com";
        u2.Password = "mich1030";
        u2.Active = true;

        User u3 = new User();
        u3.UserName = "jose";
        u3.FirstName = "Joseph";
        u3.LastName = "Videla";
        u3.MiddleName = "Gatchalian";
        u3.Email = "jose@gmail.com";
        u3.Password = "jose1030";
        u3.Active = true;

        User u4 = new User();
        u4.UserName = "emma";
        u4.FirstName = "Emma";
        u4.LastName = "Smith";
        u4.MiddleName = "Kotos";
        u4.Email = "emma@gmail.com";
        u4.Password = "emma1030";
        u4.Active = true;

        User u5 = new User();
        u5.UserName = "jasm";
        u5.FirstName = "Jasmin";
        u5.LastName = "Lees";
        u5.MiddleName = "Lees";
        u5.Email = "jasm@gmail.com";
        u5.Password = "jasm1030";
        u5.Active = true;

        User u6 = new User();
        u6.UserName = "susa";
        u6.FirstName = "Susanne";
        u6.LastName = "Bastviken";
        u6.MiddleName = "Bastviken";
        u6.Email = "susa@gmail.com";
        u6.Password = "susa1030";
        u6.Active = true;

        User u7 = new User();
        u7.UserName = "sofi";
        u7.FirstName = "Sofie";
        u7.LastName = "Lein";
        u7.MiddleName = "Elliot";
        u7.Email = "sofie@gmail.com";
        u7.Password = "sofi1030";
        u7.Active = true;

        User u8 = new User();
        u8.UserName = "juli";
        u8.FirstName = "Julia";
        u8.LastName = "Corrs";
        u8.MiddleName = "Lawrence";
        u8.Email = "juli@gmail.com";
        u8.Password = "juli1030";
        u8.Active = true;

        User u9 = new User();
        u9.UserName = "sabr";
        u9.FirstName = "Sabrina";
        u9.LastName = "De Mayo";
        u9.MiddleName = "Lee";
        u9.Email = "sabr@gmail.com";
        u9.Password = "sabr1030";
        u9.Active = true;

        dbContext.Users.Add(u);
        dbContext.Users.Add(u1);
        dbContext.Users.Add(u2);
        dbContext.Users.Add(u3);
        dbContext.Users.Add(u4);
        dbContext.Users.Add(u5);
        dbContext.Users.Add(u6);
        dbContext.Users.Add(u7);
        dbContext.Users.Add(u8);
        dbContext.Users.Add(u9);
        dbContext.SaveChanges();

        //con.Close();
    }

    public void AddMoreContacts()
    {
        //var con = new MySqlConnection("server=127.0.0.1;uid=root;pwd=root1030;database=Scheduler");
        //var dbContext = new SchedulerDbContext(con, false);
        //dbContext.Database.CreateIfNotExists();
        //con.Open();

        var dbContext = new SchedulerDbContext();

        var user = dbContext.Users.FirstOrDefault(u => u.UserName == "john");
        var user1 = dbContext.Users.FirstOrDefault(u => u.UserName == "sofi");
        var user2 = dbContext.Users.FirstOrDefault(u => u.UserName == "juli");
        var user3 = dbContext.Users.FirstOrDefault(u => u.UserName == "jasm");
        var user4 = dbContext.Users.FirstOrDefault(u => u.UserName == "jose");
        var user5 = dbContext.Users.FirstOrDefault(u => u.UserName == "geor");

        UserContact uc = new UserContact();
        uc.UserId = user.Id;
        uc.ContactId = user1.Id;

        UserContact uc1 = new UserContact();
        uc1.UserId = user.Id;
        uc1.ContactId = user2.Id;

        UserContact uc2 = new UserContact();
        uc2.UserId = user.Id;
        uc2.ContactId = user3.Id;

        UserContact uc3 = new UserContact();
        uc3.UserId = user.Id;
        uc3.ContactId = user4.Id;

        UserContact uc4 = new UserContact();
        uc4.UserId = user.Id;
        uc4.ContactId = user5.Id;

        dbContext.Contacts.Add(uc);
        dbContext.Contacts.Add(uc1);
        dbContext.Contacts.Add(uc2);
        dbContext.Contacts.Add(uc3);
        dbContext.Contacts.Add(uc4);

        dbContext.SaveChanges();

        //con.Close();
    }

    public void AddGroups()
    {
        //var con = new MySqlConnection("server=127.0.0.1;uid=root;pwd=root1030;database=Scheduler");
        //var dbContext = new SchedulerDbContext(con, false);

        var dbContext = new SchedulerDbContext();

        var user = dbContext.Users.FirstOrDefault(u => u.UserName == "john");


        Group g = new Group();
        g.GroupName = "Team 1";
        g.Description = "Team 1";
        g.Active = true;
        g.Owner = user;

        Group g1 = new Group();
        g1.GroupName = "Team 2";
        g1.Description = "Team 2";
        g1.Active = true;
        g1.Owner = user;

        Group g2 = new Group();
        g2.GroupName = "Team 3";
        g2.Description = "Team 3";
        g2.Active = true;
        g2.Owner = user;

        Group g3 = new Group();
        g3.GroupName = "Team 4";
        g3.Description = "Team 4";
        g3.Active = true;
        g3.Owner = user;

        Group g4 = new Group();
        g4.GroupName = "Team 5";
        g4.Description = "Team 5";
        g4.Active = true;
        g4.Owner = user;

        Group g5 = new Group();
        g5.GroupName = "Team 6";
        g5.Description = "Team 6";
        g5.Active = true;
        g5.Owner = user;

        Group g6 = new Group();
        g6.GroupName = "Team 7";
        g6.Description = "Team 7";
        g6.Active = true;
        g6.Owner = user;

        Group g7 = new Group();
        g7.GroupName = "Team 8";
        g7.Description = "Team 8";
        g7.Active = true;
        g7.Owner = user;

        Group g8 = new Group();
        g8.GroupName = "Team 9";
        g8.Description = "Team 9";
        g8.Active = true;
        g8.Owner = user;

        Group g9 = new Group();
        g9.GroupName = "Team 10";
        g9.Description = "Team 10";
        g9.Active = true;
        g9.Owner = user;

        Group g10 = new Group();
        g10.GroupName = "Team 12";
        g10.Description = "Team 12";
        g10.Active = true;
        g10.Owner = user;

        Group g11 = new Group();
        g11.GroupName = "Team 13";
        g11.Description = "Team 13";
        g11.Active = true;
        g11.Owner = user;

        Group g12 = new Group();
        g12.GroupName = "Team 14";
        g12.Description = "Team 14";
        g12.Active = true;
        g12.Owner = user;

        Group g13 = new Group();
        g13.GroupName = "Team 15";
        g13.Description = "Team 15";
        g13.Active = true;
        g13.Owner = user;

        Group g14 = new Group();
        g14.GroupName = "Team 16";
        g14.Description = "Team 16";
        g14.Active = true;
        g14.Owner = user;

        Group g15 = new Group();
        g15.GroupName = "Team 17";
        g15.Description = "Team 17";
        g15.Active = true;
        g15.Owner = user;

        Group g16 = new Group();
        g16.GroupName = "Team 18";
        g16.Description = "Team 18";
        g16.Active = true;
        g16.Owner = user;

        dbContext.Groups.Add(g1);
        dbContext.Groups.Add(g2);
        dbContext.Groups.Add(g3);
        dbContext.Groups.Add(g4);
        dbContext.Groups.Add(g5);
        dbContext.Groups.Add(g6);
        dbContext.Groups.Add(g7);
        dbContext.Groups.Add(g8);
        dbContext.Groups.Add(g9);
        dbContext.Groups.Add(g10);
        dbContext.Groups.Add(g11);
        dbContext.Groups.Add(g12);
        dbContext.Groups.Add(g13);
        dbContext.Groups.Add(g14);
        dbContext.Groups.Add(g15);
        dbContext.Groups.Add(g16);

        dbContext.SaveChanges();
        //con.Close();
    }

    public void CreateEvents()
    {
        var dbContext = new SchedulerDbContext();

        Appointment ap0 = new Appointment();
        ap0.Location = "Apple premium reseller";
        ap0.Title = "Macbook pro applecare appointment";
        ap0.Color = "gray";
        ap0.Details = "In hac habitasse platea dictumst. Suspendisse laoreet porta sem non maximus. Pellentesque ante felis, feugiat sit amet congue sed, dignissim consectetur orci. Vestibulum a euismod felis, ac dapibus leo. Maecenas odio leo, imperdiet congue tortor a, aliquam tristique massa. Curabitur vehicula tellus ut est dictum pharetra. Nullam id arcu id justo faucibus ultrices id vitae dui. Integer luctus, ligula nec dignissim ornare, nisi erat pretium quam, sed dapibus justo justo sed nibh. Donec et quam arcu. In hac habitasse platea dictumst. Mauris at nulla dignissim velit elementum euismod";
        ap0.YearMonth = "9/2022";
        ap0.Date = DateTime.ParseExact("09/03/2022", "MM/dd/yyyy", CultureInfo.InvariantCulture);
        ap0.Time = "11:00 AM";
        ap0.CreatorId = 2;

        UserSchedule us01 = new UserSchedule();
        us01.Meeting = ap0;
        us01.ParticipantId = 4;

        UserSchedule us02 = new UserSchedule();
        us02.Meeting = ap0;
        us02.ParticipantId = 7;

        UserSchedule us03 = new UserSchedule();
        us03.Meeting = ap0;
        us03.ParticipantId = 2;

        dbContext.Meetings.Add(us01);
        dbContext.Meetings.Add(us02);
        dbContext.Meetings.Add(us03);

        dbContext.Appointments.Add(ap0);
        dbContext.SaveChanges();

        Appointment ap1 = new Appointment();
        ap1.Location = "https://zoom.us/j/5551112222";
        ap1.Title = "Discuss UI related changes";
        ap1.Color = "yellow";
        ap1.Details = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque sed mattis velit, quis laoreet arcu. Sed convallis lacus libero, ac rutrum arcu sollicitudin sed.";
        ap1.YearMonth = "9/2022";
        ap1.Date = DateTime.ParseExact("09/05/2022", "MM/dd/yyyy", CultureInfo.InvariantCulture);
        ap1.Time = "1:30 PM";
        ap1.CreatorId = 4;

        UserSchedule us1 = new UserSchedule();
        us1.Meeting = ap1;
        us1.ParticipantId = 4;

        UserSchedule us2 = new UserSchedule();
        us2.Meeting = ap1;
        us2.ParticipantId = 7;

        UserSchedule us3 = new UserSchedule();
        us3.Meeting = ap1;
        us3.ParticipantId = 2;

        dbContext.Meetings.Add(us1);
        dbContext.Meetings.Add(us2);
        dbContext.Meetings.Add(us3);

        dbContext.Appointments.Add(ap1);
        dbContext.SaveChanges();


        Appointment ap2 = new Appointment();
        ap2.Location = "https://zoom.us/j/55511135232";
        ap2.Title = "Meetig with Business and Users";
        ap2.Color = "red";
        ap2.Details = "Praesent ac justo nisl. Aenean eu purus dictum, faucibus purus nec, sagittis nunc. Aliquam ultrices blandit sem vitae sollicitudin. In bibendum ligula eget condimentum ullamcorper. Nam ut ante sagittis, luctus odio et, aliquet leo. Phasellus eu felis aliquet, iaculis lectus ut, facilisis est. Duis sagittis quam a nulla pulvinar, ut luctus ligula dapibus. Duis consequat tortor dolor, sed dapibus nisl volutpat eget.  Etiam vitae vehicula urna, eu maximus nisi. Phasellus blandit, sapien a gravida elementum, lectus massa pellentesque ipsum, et fringilla lorem risus quis est. Cras sit amet dui sapien. In a felis sodales, mattis metus vitae, imperdiet ante. Sed elementum odio id fermentum rhoncus. Ut tincidunt bibendum ex feugiat interdum. Aliquam rhoncus dolor leo, ac sagittis enim dictum tincidunt.";
        ap2.YearMonth = "9/2022";
        ap2.Date = DateTime.ParseExact("09/08/2022", "MM/dd/yyyy", CultureInfo.InvariantCulture);
        ap2.Time = "9:10 AM";
        ap2.CreatorId = 4;

        UserSchedule us6 = new UserSchedule();
        us6.Meeting = ap2;
        us6.ParticipantId = 4;

        UserSchedule us4 = new UserSchedule();
        us4.Meeting = ap2;
        us4.ParticipantId = 7;

        UserSchedule us5 = new UserSchedule();
        us5.Meeting = ap2;
        us5.ParticipantId = 2;

        dbContext.Meetings.Add(us6);
        dbContext.Meetings.Add(us4);
        dbContext.Meetings.Add(us5);

        dbContext.Appointments.Add(ap2);
        dbContext.SaveChanges();

        Appointment ap3 = new Appointment();
        ap3.Location = "Pepper Lunch - Eastwood Cyber & Fashion Mall";
        ap3.Title = "Lunch Meeting";
        ap3.Color = "green";
        ap3.Details = "Aliquam cursus congue arcu at consequat. Suspendisse potenti. Nunc vestibulum, ante sit amet luctus porta, velit neque vehicula nunc, sed dapibus arcu dui id est. Fusce feugiat ligula id ullamcorper dapibus. Nam interdum justo metus, nec aliquet nisi mattis at. Phasellus aliquam interdum neque et malesuada. Pellentesque ac nisi sit amet lorem rutrum tristique nec sed lorem. Morbi euismod maximus ullamcorper. Pellentesque non laoreet justo. Pellentesque viverra nisi placerat urna facilisis, nec facilisis felis accumsan. Curabitur id turpis urna. Fusce elementum urna vitae leo pharetra iaculis. Sed nec nisl at eros tristique condimentum. Fusce ultricies dolor eu porta volutpat.";
        ap3.YearMonth = "9/2022";
        ap3.Date = DateTime.ParseExact("09/09/2022", "MM/dd/yyyy", CultureInfo.InvariantCulture);
        ap3.Time = "12:10 PM";
        ap3.CreatorId = 4;

        UserSchedule us7 = new UserSchedule();
        us7.Meeting = ap3;
        us7.ParticipantId = 4;

        UserSchedule us8 = new UserSchedule();
        us8.Meeting = ap3;
        us8.ParticipantId = 7;

        UserSchedule us9 = new UserSchedule();
        us9.Meeting = ap3;
        us9.ParticipantId = 2;

        dbContext.Meetings.Add(us7);
        dbContext.Meetings.Add(us8);
        dbContext.Meetings.Add(us9);

        dbContext.Appointments.Add(ap3);
        dbContext.SaveChanges();

        Appointment ap4 = new Appointment();
        ap4.Location = "Katsu Ora - Eastwood Mall, Orchard Rd, Bagumbayan, Quezon City, 1110 Metro Manila";
        ap4.Title = "Dinner Meeting";
        ap4.Color = "blue";
        ap4.Details = "Nullam eget sapien imperdiet, pretium neque in, facilisis ex. Nam quis mollis felis. Aenean malesuada lacinia nulla ut aliquet. Praesent ex purus, pharetra non euismod quis, facilisis quis tortor. Ut malesuada quis lacus id efficitur. Aliquam egestas tortor vel urna scelerisque gravida. Phasellus eget quam magna. Cras metus libero, convallis in mattis eget, feugiat ac ligula. Pellentesque feugiat eget ligula ac euismod.  Integer condimentum dolor quis lobortis ornare. Integer odio lorem, mattis id dapibus eget, facilisis et eros. Vivamus in tristique dolor. Morbi posuere faucibus tellus, eget venenatis urna consectetur et. Maecenas interdum neque et scelerisque egestas. Proin faucibus, ex ut varius tristique, arcu elit luctus ligula, eget hendrerit nibh dui sed ipsum. Donec gravida condimentum massa, ultricies dignissim risus vehicula quis. Morbi tortor arcu, luctus facilisis scelerisque varius, suscipit a ante. Praesent convallis velit nec lobortis venenatis.";
        ap4.YearMonth = "9/2022";
        ap4.Date = DateTime.ParseExact("09/09/2022", "MM/dd/yyyy", CultureInfo.InvariantCulture);
        ap4.Time = "6:05 PM";
        ap4.CreatorId = 4;

        UserSchedule us10 = new UserSchedule();
        us10.Meeting = ap4;
        us10.ParticipantId = 4;

        UserSchedule us11 = new UserSchedule();
        us11.Meeting = ap4;
        us11.ParticipantId = 7;

        UserSchedule us12 = new UserSchedule();
        us12.Meeting = ap4;
        us12.ParticipantId = 2;

        dbContext.Meetings.Add(us10);
        dbContext.Meetings.Add(us11);
        dbContext.Meetings.Add(us12);

        dbContext.Appointments.Add(ap4);
        dbContext.SaveChanges();

        Appointment ap5 = new Appointment();
        ap5.Location = "LockHeed Martin - 3Rd buiding.";
        ap5.Title = "Pest Control";
        ap5.Color = "red";
        ap5.Details = "Cras laoreet interdum fermentum. In gravida nulla eget lacus tincidunt interdum. In sed sapien sed neque rhoncus egestas maximus tincidunt tortor. Nullam ut leo eros. In vitae odio id ante consequat gravida et sed orci. Phasellus in aliquet enim, non sagittis massa. Curabitur sit amet maximus erat. Aliquam tempus nunc sit amet blandit rhoncus. Etiam at urna vitae neque eleifend condimentum in eu ante. Integer non justo feugiat, imperdiet tellus sit amet, porttitor lectus. Donec at tincidunt enim, at varius orci. Donec dapibus elit vel ullamcorper pretium. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Vestibulum id dolor nisl. Aenean et gravida quam. In ac arcu urna.";
        ap5.YearMonth = "9/2022";
        ap5.Date = DateTime.ParseExact("09/20/2022", "MM/dd/yyyy", CultureInfo.InvariantCulture);
        ap5.Time = "6:00 PM";
        ap5.CreatorId = 2;

        UserSchedule us13 = new UserSchedule();
        us13.Meeting = ap5;
        us13.ParticipantId = 4;

        UserSchedule us14 = new UserSchedule();
        us14.Meeting = ap5;
        us14.ParticipantId = 7;

        UserSchedule us15 = new UserSchedule();
        us15.Meeting = ap5;
        us15.ParticipantId = 2;

        dbContext.Meetings.Add(us13);
        dbContext.Meetings.Add(us14);
        dbContext.Meetings.Add(us15);

        dbContext.Appointments.Add(ap5);
        dbContext.SaveChanges();

        Appointment ap6 = new Appointment();
        ap6.Location = "https://zoom.us/j/5553232122";
        ap6.Title = "Deployment stratetegy";
        ap6.Color = "blue";
        ap6.Details = "turpis at, eleifend massa. Curabitur ac elit eros. Vivamus nulla ipsum, tristique sed ultricies a, fermentum et orci. Sed odio magna, posuere eget orci at, dictum finibus est. Nam eleifend nunc velit, et maximus mauris hendrerit at. Etiam aliquam et mauris a egestas. Aliquam nisl augue, efficitur ut dolor eu, volutpat dapibus eros. In fringilla ipsum ac blandit auctor. Pellentesque malesuada dui sit amet tincidunt aliquam. Sed eu tristique ligula, nec auctor leo.";
        ap6.YearMonth = "9/2022";
        ap6.Date = DateTime.ParseExact("09/26/2022", "MM/dd/yyyy", CultureInfo.InvariantCulture);
        ap6.Time = "3:30 PM";
        ap6.CreatorId = 4;

        UserSchedule us16 = new UserSchedule();
        us16.Meeting = ap6;
        us16.ParticipantId = 4;

        UserSchedule us17 = new UserSchedule();
        us17.Meeting = ap6;
        us17.ParticipantId = 7;

        UserSchedule us18 = new UserSchedule();
        us18.Meeting = ap6;
        us18.ParticipantId = 2;

        dbContext.Meetings.Add(us16);
        dbContext.Meetings.Add(us17);
        dbContext.Meetings.Add(us18);

        dbContext.Appointments.Add(ap6);
        dbContext.SaveChanges();

        Appointment ap7 = new Appointment();
        ap7.Location = "https://zoom.us/j/555113223523";
        ap7.Title = "Team Building - 1st meeting";
        ap7.Color = "gray";
        ap7.Details = "efficitur vulputate mauris. Etiam convallis lacus mattis enim gravida dapibus. Cras ut mauris enim. Aliquam nec lorem risus. Interdum et malesuada fames ac ante ipsum primis in faucibus. Sed vitae placerat massa. Quisque pulvinar rutrum erat vitae fringilla. Nunc fringilla vulputate diam nec aliquet. Pellentesque sed facilisis tellus, eu varius neque. Suspendisse tincidunt vestibulum justo, a porta est maximus at. Nullam at laoreet est. Interdum";
        ap7.YearMonth = "9/2022";
        ap7.Date = DateTime.ParseExact("09/30/2022", "MM/dd/yyyy", CultureInfo.InvariantCulture);
        ap7.Time = "2:00 PM";
        ap7.CreatorId = 7;

        UserSchedule us19 = new UserSchedule();
        us19.Meeting = ap7;
        us19.ParticipantId = 4;

        UserSchedule us20 = new UserSchedule();
        us20.Meeting = ap7;
        us20.ParticipantId = 7;

        UserSchedule us21 = new UserSchedule();
        us21.Meeting = ap7;
        us21.ParticipantId = 2;

        dbContext.Meetings.Add(us19);
        dbContext.Meetings.Add(us20);
        dbContext.Meetings.Add(us21);

        dbContext.Appointments.Add(ap7);
        dbContext.SaveChanges();


        Appointment ap8 = new Appointment();
        ap8.Location = "5th Floor - Resilient room";
        ap8.Title = "Onboarding Meeting";
        ap8.Color = "orange";
        ap8.Details = "Ut gravida a nisl in tincidunt. In pretium velit urna, quis placerat nisl lacinia a. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; Vestibulum ultrices enim eget elit fringilla eleifend. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse potenti. Donec egestas justo placerat lorem tincidunt volutpat.";
        ap8.YearMonth = "8/2022";
        ap8.Date = DateTime.ParseExact("08/17/2022", "MM/dd/yyyy", CultureInfo.InvariantCulture);
        ap8.Time = "8:30 AM";
        ap8.CreatorId = 4;

        UserSchedule us22 = new UserSchedule();
        us22.Meeting = ap8;
        us22.ParticipantId = 4;

        UserSchedule us23 = new UserSchedule();
        us23.Meeting = ap8;
        us23.ParticipantId = 7;

        UserSchedule us24 = new UserSchedule();
        us24.Meeting = ap8;
        us24.ParticipantId = 2;

        dbContext.Meetings.Add(us22);
        dbContext.Meetings.Add(us23);
        dbContext.Meetings.Add(us24);

        dbContext.Appointments.Add(ap8);
        dbContext.SaveChanges();


        Appointment ap9 = new Appointment();
        ap9.Location = "18Th Floor - 14 Room";
        ap9.Title = "Transition discussion";
        ap9.Color = "yellow";
        ap9.Details = "Viverra efficitur, ligula elit maximus arcu, eu dignissim tortor libero eget massa. Duis gravida consequat elit. Praesent non enim et nibh luctus vestibulum id eget ante. Nunc ullamcorper auctor sapien, eget pharetra urna volutpat aliquet. Donec est augue, suscipit at bibendum sed, porta ac ante. Vivamu";
        ap9.YearMonth = "10/2022";
        ap9.Date = DateTime.ParseExact("10/05/2022", "MM/dd/yyyy", CultureInfo.InvariantCulture);
        ap9.Time = "9:30 AM";
        ap9.CreatorId = 4;

        UserSchedule us25 = new UserSchedule();
        us25.Meeting = ap9;
        us25.ParticipantId = 4;

        UserSchedule us26 = new UserSchedule();
        us26.Meeting = ap9;
        us26.ParticipantId = 7;

        UserSchedule us27 = new UserSchedule();
        us27.Meeting = ap9;
        us27.ParticipantId = 2;

        dbContext.Meetings.Add(us25);
        dbContext.Meetings.Add(us26);
        dbContext.Meetings.Add(us27);

        dbContext.Appointments.Add(ap9);
        dbContext.SaveChanges();
    }

    [Test]
    public void AddTaskAndReminders()
    {
        var dbContext = new SchedulerDbContext();

        var task1 = new Appointment();
        task1.Title = "Upper body";
        task1.Color = "yellow";
        task1.Details = "Use dumbbells and resistance band";
        task1.Date = DateTime.ParseExact("09/05/2022", "MM/dd/yyyy", CultureInfo.InvariantCulture);
        task1.YearMonth = "9/2022";
        task1.Time = "7:30 AM";
        task1.isRepeat = true;
        task1.RepeatSelection = RepeatSelectionEnum.EveryWeek;
        task1.RepeatEnd = RepeatEndEnum.After;
        task1.After = 30;
        task1.CreatorId = 4;
        task1.Type = AppointmentType.Task;

        dbContext.Appointments.Add(task1);
        dbContext.SaveChanges();

        var task2 = new Appointment();
        task2.Title = "Lower body";
        task2.Color = "yellow";
        task2.Details = "Use dumbbells and resistance band";
        task2.Date = DateTime.ParseExact("09/06/2022", "MM/dd/yyyy", CultureInfo.InvariantCulture);
        task2.YearMonth = "9/2022";
        task2.Time = "7:30 AM";
        task2.isRepeat = true;
        task2.RepeatSelection = RepeatSelectionEnum.EveryWeek;
        task2.RepeatEnd = RepeatEndEnum.After;
        task2.After = 30;
        task2.CreatorId = 4;
        task2.Type = AppointmentType.Task;

        dbContext.Appointments.Add(task2);
        dbContext.SaveChanges();

        var reminder1 = new Appointment();
        reminder1.Title = "Pay electric bill";
        reminder1.Color = "orange";
        reminder1.Date = DateTime.ParseExact("09/05/2022", "MM/dd/yyyy", CultureInfo.InvariantCulture);
        reminder1.YearMonth = "9/2022";
        reminder1.Time = "9:30 AM";
        reminder1.isRepeat = false;
        reminder1.CreatorId = 4;
        reminder1.Type = AppointmentType.Reminder;

        dbContext.Appointments.Add(reminder1);
        dbContext.SaveChanges();

        var reminder2 = new Appointment();
        reminder2.Title = "Pay internel bill";
        reminder2.Color = "orange";
        reminder2.Date = DateTime.ParseExact("09/06/2022", "MM/dd/yyyy", CultureInfo.InvariantCulture);
        reminder2.YearMonth = "9/2022";
        reminder2.Time = "10:00 AM";
        reminder2.isRepeat = false;
        reminder2.CreatorId = 4;
        reminder2.Type = AppointmentType.Reminder;

        dbContext.Appointments.Add(reminder2);
        dbContext.SaveChanges();

        var reminder3 = new Appointment();
        reminder3.Title = "Rest Day";
        reminder3.Color = "green";
        reminder3.Date = DateTime.ParseExact("09/07/2022", "MM/dd/yyyy", CultureInfo.InvariantCulture);
        reminder3.YearMonth = "9/2022";
        reminder3.Time = "10:00 AM";
        reminder3.isRepeat = true;
        reminder3.RepeatSelection = RepeatSelectionEnum.EveryWeek;
        reminder3.RepeatEnd = RepeatEndEnum.After;
        reminder3.After = 30;
        reminder3.CreatorId = 4;
        reminder3.Type = AppointmentType.Task;

        dbContext.Appointments.Add(reminder3);
        dbContext.SaveChanges();

        var task3 = new Appointment();
        task3.Title = "Upper Legs";
        task3.Color = "purple";
        task3.Details = "Use dumbbells and resistance band";
        task3.Date = DateTime.ParseExact("09/08/2022", "MM/dd/yyyy", CultureInfo.InvariantCulture);
        task3.YearMonth = "9/2022";
        task3.Time = "7:30 AM";
        task3.isRepeat = true;
        task3.RepeatSelection = RepeatSelectionEnum.EveryWeek;
        task3.RepeatEnd = RepeatEndEnum.After;
        task3.After = 30;
        task3.CreatorId = 4;
        task3.Type = AppointmentType.Task;

        dbContext.Appointments.Add(task3);
        dbContext.SaveChanges();

        var task4 = new Appointment();
        task4.Title = "Lower Legs";
        task4.Color = "purple";
        task4.Details = "Use dumbbells and resistance band";
        task4.Date = DateTime.ParseExact("09/09/2022", "MM/dd/yyyy", CultureInfo.InvariantCulture);
        task4.YearMonth = "9/2022";
        task4.Time = "7:30 AM";
        task4.isRepeat = true;
        task4.RepeatSelection = RepeatSelectionEnum.EveryWeek;
        task4.RepeatEnd = RepeatEndEnum.After;
        task4.After = 30;
        task4.CreatorId = 4;
        task4.Type = AppointmentType.Task;

        dbContext.Appointments.Add(task4);
        dbContext.SaveChanges();

    }

    [Test]
    public void AddRepeats()
    {
        var dbContext = new SchedulerDbContext();
        //Repeat Daily Never
        Appointment ap0 = new Appointment();
        ap0.Location = "Home";
        ap0.Title = "Excercise - Back posture";
        ap0.Color = "red";
        ap0.Details = "Do some 2 minutes back posture";
        ap0.YearMonth = "9/2022";
        ap0.Date = DateTime.ParseExact("09/08/2022", "MM/dd/yyyy", CultureInfo.InvariantCulture);
        ap0.Time = "7:30 PM";
        ap0.isRepeat = true;
        ap0.RepeatSelection = RepeatSelectionEnum.EveryDay;
        ap0.RepeatEnd = RepeatEndEnum.Never;
        ap0.CreatorId = 4;

        dbContext.Appointments.Add(ap0);
        dbContext.SaveChanges();

        //Repeat Daily OnDate
        Appointment ap1 = new Appointment();
        ap1.Location = "Home";
        ap1.Title = "Jogging - 1 mile";
        ap1.Color = "gray";
        ap1.Details = "Do some 1 mile run";
        ap1.YearMonth = "9/2022";
        ap1.Date = DateTime.ParseExact("09/09/2022", "MM/dd/yyyy", CultureInfo.InvariantCulture);
        ap1.Time = "6:30 PM";
        ap1.isRepeat = true;
        ap1.RepeatSelection = RepeatSelectionEnum.EveryDay;
        ap1.RepeatEnd = RepeatEndEnum.OnDate;
        ap1.OnDate = DateTime.ParseExact("12/14/2022", "MM/dd/yyyy", CultureInfo.InvariantCulture);
        ap1.CreatorId = 4;

        dbContext.Appointments.Add(ap1);
        dbContext.SaveChanges();

        //Repeat Daily After 35
        Appointment ap2 = new Appointment();
        ap2.Location = "Home";
        ap2.Title = "Postre SQL course";
        ap2.Color = "yellow";
        ap2.Details = "Postre SQL product presentation";
        ap2.YearMonth = "9/2022";
        ap2.Date = DateTime.ParseExact("09/22/2022", "MM/dd/yyyy", CultureInfo.InvariantCulture);
        ap2.Time = "2:30 PM";
        ap2.isRepeat = true;
        ap2.RepeatSelection = RepeatSelectionEnum.EveryDay;
        ap2.RepeatEnd = RepeatEndEnum.After;
        ap2.After = 35;
        ap2.CreatorId = 4;

        dbContext.Appointments.Add(ap2);
        dbContext.SaveChanges();

        //Repeat Weekly After 35
        Appointment ap3 = new Appointment();
        ap3.Location = "Home Office";
        ap3.Title = "Weekly status report";
        ap3.Color = "red";
        ap3.Details = "Status of our software development project";
        ap3.YearMonth = "9/2022";
        ap3.Date = DateTime.ParseExact("09/05/2022", "MM/dd/yyyy", CultureInfo.InvariantCulture);
        ap3.Time = "11:00 AM";
        ap3.isRepeat = true;
        ap3.RepeatSelection = RepeatSelectionEnum.EveryWeek;
        ap3.RepeatEnd = RepeatEndEnum.After;
        ap3.After = 12;
        ap3.CreatorId = 4;

        dbContext.Appointments.Add(ap3);
        dbContext.SaveChanges();

        //Repeat Weekly OnDate
        Appointment ap6 = new Appointment();
        ap6.Location = "Home Office";
        ap6.Title = "Meet security";
        ap6.Color = "green";
        ap6.Details = "Meeet and greet the security team";
        ap6.YearMonth = "9/2022";
        ap6.Date = DateTime.ParseExact("09/02/2022", "MM/dd/yyyy", CultureInfo.InvariantCulture);
        ap6.Time = "8:00 AM";
        ap6.isRepeat = true;
        ap6.RepeatSelection = RepeatSelectionEnum.EveryWeek;
        ap6.RepeatEnd = RepeatEndEnum.OnDate;
        ap6.OnDate = DateTime.ParseExact("09/02/2023", "MM/dd/yyyy", CultureInfo.InvariantCulture);
        ap6.CreatorId = 4;

        dbContext.Appointments.Add(ap6);
        dbContext.SaveChanges();

        //Repeat Monthly onDate
        Appointment ap4 = new Appointment();
        ap4.Location = "Office";
        ap4.Title = "Montly status meeting";
        ap4.Color = "gray";
        ap4.Details = "Status of our software development project";
        ap4.YearMonth = "9/2022";
        ap4.Date = DateTime.ParseExact("09/02/2022", "MM/dd/yyyy", CultureInfo.InvariantCulture);
        ap4.Time = "11:10 AM";
        ap4.isRepeat = true;
        ap4.RepeatSelection = RepeatSelectionEnum.EveryMonth;
        ap4.RepeatEnd = RepeatEndEnum.OnDate;
        ap4.OnDate = DateTime.ParseExact("06/05/2023", "MM/dd/yyyy", CultureInfo.InvariantCulture); ;
        ap4.CreatorId = 4;

        dbContext.Appointments.Add(ap4);
        dbContext.SaveChanges();

        //Repeat Monthly After
        Appointment ap5 = new Appointment();
        ap5.Location = "Office";
        ap5.Title = "Monthly Lunch meeting";
        ap5.Color = "orange";
        ap5.Details = "greetings and meetups";
        ap5.YearMonth = "9/2022";
        ap5.Date = DateTime.ParseExact("09/02/2022", "MM/dd/yyyy", CultureInfo.InvariantCulture);
        ap5.Time = "12:01 PM";
        ap5.isRepeat = true;
        ap5.RepeatSelection = RepeatSelectionEnum.EveryMonth;
        ap5.RepeatEnd = RepeatEndEnum.After;
        ap5.After = 13;
        ap5.CreatorId = 4;

        dbContext.Appointments.Add(ap5);
        dbContext.SaveChanges();

        //Repeat Yearly onDate
        Appointment ap7 = new Appointment();
        ap7.Location = "Home";
        ap7.Title = "Yearly Insurance Update";
        ap7.Color = "green";
        ap7.Details = "Update my life insurance and SSS";
        ap7.YearMonth = "10/2022";
        ap7.Date = DateTime.ParseExact("10/18/2022", "MM/dd/yyyy", CultureInfo.InvariantCulture);
        ap7.Time = "12:01 PM";
        ap7.isRepeat = true;
        ap7.RepeatSelection = RepeatSelectionEnum.EveryYear;
        ap7.RepeatEnd = RepeatEndEnum.OnDate;
        ap7.OnDate = DateTime.ParseExact("10/19/2025", "MM/dd/yyyy", CultureInfo.InvariantCulture);
        ap7.CreatorId = 4;

        dbContext.Appointments.Add(ap7);
        dbContext.SaveChanges();

        //Repeat Yearly After
        Appointment ap8 = new Appointment();
        ap8.Location = "Office";
        ap8.Title = "Yearly performance evaluation";
        ap8.Color = "purple";
        ap8.Details = "My performance evaluation";
        ap8.YearMonth = "10/2022";
        ap8.Date = DateTime.ParseExact("10/19/2022", "MM/dd/yyyy", CultureInfo.InvariantCulture);
        ap8.Time = "12:01 PM";
        ap8.isRepeat = true;
        ap8.RepeatSelection = RepeatSelectionEnum.EveryYear;
        ap8.RepeatEnd = RepeatEndEnum.After;
        ap8.After = 5;
        ap8.CreatorId = 4;

        dbContext.Appointments.Add(ap8);
        dbContext.SaveChanges();
    }

    [Test]
    public void AddRepeatEdits()
    {
        var dbContext = new SchedulerDbContext();

        //Weekly Repeat change
        var parentRepeat1 = dbContext.Appointments.Where(a => a.isRepeat == true && a.Title == "Weekly status report").FirstOrDefault();

        var edit1 = new RepeatEdit();
        edit1.AppointmentId = parentRepeat1.Id;
        edit1.OriginalDate = DateTime.ParseExact("09/12/2022", "MM/dd/yyyy", CultureInfo.InvariantCulture);
        edit1.EditedDate = DateTime.ParseExact("09/13/2022", "MM/dd/yyyy", CultureInfo.InvariantCulture);
        edit1.Title = parentRepeat1.Title + " (moved)";
        edit1.Location = "Home Office";
        edit1.Details = parentRepeat1.Details + " - moved due to flood";
        edit1.Time = "11:30 AM";

        dbContext.RepeatEdits.Add(edit1);
        dbContext.SaveChanges();


        //Monthly Repeat change
        var parentRepeat2 = dbContext.Appointments.Where(a => a.isRepeat == true && a.Title == "Monthly Lunch meeting").FirstOrDefault();

        var edit2 = new RepeatEdit();
        edit2.AppointmentId = parentRepeat2.Id;
        edit2.OriginalDate = DateTime.ParseExact("11/02/2022", "MM/dd/yyyy", CultureInfo.InvariantCulture);
        edit2.EditedDate = DateTime.ParseExact("11/04/2022", "MM/dd/yyyy", CultureInfo.InvariantCulture);
        edit2.Title = parentRepeat2.Title + " (moved)";
        edit2.Location = "Office";
        edit2.Details = parentRepeat2.Details + " - moved due to changed restaurant.";
        edit2.Time = "12:10 PM";

        dbContext.RepeatEdits.Add(edit2);
        dbContext.SaveChanges();
    }

    [Test]
    public void GetContacts()
    {
        //var con = new MySqlConnection("server=127.0.0.1;uid=root;pwd=root1030;database=Scheduler");
        //var dbContext = new SchedulerDbContext(con, false);

        var dbContext = new SchedulerDbContext();

        UserRepository userRepo = new UserRepository(dbContext);
        var users = userRepo.GetContacts(1);
        //con.Close();
    }

    [Test]
    public void AddUserToGroup()
    {
        //var con = new MySqlConnection("server=127.0.0.1;uid=root;pwd=root1030;database=Scheduler");
        //var dbContext = new SchedulerDbContext(con, false);

        var dbContext = new SchedulerDbContext();

        GroupRepository groupRepo = new GroupRepository(dbContext);
        groupRepo.AddUser(1, 2);
        groupRepo.AddUser(1, 3);
        groupRepo.Dispose();
    }

    [Test]
    public void SearchUserByGroup()
    {
        //var con = new MySqlConnection("server=127.0.0.1;uid=root;pwd=root1030;database=Scheduler");
        //var dbContext = new SchedulerDbContext(con, false);

        var dbContext = new SchedulerDbContext();

        GroupRepository groupRepo = new GroupRepository(dbContext);
        var users = groupRepo.GetUsersInGroup(1);

        Assert.AreEqual(3, users.Count());

        //con.Close();
    }

    [Test]
    public void RemoveUserToGroup() {
        //var con = new MySqlConnection("server=127.0.0.1;uid=root;pwd=root1030;database=Scheduler");
        //var dbContext = new SchedulerDbContext(con, false);

        var dbContext = new SchedulerDbContext();

        GroupRepository groupRepo = new GroupRepository(dbContext);
        groupRepo.RemoveUser(1, 1);
        groupRepo.Dispose();

    }

    [Test]
    public void CreateGroup()
    {
        //var con = new MySqlConnection("server=127.0.0.1;uid=root;pwd=root1030;database=Scheduler");
        //var dbContext = new SchedulerDbContext(con, false);

        var dbContext = new SchedulerDbContext();

        GroupRepository groupRepository = new GroupRepository(dbContext);


        var user = dbContext.Users.FirstOrDefault(u => u.Id == 2);

        var newGroup = new Group();

        newGroup.GroupName = "Front end team";
        newGroup.Description = "2nd dev team on project";
        newGroup.Owner = user;


        groupRepository.CreateGroup(newGroup);
    }

    [Test]
    public void EditGroup()
    {
        //var con = new MySqlConnection("server=127.0.0.1;uid=root;pwd=root1030;database=Scheduler");
        //var dbContext = new SchedulerDbContext(con, false);

        var dbContext = new SchedulerDbContext();

        GroupRepository groupRepository = new GroupRepository(dbContext);


        var editGroup = new Group();

        editGroup.Id = 2;
        editGroup.GroupName = "Front end team babes";
        editGroup.Description = "2nd dev team babes on project";

        groupRepository.EditGroup(editGroup);
    }

    [Test]
    public void DeleteGroup()
    {
        //var con = new MySqlConnection("server=127.0.0.1;uid=root;pwd=root1030;database=Scheduler");
        //var dbContext = new SchedulerDbContext(con, false);

        var dbContext = new SchedulerDbContext();

        GroupRepository groupRepository = new GroupRepository(dbContext);
        groupRepository.DeleteGroup(2);
    }

    [Test]
    public void GetGroupListWithMembers()
    {
        //var con = new MySqlConnection("server=127.0.0.1;uid=root;pwd=root1030;database=Scheduler");
        //var dbContext = new SchedulerDbContext(con, false);

        var dbContext = new SchedulerDbContext();

        GroupRepository groupRepository = new GroupRepository(dbContext);
        var result = groupRepository.GetGroupListWithMembers();
    }
}
