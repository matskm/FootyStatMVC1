using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FootyStatMVC1.Models.FootyStat.Filters.KeepBehaviours;
using FootyStatMVC1.Models.FootyStat.SnapViewNS;

namespace FootyStatMVC1.Models.FootyStat.Actions.Constraints
{
    // Constraint limiting stats within gameweek range
    public class GameweekConstraint : BaseConstraint
    {

        // The gameweek constraint always needs the range behaviour
        public GameweekConstraint(Field f, string the_min, string the_max)
            : base(f, new KeepIfInRange() )
        {
            min = the_min;
            max = the_max;

            // Default to pass
            decision = true;
        }

        // Value of constraints
        string min;
        string max;

        public override void doAction(SnapViewNS.SVRow r)
        {
            //decision = kBehaviour.keepIf(r.row[field.address()], HA_arr[(int)value]);
            decision = kBehaviour.keepIf(r.row[field.address()], new DoubleCutVal(min, max));
        }

        

    }//class

    // Adapter so that the ViewModel can generate this constraint
    // Adapter needed because numbers in viewmodel, strings in underlying model
    public class GameweekConstraintAdapter
    {
        public GameweekConstraintAdapter(Field f, int min, int max)
        {
            adaptee = new GameweekConstraint(f, Convert.ToString(min), Convert.ToString(max));
        }

        public GameweekConstraint adaptee { get; private set; }
    }//class

}