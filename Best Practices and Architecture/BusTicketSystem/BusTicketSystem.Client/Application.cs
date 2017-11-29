using System;
using System.Linq;
using BusTicketSystem.Client.Core;
using BusTicketSystem.Data;
using BusTicketSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BusTicketSystem.Client
{
    public class Application
    {
        static void Main(string[] args)
        {
            //ResetDatabase();

            var commandDispatcher = new CommandDispatcher();
            var engine = new Engine(commandDispatcher);
            engine.Run();
        }

        public static void ResetDatabase()
        {
            using (var db = new BusTicketSystemContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            }
        }
    }
}
