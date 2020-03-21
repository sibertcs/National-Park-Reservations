using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    public class CapstoneCLI
    {
        private ICampground campgroundDAO;
        private IPark parkDAO;
        private IReservation reservationDAO;
        private ISite siteDAO;

        public CapstoneCLI(ICampground campgroundDAO, IReservation reservationDAO, IPark parkDAO, ISite siteDAO)
        {
            this.campgroundDAO = campgroundDAO;
            this.reservationDAO = reservationDAO;
            this.parkDAO = parkDAO;
            this.siteDAO = siteDAO;
        }

        private Dictionary<int, string> Calendar = new Dictionary<int, string>()
        {
            { 1, "January" },
            { 2, "February" },
            { 3, "March" },
            { 4, "April" },
            { 5, "May" },
            { 6, "June" },
            { 7, "July" },
            { 8, "August" },
            { 9, "September" },
            { 10, "October" },
            { 11, "November" },
            { 12, "December" }
        };

        public void RunMainMenu()
        {           
            Console.Clear();
            PrintHeader();            
            Console.WriteLine("Select a Park for Further Details");
            int parkCount = PrintAllParks();
            Console.WriteLine("Q) Quit");
            bool isRunning = true;
            while (isRunning)
            {
                int selectedPark;
                var input = Console.ReadLine();

                bool success = Int32.TryParse(input, out selectedPark);

                if (success)
                {
                    if (selectedPark > 0 && selectedPark <= parkCount)
                    {
                        Console.Clear();
                        ParkMenu(selectedPark);                       
                    }
                    else
                    {
                        Console.WriteLine("Invalid entry. Please select a valid key.");
                        continue;
                    }                  
                }
                else
                {                  
                    if (input == "q")
                    {
                        Console.Clear();
                        Console.WriteLine("Thank you for using your National Park Campsite Reservation System.");
                        Console.WriteLine("Press any key to close.");
                        Console.WriteLine();
                        isRunning = false;
                    }
                    else
                    {
                        Console.WriteLine("Invalid entry. Please select a valid key.");
                        continue;
                    }
                }     
            }
        }

        private int PrintAllParks()
        {
            IList<Park> parks = parkDAO.GetParks();

            int count = 1;
            if (parks.Count > 0)
            {
                foreach (Park pk in parks)
                {
                    Console.WriteLine($"{count}) {pk.Name}");
                    count++;
                }
            }
            return parks.Count;
        }

        private void ParkMenu(int selectedPark)
        {
            Console.Clear();
            DisplayParkInfo(selectedPark);

            Console.WriteLine();
            Console.WriteLine($"Select a Command:");
            Console.WriteLine($"    1)  View Campgrounds");
            Console.WriteLine($"    2)  Search for Reservation");
            Console.WriteLine($"    3)  Return to Previous Screen");
            Console.WriteLine();
            
            while (true)
            {
                try
                {                    
                    int input = int.Parse(Console.ReadLine());
                    if (input == 1)
                    {
                        Console.Clear();
                        CampgroundMenu(selectedPark);
                    }
                    else if (input == 2)
                    {
                        Console.WriteLine("Feature Coming Soon. Press any key to return to the main menu.");
                        Console.ReadLine();
                        RunMainMenu();
                    }
                    else if (input == 3)
                    {
                        Console.Clear();
                        RunMainMenu();
                    }
                    else
                    {
                        Console.WriteLine("Invalid entry. Plese select a valid key.");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid entry. Plese select a valid key.");
                }
            }
        }

        private void DisplayParkInfo(int selectedPark)
        {
            Park prk = parkDAO.GetParkInfo(selectedPark);

            Console.WriteLine($"{prk.Name} National Park");
            Console.WriteLine($"Location: {prk.Location}");
            Console.WriteLine($"Established: {prk.DateEst}");
            Console.WriteLine($"Area: {prk.Area}");
            Console.WriteLine($"Annual Visitors: {prk.Visitors}");
            Console.WriteLine();
            Console.WriteLine($"{prk.Description}");
        }

        private void CampgroundMenu(int selectedPark)
        {
            DisplayCampgroundInfo(selectedPark);

            Console.WriteLine();
            Console.WriteLine("Select a Command");
            Console.WriteLine($"    1)  Search for Available Reservation");
            Console.WriteLine($"    2)  Return to Previous Screen");
            
            while (true)
            {
                try
                {                   
                    int input = int.Parse(Console.ReadLine());

                    if (input == 1)
                    {
                        PromptForReservationInfo(selectedPark);
                    }
                    else if (input == 2)
                    {
                        Console.Clear();
                        ParkMenu(selectedPark);                                              
                    }
                    else
                    {
                        Console.WriteLine("Invalid. Please enter a valid key.");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid. Please enter a valid key.");
                }
            }            
        }
        private void DisplayCampgroundInfo(int parkID)
        {
            Park prk = parkDAO.GetParkInfo(parkID);

            Console.WriteLine($"{prk.Name} National Park Campgrounds");
            Console.WriteLine();
            Console.WriteLine($"{"Name", 7} {"Open", 20} {"Close", 13} {"Daily Fee", 19}");

            IList<Campground> campgrounds = campgroundDAO.GetCampgroundsFromPark(parkID);

            if (campgrounds.Count > 0)
            {
                int count = 1;
                foreach (Campground cg in campgrounds)
                {
                    Console.WriteLine($"#{count} {cg.CampgroundName, -20} {Calendar[cg.OpenFrom], -12} {Calendar[cg.OpenTo], -15} {cg.DailyFee:C}");
                    count++;
                }
            }
        }
        private void PromptForReservationInfo(int selectedPark)
        {
            IList<Campground> availableCampgrounds = campgroundDAO.GetCampgroundsFromPark(selectedPark);  
            
            int desiredCampground;
            DateTime arrivalDate;
            DateTime departureDate;
            while (true)
            {
                desiredCampground = CLIHelper.GetInteger("Which campground? Enter 0 to cancel");
                if (desiredCampground == 0)
                {
                    Console.Clear();
                    CampgroundMenu(selectedPark);
                }
                else if (desiredCampground <= availableCampgrounds.Count())
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid Entry. Please select a campground from the list.");
                }
            }
            while (true)
            {
                arrivalDate = CLIHelper.GetDateTime("What day do you plan on arriving? Please enter in yyyy-mm-dd format.");
                departureDate = CLIHelper.GetDateTime("What day do you plan on leaving? Please enter in yyyy-mm-dd format.");
                if (arrivalDate > departureDate)
                {
                    Console.WriteLine();
                    Console.WriteLine("Your arrival date must be before your departure date. Please try again.");
                }
                else
                {
                    break;
                }
            }
            SiteMenu(selectedPark, desiredCampground, arrivalDate, departureDate);
        }

        private void SiteMenu(int selectedPark, int desiredCampground, DateTime arrivalDate, DateTime departureDate)
        {
            IList<Site> sites = siteDAO.GetAvailableSites(desiredCampground, arrivalDate, departureDate);

            if (sites == null || sites.Count == 0)
            {
                Console.WriteLine("There are no sites available for the dates you specified."); 
                Console.WriteLine("Would you like to enter an alternate date range?");
                Console.WriteLine("     (Select 'Y' for Yes Or Press any other key to return to the main menu)");

                string choice = Console.ReadLine().ToLower();
                if (choice == "y")
                {
                    PromptForReservationInfo(selectedPark);
                }
                else 
                {
                    RunMainMenu();
                }              
            }
            Console.Clear();
            Console.WriteLine("Results Matching Your Search Criteria:");
            Console.WriteLine();
            Console.WriteLine($"{"Results",-15} {"Max Occup.",-17} {"Accessible?",-13} {"Max RV Length",-14} {"Utility",-15} {"Cost",-14}");
            Console.WriteLine();

            DisplayAvailableSites(sites);

            MakeReservation(sites, arrivalDate, departureDate);
        }

        private void DisplayAvailableSites (IList<Site> sites)
        {
            int count = 0;
            foreach (Site sit in sites)
            {
                Console.WriteLine($"{sit.SiteNumber + ")", -14}  {sit.MaxOccupancy, -16}  {ConvertAccessibilityToString(sit.Accessible), -12}  {sit.MaxRVLenght, -14}  {ConvertUtilityToString(sit.Utilities), -14}  {sit.Cost:C}"); //Needs cost implemented
                count++;
            }          
        }

        private void MakeReservation(IList<Site> sites, DateTime arrivalDate, DateTime departureDate)
        {
            while (true)
            {
                try
                {
                    Console.WriteLine();
                    Console.WriteLine("Which site should be reserved (enter 0 to cancel)?");

                    int desiredSite = int.Parse(Console.ReadLine());
                    if (desiredSite == 0)
                    {
                        RunMainMenu();
                    }
                    bool isValidInput = false;
                    foreach (Site site in sites)
                    {
                        if (site.SiteNumber == desiredSite)
                        {
                            isValidInput = true;

                            Console.WriteLine("What name should the reservation be made under?");

                            string reservationName = Console.ReadLine();
                            int confirmationId = reservationDAO.CreateReservation(desiredSite, reservationName, arrivalDate, departureDate);

                            Console.Clear();
                            Console.WriteLine($"The reservation has been made and the confirmation number is: *** {confirmationId} ***");
                            Console.WriteLine();
                            Console.WriteLine("Enjoy staying at your National Park. Press any key to return to the main menu.");
                            Console.ReadKey();
                            break;
                        }
                    }
                    if (!isValidInput) throw new Exception();
                    break;
                }               
                catch (Exception)
                {
                    Console.WriteLine("Invalid. Please select an available site");
                    continue;
                }            
            }
            RunMainMenu();
        }

        private string ConvertAccessibilityToString(int number)
        {
            string accessibility; 
            if (number == 0)
            {
                accessibility = "No";
            }
            else
            {
                accessibility = "Yes";
            }
            return accessibility;
        }

        private string ConvertUtilityToString(int number)
        {
            string utilities;
            if (number == 0)
            {
                utilities = "N/A";
            }
            else
            {
                utilities = "Yes";
            }
            return utilities;
        }

        private void PrintHeader()
        {
            Console.WriteLine(@" _   _       _   _                   _   _____           _       _____                          _ _       ");
            Console.WriteLine(@"| \ | |     | | (_)                 | | |  __ \         | |     / ____|                        (_) |      ");
            Console.WriteLine(@"|  \| | __ _| |_ _  ___  _ __   __ _| | | |__) |_ _ _ __| | __ | |     __ _ _ __ ___  _ __  ___ _| |_ ___ ");
            Console.WriteLine(@"| . ` |/ _` | __| |/ _ \| '_ \ / _` | | |  ___/ _` | '__| |/ / | |    / _` | '_ ` _ \| '_ \/ __| | __/ _ \");
            Console.WriteLine(@"| |\  | (_| | |_| | (_) | | | | (_| | | | |  | (_| | |  |   <  | |___| (_| | | | | | | |_) \__ \ | ||  __/");
            Console.WriteLine(@"|_| \_|\__,_|\__|_|\___/|_| |_|\__,_|_| |_|   \__,_|_|  |_|\_\  \_____\__,_|_| |_| |_| .__/|___/_|\__\___|");
            Console.WriteLine(@"                                                                                     | |                  ");
            Console.WriteLine(@"                                                                                     |_|                  ");
            Console.WriteLine();
            Console.WriteLine(@" _____                                _   _                _____           _                 ");
            Console.WriteLine(@"|  __ \                              | | (_)              / ____|         | |                ");
            Console.WriteLine(@"| |__) |___  ___  ___ _ ____   ____ _| |_ _  ___  _ __   | (___  _   _ ___| |_ ___ _ __ ___  ");
            Console.WriteLine(@"|  _  // _ \/ __|/ _ \ '__\ \ / / _` | __| |/ _ \| '_ \   \___ \| | | / __| __/ _ \ '_ ` _ \ ");
            Console.WriteLine(@"| | \ \  __/\__ \  __/ |   \ V / (_| | |_| | (_) | | | |  ____) | |_| \__ \ ||  __/ | | | | |");
            Console.WriteLine(@"|_|  \_\___||___/\___|_|    \_/ \__,_|\__|_|\___/|_| |_| |_____/ \__, |___/\__\___|_| |_| |_|");
            Console.WriteLine(@"                                                                  __/ |                      ");
            Console.WriteLine(@"                                                                 |___/                       ");
            Console.WriteLine();
        }
    }
}
