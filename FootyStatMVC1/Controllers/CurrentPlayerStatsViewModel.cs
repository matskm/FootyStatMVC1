using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootyStatMVC1.Controllers
{
    // View model to pass current player stats 
    public class CurrentPlayerStatsViewModel
    {
        public CurrentPlayerStatsViewModel(int g, int a)
        {
            goals = g;
            assists = a;
        }

        public int goals { get; set; }
        public int assists { get; set; }
    }
}