using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FootyStatMVC1.Models.FootyStat.Mediator.Colleagues;
using FootyStatMVC1.Models.FootyStat.Factory.Inputs;
using FootyStatMVC1.Models.FootyStat.Mediator;
using FootyStatMVC1.Models.FootyStat.SnapViewNS;
using FootyStatMVC1.Models.FootyStat.Actions;
using FootyStatMVC1.Models.FootyStat.Actions.Constraints;
using FootyStatMVC1.Models.FootyStat.Filters;

namespace FootyStatMVC1.Models.FootyStat.Factory
{
    // MediatorColleague (MC) factory for creating MC's with TWO EXTRA input parameter (e.g., filter value) 
    // NOTE: this is over and above the field_name which every MC must have.
    // NOTE2: So in total, the create method will have FOUR inputs: the MC name, fieldname and  2 extra inputs.
    public class DoubleInputMCActionFactory : MediatorColleagueFactory
    {

        public DoubleInputMCActionFactory(SnapViewDirector svd)
            : base(svd)
        {
        }

        public override MediatorColleague createMC(string mc_type, MC_Input mci)
        {
            
            
            Field f = svd.findInDict(mci.field_name);

            // This cast should be safe because MCFactoryWrapper only calls this with a SingleInputActionMC_Input
            DoubleInputActionMC_Input concrete_mci = null;
            if (mci is DoubleInputActionMC_Input)
            {
                concrete_mci = (DoubleInputActionMC_Input)mci;
            }
            else
            {
                // Throw exception
                return null;
            }


            // Stuff.
            if (mc_type == "GameweekConstraintMC")
            {
                return new ConstraintMC(svd, new GameweekConstraint(f, concrete_mci.par1, concrete_mci.par2));
                
            }



            // If got here and haven't returned - we have been passed an invalid mc_type

            // Throw a "Unknown MediatorColleague type" excepiton
            return null;
        }

    }
}