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
            Add();
            //Change();
        }
        static void Add()
        {
            using (SoccerContext db = new SoccerContext())
            {
                // создание и добавление моделей
                Team t1 = new Team { Name = "Basr" };
                Team t2 = new Team { Name = "Real" };
                db.Teams.Add(t1);
                db.Teams.Add(t2);
                db.SaveChanges();
                Player pl1 = new Player { Name = "Ronal", Age = 31, Position = "Forward", Team = t2 };
                Player pl2 = new Player { Name = "Messi", Age = 28, Position = "Forward", Team = t1 };
                Player pl3 = new Player { Name = "Havi", Age = 34, Position = "Defence", Team = t1 };
                db.Players.AddRange(new List<Player> { pl1, pl2, pl3 });
                db.SaveChanges();

                // вывод 
                foreach (Player pl in db.Players)
                    Console.WriteLine("{0} - {1}", pl.Name, pl.Team != null ? pl.Team.Name : "");
                Console.WriteLine();
                foreach (Team t in db.Teams)
                {
                    Console.WriteLine("Team: {0}", t.Name);
                    foreach (Player pl in t.Players)
                    {
                        Console.WriteLine("{0} - {1}", pl.Name, pl.Position);
                    }
                    Console.WriteLine();
                }
            }
        }
        static void Change()
        {
            using (SoccerContext db = new SoccerContext())
            {
                //Player player = null;
                //foreach(Player p in db.Players)
                //{
                //    if(p.Name == "Messi")
                //    {
                //        player = p;
                //        break;
                //    }
                //}
                //if (player != null)
                //{
                //    player.Name = "Messi #1";
                //    db.SaveChanges();
                //}
                //Player player = db.Players.Where(p => p.Name == "Messi").First<Player>();
                //player.Name = "Messi №1";
                //db.SaveChanges();
                foreach (Team t in db.Teams.Include("Players"))
                {
                    Console.WriteLine("Team: {0}", t.Name);
                    foreach (Player pl in t.Players)
                    {
                        Console.WriteLine("{0} - {1}", pl.Name, pl.Position);
                    }
                    Console.WriteLine();
                }
                foreach (Team t in db.Teams)
                {
                    Console.WriteLine("Team: {0}", t.Name);
                    foreach (Player pl in t.Players)
                    {
                        Console.WriteLine("{0} - {1}", pl.Name, pl.Position);
                    }
                    Console.WriteLine();
                }
            }
        }
        static void Delete()
        {
            using (SoccerContext db = new SoccerContext())
            {
                db.Database.ExecuteSqlCommand("ALTER TABLE dbo.Players ADD CONSTRAINT Players_Teams FOREIGN KEY (TeamId) REFERENCES dbo.Teams (Id) ON DELETE SET NULL");
                //удаление игрока
                Player pl_toDelete = db.Players.First(p => p.Name == "Роналду");
                db.Players.Remove(pl_toDelete);
                // удаление команды     
                Team t_toDelete = db.Teams.First();
                db.Teams.Remove(t_toDelete);
                db.SaveChanges();
            }
        }
    }
}
