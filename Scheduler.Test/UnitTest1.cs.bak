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
    public void SeedData()
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

        dbContext.Users.Add(u);
        dbContext.Users.Add(u1);
        dbContext.Users.Add(u2);
        dbContext.SaveChanges();

        Group g = new Group();
        g.GroupName = "Island Team";
        g.Description = "Group of Dev 1";
        g.Active = true;
        g.Owner = u;
        
        dbContext.Groups.Add(g);
        dbContext.SaveChanges();

        UserGroup ug = new UserGroup();
        ug.User = u;
        ug.Group = g;
        
        dbContext.UsersGroups.Add(ug);
        dbContext.SaveChanges();

        //con.Close();
    }

    [Test]
    public void AddMoreUser()
    {
        //var con = new MySqlConnection("server=127.0.0.1;uid=root;pwd=root1030;database=Scheduler");
        //var dbContext = new SchedulerDbContext(con, false);
        //dbContext.Database.CreateIfNotExists();
        //con.Open();

        var dbContext = new SchedulerDbContext();

        User u = new User();
        u.UserName = "anna";
        u.FirstName = "Anna";
        u.LastName = "Leyne";
        u.MiddleName = "Leyne";
        u.Email = "anna@gmail.com";
        u.Password = "anna1030";
        u.Active = true;

        User u1 = new User();
        u1.UserName = "patr";
        u1.FirstName = "Patricia";
        u1.LastName = "Teodoro";
        u1.MiddleName = "Teodoro";
        u1.Email = "patricia@gmail.com";
        u1.Password = "patr1030";
        u1.Active = true;

        User u2 = new User();
        u2.UserName = "reyn";
        u2.FirstName = "Reynalyn";
        u2.LastName = "Torres";
        u2.MiddleName = "Torres";
        u2.Email = "reyn@gmail.com";
        u2.Password = "reyn1030";
        u2.Active = true;

        User u3 = new User();
        u3.UserName = "chan";
        u3.FirstName = "Chantal";
        u3.LastName = "Videla";
        u3.MiddleName = "Videla";
        u3.Email = "chan@gmail.com";
        u3.Password = "chan1030";
        u3.Active = true;

        User u4 = new User();
        u4.UserName = "emma";
        u4.FirstName = "Emma";
        u4.LastName = "Kotos";
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
        u7.MiddleName = "Lein";
        u7.Email = "sofie@gmail.com";
        u7.Password = "sofi1030";
        u7.Active = true;

        User u8 = new User();
        u8.UserName = "juli";
        u8.FirstName = "Julia";
        u8.LastName = "Lawrence";
        u8.MiddleName = "Lawrence";
        u8.Email = "juli@gmail.com";
        u8.Password = "juli1030";
        u8.Active = true;

        User u9 = new User();
        u9.UserName = "sabr";
        u9.FirstName = "Sabrina";
        u9.LastName = "Lynn";
        u9.MiddleName = "Lynn";
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

    [Test]
    public void AddMoreContacts()
    {
        //var con = new MySqlConnection("server=127.0.0.1;uid=root;pwd=root1030;database=Scheduler");
        //var dbContext = new SchedulerDbContext(con, false);
        //dbContext.Database.CreateIfNotExists();
        //con.Open();

        var dbContext = new SchedulerDbContext();

        var user = dbContext.Users.FirstOrDefault(u => u.UserName == "rube");
        var user1 = dbContext.Users.FirstOrDefault(u => u.UserName == "chan");
        var user2 = dbContext.Users.FirstOrDefault(u => u.UserName == "juli");
        var user3 = dbContext.Users.FirstOrDefault(u => u.UserName == "sabr");
        var user4 = dbContext.Users.FirstOrDefault(u => u.UserName == "emma");
        var user5 = dbContext.Users.FirstOrDefault(u => u.UserName == "hana");

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

    [Test]
    public void AddGroups()
    {
        //var con = new MySqlConnection("server=127.0.0.1;uid=root;pwd=root1030;database=Scheduler");
        //var dbContext = new SchedulerDbContext(con, false);

        var dbContext = new SchedulerDbContext();

        var user = dbContext.Users.FirstOrDefault(u => u.UserName == "rube");


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
