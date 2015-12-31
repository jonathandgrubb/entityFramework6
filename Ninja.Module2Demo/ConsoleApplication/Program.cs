using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NinjaDomain.Classes;
using NinjaDomain.DataModel;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer(new NullDatabaseInitializer<NinjaContext>()); // stop EF from going through its DB init process when its working with NinjaContext
            //InsertClan();
            //InsertNinja();
            InsertMultipleNinjas();
            SimpleNinjaQuery();
            QueryAndUpdateNinja();

        }
        private static void InsertClan()
        {
            var clan = new Clan
            {
                Id = 1,
                ClanName = "CoolClan"
            };
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                context.Clans.Add(clan);
                context.SaveChanges();
            }
        }

        private static void InsertNinja()
        {
            var ninja = new Ninja
            {
                Name = "JulieSan",
                ServiceInOniwaban = false,
                DateOfBirth = new DateTime(1980, 1, 1),
                ClanId = 1
            };
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                context.Ninjas.Add(ninja);
                context.SaveChanges();
            }
        }
        private static void InsertMultipleNinjas()
        {
            var ninja1 = new Ninja
            {
                Name = "Leonardo",
                ServiceInOniwaban = false,
                DateOfBirth = new DateTime(1980, 1, 1),
                ClanId = 1
            };
            var ninja2 = new Ninja
            {
                Name = "Michelangelo",
                ServiceInOniwaban = false,
                DateOfBirth = new DateTime(1980, 1, 1),
                ClanId = 1
            };
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                context.Ninjas.AddRange(new List<Ninja> {ninja1, ninja2});
                context.SaveChanges();
            }
        }

        private static void SimpleNinjaQuery()
        {
            using (var context = new NinjaContext())
            {
                //var ninjas = context.Ninjas.ToList();
                //var ninjas = context.Ninjas.Where(n => n.Name == "Leonardo").FirstOrDefault();
                var ninjas = context.Ninjas.FirstOrDefault(n => n.Name == "Leonardo");
            }
        }

        private static void QueryAndUpdateNinja()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                var ninja = context.Ninjas.FirstOrDefault();
                ninja.ServiceInOniwaban = (!ninja.ServiceInOniwaban);
                context.SaveChanges();
            }
        }

        private static void QueryAndUpdateNinjaDisconnected()
        {
            Ninja ninja;

            // we get something from the db
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                ninja = context.Ninjas.FirstOrDefault();
            }

            // a client get the value, modifies it, and sends it back
            ninja.ServiceInOniwaban = (!ninja.ServiceInOniwaban);

            // save it to the DB, but this new context doesn't know about this ninja... unless we tell it
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;

                // approach 1
                context.Ninjas.Add(ninja);  // EF, please INSERT this data

                // approach 2
                context.Ninjas.Attach(ninja); // EF, please WATCH this data
                context.Entry(ninja).State = EntityState.Modified; // this is a modified existing entry

                context.SaveChanges();
            }
        }

        private static void DeleteNinja()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                var ninja = context.Ninjas.FirstOrDefault();
                context.Ninjas.Remove(ninja);
                context.SaveChanges();
            }
        }
        private static void DeleteNinjaDisconnected()
        {
            Ninja ninja;
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                ninja = context.Ninjas.FirstOrDefault();
            }
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                context.Entry(ninja).State = EntityState.Deleted;
                context.Ninjas.Remove(ninja);
                context.SaveChanges();
            }
        }
        private static void DeleteNinjaWithKeyValue()
        {
            var keyval = 1;
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                var ninja = context.Ninjas.Find(keyval);
                context.Ninjas.Remove(ninja);
                context.SaveChanges();
            }
        }
        private static void InsertNinjaWithEquipment()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;

                var ninja = new Ninja()
                {
                    Name = "Raphel",
                    ServiceInOniwaban = false,
                    DateOfBirth = new DateTime(1990,1,1),
                    ClanId = 1
                };
                var sai = new NinjaEquipment
                {
                    Name = "Sai",
                    Type = EquipmentType.Weapon
                };
                context.Ninjas.Add(ninja);
                ninja.EquipmentOwned.Add(sai);
                context.SaveChanges();
            }
        }

        private static void QueryNinjaWithEquipment()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;

                var ninja = context.Ninjas.Include(n => n.EquipmentOwned)
                    .FirstOrDefault(n => n.Name.StartsWith("Raphel"));
            }
        }

        private static void QueryNinjaWithEquipment2()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;

                var ninja = context.Ninjas
                    .FirstOrDefault(n => n.Name.StartsWith("Raphel"));
                
                // if there was more than one result we could just go back and get the equipment for this one
                context.Entry(ninja).Collection(n => n.EquipmentOwned).Load();
            }
        }
        private static void QueryNinjaWithEquipment3()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;

                var ninja = context.Ninjas
                    .FirstOrDefault(n => n.Name.StartsWith("Raphel"));

                // lazy loading: if EquipmentOwned is virtual, entity will go back to the db to get EquipmentOwned when it is referenced
                Console.WriteLine("Ninja EquipmentCount: {0}", ninja.EquipmentOwned.Count());
            }
        }
    }

}
