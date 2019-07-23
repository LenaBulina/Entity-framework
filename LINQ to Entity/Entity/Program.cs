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
            UnionIntersectExcept();

        }
        static void SimpleLINQ()
        {
            using (SoccerContext db = new SoccerContext())
            {
                var players = from p in db.Players
                             where p.TeamId == 1
                             select p;
                foreach (var player in players)
                {
                    Console.WriteLine(player.Name);
                }
            }
        }
        static void LINQExtensions()
        {
            using (SoccerContext db = new SoccerContext())
            {
                var players = db.Players.Where(p => p.TeamId == 1);
            }
        }
        static void LINQToEntityObject()
        {
            using (SoccerContext db = new SoccerContext())
            {
                var players = db.Players.Where(p => p.Age < 30).ToList().Where(p => p.Id < 10);
            }
        }
        static void FindFirst()
        {
            using (SoccerContext db = new SoccerContext())
            {
                Player player = db.Players.Find(2); // выберем элемент с id=2
                Console.WriteLine(player);
                player = db.Players.FirstOrDefault(p => p.Id == 3);
                if (player != null)
                    Console.WriteLine(player.Name);
            }

        }
        static void Select()
        {
            using (SoccerContext db = new SoccerContext())
            {
                var players = db.Players.Select(p => new
                {
                    Name = p.Name,
                    Position = p.Position,
                    Age = p.Age,
                    CommandName = p.Team.Name
                });
                foreach (var p in players)
                    Console.WriteLine("{0} ({1}) - {2} ... {3} years", p.Name,p.CommandName, p.Position,p.Age);
            }
        }
        static void OrderBy()
        {
            using (SoccerContext db = new SoccerContext())
            {
                var players = db.Players.OrderBy(p => p.Name);
                foreach (Player p in players)
                    Console.WriteLine("{0}.{1} - {2}", p.Id, p.Name, p.Position);
                players = db.Players.OrderByDescending(p => p.Name);

                Console.WriteLine("___________________________________");

                foreach (Player p in players)
                    Console.WriteLine("{0}.{1} - {2}", p.Id, p.Name, p.Position);

                var PlayerCollection = db.Players.Select(p => new {Id = p.Id, Name = p.Name, Position = p.Position}).OrderBy(p => p.Name).ThenBy(p => p.Position);
                foreach (var p in PlayerCollection)
                    Console.WriteLine("{0}.{1} - {2}", p.Id, p.Name, p.Position);

            }
        }
        static void Join()
        {
            using (SoccerContext db = new SoccerContext())
            {
                //var players = from p in db.Players
                //             join c in db.Teams on p.TeamId equals c.Id
                //             select new { Name = p.Name, Team = c.Name, Position = p.Position };


                var players = db.Players.Join(db.Teams, // вторая таблица для соединения
                    x => x.TeamId, // свойство из первой таблицы для соединения
                    c => c.Id, // свойство из второй таблицы для соединения
                    (p, c) => new // новый результативный объект
            {
                        Name = p.Name,
                        Team = c.Name,
                        Position = p.Position
                    });
                foreach (var p in players)
                    Console.WriteLine("{0} ({1}) - {2}", p.Name, p.Team, p.Position);
            }
        }
        static void Group()
        {
            using (SoccerContext db = new SoccerContext())
            {
                var groups = from p in db.Players
                             group p by p.Team.Name;
                foreach (var g in groups)
                {
                    Console.WriteLine(g.Key);
                    foreach (var p in g)
                        Console.WriteLine("{0} - {1}", p.Name, p.Position);
                    Console.WriteLine();
                }
                //var groups = db.Players.GroupBy(p => p.Team.Name);


                var groups2 = from p in db.Players
                             group p by p.Team.Name into g
                             select new { Name = g.Key, Count = g.Count() };
                // альтернативный способ
                //var groups2 = db.Players.GroupBy(p=>p.Team.Name)
                //                  .Select(g => new { Name = g.Key, Count = g.Count()});
                foreach (var c in groups2)
                    Console.WriteLine("Team: {0} Count of players: {1}", c.Name, c.Count);

            }
        }

        static void UnionIntersectExcept()
        {
            using (SoccerContext db = new SoccerContext())
            {
                var players = db.Players.Where(p => p.Id < 10)
                    .Union(db.Players.Where(p => p.Team.Name.Contains("Basr")));
                foreach (var p in players)
                    Console.WriteLine(p.Name);
                Console.WriteLine("\n_______________________________\n");
                var players2 = db.Players.Where(p => p.Id < 10)
                .Intersect(db.Players.Where(p => p.Team.Name.Contains("Basr")));
                foreach (var p in players2)
                    Console.WriteLine(p.Name);
                Console.WriteLine("\n_______________________________\n");
                var except1 = db.Players.Where(p => p.Id < 10); 
                var except2 = db.Players.Where(p => p.Team.Name.Contains("Basr")); 
                var players3 = except1.Except(except2); 

                foreach (var p in players3)
                    Console.WriteLine(p.Name);

            }
        }
        static void AggregationOperatorsSQL()
        {
            using (SoccerContext db = new SoccerContext())
            {
                int number1 = db.Players.Count();
                
                int number2 = db.Players.Count(p => p.Team.Name.Contains("Basr"));

                Console.WriteLine("Count of players : {0}",number1);
                Console.WriteLine("Count of Barcelona players : {0}",number2);

               
                int minId = db.Players.Min(p => p.Id);
               
                int maxId = db.Players.Max(p => p.Id);
               
                double avgAge = db.Players.Where(p => p.Team.Name == "Basr")
                                    .Average(p => p.Age);

                Console.WriteLine("Min ID : {0}",minId);
                Console.WriteLine("Max ID : {0}",maxId);
                Console.WriteLine("Average age : {0}",avgAge);

                
                int ageSum = db.Players.Sum(p => p.Age);
                // суммарная цена всех моделей фирмы Samsung
                int ageSumBar = db.Players.Where(p => p.Team.Name.Contains("Basr"))
                                    .Sum(p => p.Age);
                Console.WriteLine("Age sum : {0}", ageSum);
                Console.WriteLine("Age sum of Barcelona : {0}",ageSumBar);
            }
        }

        static void AsNoTracking()
        {
            using (SoccerContext db = new SoccerContext())
            {
                Player firstPlayer = db.Players.FirstOrDefault();
                firstPlayer.Name = "Test";
                //db.SaveChanges();

                List<Player> players = db.Players.ToList();

                //Player firstPlayer = db.Players.AsNoTracking().FirstOrDefault();
                //firstPlayer.Name = "Test";
                ////db.SaveChanges();

                //List<Player> players = db.Players.AsNoTracking().ToList();
            }
        }
        static void Add()
        {
            using (SoccerContext db = new SoccerContext())
            {
                // создание и добавление моделей
                Team t1 = new Team { Name = "Барселона" };
                Team t2 = new Team { Name = "Реал Мадрид" };
                db.Teams.Add(t1);
                db.Teams.Add(t2);
                db.SaveChanges();
                Player pl1 = new Player { Name = "Роналду", Age = 31, Position = "Нападающий", Team = t2 };
                Player pl2 = new Player { Name = "Месси", Age = 28, Position = "Нападающий", Team = t1 };
                Player pl3 = new Player { Name = "Хави", Age = 34, Position = "Полузащитник", Team = t1 };
                db.Players.AddRange(new List<Player> { pl1, pl2, pl3 });
                db.SaveChanges();

                // вывод 
                foreach (Player pl in db.Players.Include("Team"))
                    Console.WriteLine("{0} - {1}", pl.Name, pl.Team != null ? pl.Team.Name : "");
                Console.WriteLine();
                foreach (Team t in db.Teams)
                {
                    Console.WriteLine("Команда: {0}", t.Name);
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
                Player player = db.Players.Where(p => p.Name == "Месси").FirstOrDefault<Player>();
                player.Name = "Месси №1";
                db.SaveChanges();
                foreach (Team t in db.Teams.Include("Players"))
                {
                    Console.WriteLine("Команда: {0}", t.Name);
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
