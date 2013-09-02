using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootyStatMVC1.Controllers
{
    // View model to pass current player stats 
    // Future: this could be a container of an arbitrary number of stats.
    // For the moment hard code it to just two (goals and assists)
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