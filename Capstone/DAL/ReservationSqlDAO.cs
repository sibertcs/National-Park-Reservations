using System;
using System.Collections.Generic;
using System.Text;
using Capstone.Models;
using System.Data.SqlClient;

namespace Capstone.DAL
{
    public class ReservationSqlDAO : IReservation
    {
        private string connectionString;
        public ReservationSqlDAO(string dbconnectionString)
        {
            this.connectionString = dbconnectionString;
        }
        public int CreateReservation(int siteId, string reservationName, DateTime arrivalDate, DateTime departureDate)
        {
            int reservationId = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("INSERT INTO reservation (site_id, name, from_date, to_date, create_date) VALUES (@siteId, @reservationName, @arrivalDate, @departureDate, @createDate);", conn);
                    cmd.Parameters.AddWithValue("@siteId", siteId);
                    cmd.Parameters.AddWithValue("@reservationName", reservationName);
                    cmd.Parameters.AddWithValue("@arrivalDate", arrivalDate);
                    cmd.Parameters.AddWithValue("@departureDate", departureDate);
                    cmd.Parameters.AddWithValue("@createDate", DateTime.Now);

                    cmd.ExecuteNonQuery();

                    cmd = new SqlCommand("SELECT MAX(reservation_id) FROM reservation", conn);
                    reservationId = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return reservationId;
        }
    }
}
