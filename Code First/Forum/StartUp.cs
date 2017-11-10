using System;
using Forum.Data;
using Forum.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Forum
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (var db = new ForumDbContext())
            {
                ResetDatabase(db);
                var users = new[]
                {
                    new User("Gosho", "123"),
                    new User("Pesho", "123"),
                    new User("Ivan", "123"),
                    new User("Meri", "123")
                };

                db.Users.AddRange(users);
                db.SaveChanges();
            }
        }

        private static void ResetDatabase(ForumDbContext db)
        {
            db.Database.EnsureDeleted();

            db.Database.Migrate();

            Seed(db);
        }

        private static void Seed(ForumDbContext db)
        {
            var users = new[]
            {
                new User("Gosho", "123"),
                new User("Pesho", "123"),
                new User("Ivan", "123"),
                new User("Meri", "123")
            };

            db.Users.AddRange(users);
            db.SaveChanges();
        }
    }
}
