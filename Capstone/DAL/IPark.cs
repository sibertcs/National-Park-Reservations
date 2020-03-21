using System;
using System.Collections.Generic;
using System.Text;
using Capstone.Models;


namespace Capstone.DAL
{
    public interface IPark
    {
        IList<Park> GetParks();
        Park GetParkInfo(int parkID);
    }
}
