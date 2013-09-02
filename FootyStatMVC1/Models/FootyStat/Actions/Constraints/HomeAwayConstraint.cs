using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FootyStatMVC1.Models.FootyStat.SnapViewNS;
using FootyStatMVC1.Models.FootyStat.Filters;
using FootyStatMVC1.Models.FootyStat.Filters.KeepBehaviours;

namespace FootyStatMVC1.Models.FootyStat.Actions.Constraints
{
    // Constraint limiting stats to either home or away
    public class HomeAwayConstraint : BaseConstraint
    {

        // Hardwire this to KeepIfEquals because HomeAway constraint
        // always operates like this
        public HomeAwayConstraint(Field f, string choice)
            : base(f, new KeepIfEquals() )
        {
            HomeAway ha_enum;

            if (choice == "H") ha_enum = HomeAway.Home;
            else ha_enum = HomeAway.Away;
            
            value = ha_enum;

            // Default to pass
            decision = true;

            HA_arr = new string[2];

            HA_arr[0] = "H";
            HA_arr[1] = "A";


        }

        // The value of the constraint (either home or away)
        HomeAway value;

        public override void doAction(SVRow r)
        {
            decision = kBehaviour.keepIf(r.row[field.address()], new SingleCutVal(HA_arr[(int)value]));
        }

        

        // Helper array matching the data format (data dependency here)
        string[] HA_arr;

    }

    public enum HomeAway { Home, Away };


    // Adapter
    public class HomeAwayConstraintAdapter
    {
        public HomeAwayConstraintAdapter(Field f, bool IsHome)
        {
            string choice;
            if(IsHome)choice="H";
            else choice="A";

            adaptee = new HomeAwayConstraint(f, choice);
        }

        public HomeAwayConstraint adaptee { get; private set; }
    }

}