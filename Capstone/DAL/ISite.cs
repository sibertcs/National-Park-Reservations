using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    public interface ISite
    {
        IList<Site> GetAvailableSites(int campgroundID, DateTime arrivalDate, DateTime departureDate);
    }
}
