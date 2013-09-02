using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FootyStatMVC1.Models.FootyStat.Mediator.Colleagues;
using FootyStatMVC1.Models.FootyStat.Factory.Inputs;
using FootyStatMVC1.Models.FootyStat.Mediator;
using FootyStatMVC1.Models.FootyStat.SnapViewNS;
using FootyStatMVC1.Models.FootyStat.Actions;
using FootyStatMVC1.Models.FootyStat.Filters;
using FootyStatMVC1.Models.FootyStat.Actions.Constraints;

namespace FootyStatMVC1.Models.FootyStat.Factory
{
    // MediatorColleague (MC) factory for creating MC's with one EXTRA input parameter (e.g., filter value) 
    // NOTE: this is over and above the field_name which every MC must have.
    // NOTE2: So in total, the create method will have 3 inputs: the MC name, fieldname and extra input.
    public class SingleInputMCActionFactory : MediatorColleagueFactory
    {

        public SingleInputMCActionFactory(SnapViewDirector svd)
            : base(svd)
        {

        }

        public override MediatorColleague createMC(string mc_type, MC_Input mci)
        {

            Field f = svd.findInDict(mci.field_name);

            // This cast should be safe because MCFactoryWrapper only calls this with a SingleInputActionMC_Input
            SingleInputActionMC_Input concrete_mci = null;
            if (mci is SingleInputActionMC_Input)
            {
                concrete_mci = (SingleInputActionMC_Input)mci;
            }
            else
            {
                // Throw exception
                return null;
            }
            



            // Stuff.
            if (mc_type == "StringEqualsFilterMC")
            {
                return new FilterMC(svd, new StringEqualsFilter(mci.field_name, f, concrete_mci.par1));
            }

            if (mc_type == "MinsPlayedConstraintMC")
            {
                return new ConstraintMC(svd, new MinsPlayedConstraint(f, concrete_mci.par1));
            }

            if (mc_type == "HomeAwayConstraintMC")
            {
                return new ConstraintMC(svd, new HomeAwayConstraint(f, concrete_mci.par1));
            }



            // If got here and haven't returned - we have been passed an invalid mc_type

            // Throw a "Unknown MediatorColleague type" excepiton
            return null;
        }

    }
}