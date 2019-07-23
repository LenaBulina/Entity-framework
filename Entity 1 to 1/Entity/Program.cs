using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    class Program
    {
        static void Main(string[] args)
        {
            using (UserContext db = new UserContext())
            {
                Man man = new Man { Name = "Test" };
                db.Men.Add(man);

                db.SaveChanges();

                Person person = new Person { Name = "T", ManId = man.Id };
                db.People.Add(person);
                db.SaveChanges();
                
                foreach(Person p in db.People.Include("Man"))
                {
                    Console.WriteLine("  Person Name : {0}  Man Name: {1}",
                             p.Name, p.Man.Name);
                }
                User user1 = new User { Login = "login1", Password = "pass1234" };
                User user2 = new User { Login = "login2", Password = "5678word2" };
                db.Users.AddRange(new List<User> { user1, user2 });
                db.SaveChanges();
                UserProfile profile1 = new UserProfile { Id = user1.Id, Age = 22, Name = "Tom" };
                UserProfile profile2 = new UserProfile { Id = user2.Id, Age = 27, Name = "Alice" };
                db.UserProfiles.AddRange(new List<UserProfile> { profile1, profile2 });
                db.SaveChanges();

                foreach (User user in db.Users)
                    Console.WriteLine("  Login: {0}  Password: {1}",
                             user.Login, user.Password);
            }
        }
    }
}
