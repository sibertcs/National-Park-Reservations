using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;


namespace Capstone.DAL
{
    public class CampgroundSqlDAO : ICampground
    {
        private string connectionString;
        public CampgroundSqlDAO(string dbconnectionString)
        {
            this.connectionString = dbconnectionString;
        }
        public IList<Campground> GetCampgroundsFromPark(int parkID)
        {
            List<Campground> campgrounds = new List<Campground>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand($"SELECT * FROM campground WHERE park_id = {parkID};", conn);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Campground newCampground = new Campground();

                        newCampground.CampgroundID = Convert.ToInt32(reader["campground_id"]);
                        newCampground.CampgroundName = Convert.ToString(reader["name"]);
                        newCampground.OpenFrom = Convert.ToInt32(reader["open_from_mm"]);
                        newCampground.OpenTo = Convert.ToInt32(reader["open_to_mm"]);
                        newCampground.DailyFee = Convert.ToDecimal(reader["daily_fee"]);
                                                                     
                        campgrounds.Add(newCampground);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return campgrounds;
        }
    }
}
