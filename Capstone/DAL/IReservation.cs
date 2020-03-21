using System;
using System.Collections.Generic;
using System.Text;
using Capstone.Models;

namespace Capstone.DAL
{
    public interface IReservation
    {
        int CreateReservation(int siteId, string reservationName, DateTime arrivalDate, DateTime departureDate);
    }
}
