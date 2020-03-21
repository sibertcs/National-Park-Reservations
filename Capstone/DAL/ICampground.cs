using System;
using System.Collections.Generic;
using System.Text;
using Capstone.Models;

namespace Capstone.DAL
{
    public interface ICampground
    {
        IList<Campground> GetCampgroundsFromPark(int parkID);
    }
}
