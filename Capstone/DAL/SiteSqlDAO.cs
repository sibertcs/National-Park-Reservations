using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace Capstone.DAL
{
    public class SiteSqlDAO : ISite
    {
        private string connectionString;
        public SiteSqlDAO(string dbconnectionString)
        {
            this.connectionString = dbconnectionString;
        }
        public IList<Site> GetAvailableSites(int campgroundID, DateTime arrivalDate, DateTime departureDate)
        {
            List<Site> availableSites = new List<Site>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(@"SELECT site.*,campground.daily_fee FROM site 
                    JOIN campground ON site.campground_id = campground.campground_id 
                    WHERE site.site_id NOT IN (SELECT site.site_id FROM site
                    JOIN campground ON campground.campground_id = site.campground_id
                    JOIN reservation ON site.site_id = reservation.site_id
                    WHERE (reservation.from_date >= @arrivalDate AND reservation.from_date <= @departureDate
                    OR reservation.to_date >= @arrivalDate AND reservation.to_date <= @departureDate)) 
                    AND campground.campground_id = @campgroundID;", conn);
                    cmd.Parameters.AddWithValue("@campgroundID", campgroundID);
                    cmd.Parameters.AddWithValue("@arrivalDate", arrivalDate);
                    cmd.Parameters.AddWithValue("@departureDate", departureDate);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Site newSite = new Site();

                        newSite.SiteNumber = Convert.ToInt32(reader["site_number"]);
                        newSite.MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]);
                        newSite.Accessible = Convert.ToInt32(reader["accessible"]);
                        newSite.MaxRVLenght = Convert.ToInt32(reader["max_rv_length"]);
                        newSite.Utilities = Convert.ToInt32(reader["utilities"]);
                        newSite.Cost = Convert.ToDecimal(reader["daily_fee"]) * (departureDate - arrivalDate).Days;

                        availableSites.Add(newSite);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return availableSites;
        }
    }
}
