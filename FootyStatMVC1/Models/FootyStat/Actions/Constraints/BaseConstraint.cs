using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FootyStatMVC1.Models.FootyStat.Filters;
using FootyStatMVC1.Models.FootyStat.SnapViewNS;
using FootyStatMVC1.Models.FootyStat.Filters.KeepBehaviours;

namespace FootyStatMVC1.Models.FootyStat.Actions.Constraints
{
    // Base class for Constraints like HomeAwayConstraint
    // Differs from Filters because doesn't actually chuck the row away
    // Just limits what rows the stats are calculated over
    public class BaseConstraint : BaseAction
    {

        // Strategy pattern like Filters
        protected KeepBehaviour kBehaviour;

        public bool decision { get; protected set; }

        public BaseConstraint(Field f, KeepBehaviour k)
            : base(f)
        {
            kBehaviour = k;
        }

        // Every Constraint must be able to be "done" by the containing 
        // snapview. Derived classes have the actual implementation.
        // Parameter is a "row" of the snapView.



        //abstract public bool try_filter(SVRow r);
        public override void doAction(SVRow r)
        {
            // Throw an exception as this doesn't do anything
        }

        

    }
}