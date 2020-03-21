using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Capstone.DAL;

namespace Capstone
{
    class Program
    {
        static void Main(string[] args)
        {
            //Get the connection string from the appsettings.json file
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            string connectionString = configuration.GetConnectionString("Project");

            ICampground campgroundDAO = new CampgroundSqlDAO(connectionString);
            IPark parkDAO = new ParkSqlDAO(connectionString);
            IReservation reservationDAO = new ReservationSqlDAO(connectionString);
            ISite siteDAO = new SiteSqlDAO(connectionString);

            CapstoneCLI capstoneCLI = new CapstoneCLI(campgroundDAO, reservationDAO, parkDAO, siteDAO);
            capstoneCLI.RunMainMenu();

        }
    }
}
