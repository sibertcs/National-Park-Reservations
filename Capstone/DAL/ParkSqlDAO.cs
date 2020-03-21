using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Capstone.Models;


namespace Capstone.DAL
{
    public class ParkSqlDAO : IPark
    {
        private string connectionString;
        public ParkSqlDAO(string dbconnectionString)
        {
            this.connectionString = dbconnectionString;
        }

        public IList<Park> GetParks()
        {
            List<Park> parks = new List<Park>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT name FROM park;", conn);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read()) 
                    {
                        Park newPark = new Park();

                        newPark.Name = Convert.ToString(reader["name"]);

                        parks.Add(newPark);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return parks;
        }

        public Park GetParkInfo(int parkID)
        {
            Park park = new Park();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM park WHERE park_id = @parkID;", conn);
                    cmd.Parameters.AddWithValue("@parkID", parkID);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        park.ParkID = Convert.ToInt32(reader["park_id"]);
                        park.Name = Convert.ToString(reader["name"]);
                        park.Location = Convert.ToString(reader["location"]);
                        park.DateEst = Convert.ToDateTime(reader["establish_date"]);
                        park.Area = Convert.ToInt32(reader["area"]);
                        park.Visitors = Convert.ToInt32(reader["visitors"]);
                        park.Description = Convert.ToString(reader["description"]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return park;
        }
    }
}
