using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FootyStatMVC1.Models.FootyStat.Filters.KeepBehaviours;
using FootyStatMVC1.Models.FootyStat.SnapViewNS;

namespace FootyStatMVC1.Models.FootyStat.Actions.Constraints
{
    // Constraint limiting mins played to Greater than or equal to a value
    public class MinsPlayedConstraint : BaseConstraint
    {
        public MinsPlayedConstraint(Field f, string the_minimum)
            : base(f, new KeepIfLess())
        {
            minimum = the_minimum;

            // Default to pass
            decision = true;
        }

        // Value of minimum min constraint
        string minimum;

        public override void doAction(SVRow r)
        {
            decision = kBehaviour.keepIf(r.row[field.address()], new SingleCutVal(minimum));
        }

    }
}