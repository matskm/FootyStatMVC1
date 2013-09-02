using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FootyStatMVC1.Models.FootyStat.Mediator.Colleagues;
using FootyStatMVC1.Models.FootyStat.Factory.Inputs;
using FootyStatMVC1.Models.FootyStat.Mediator;
using FootyStatMVC1.Models.FootyStat.SnapViewNS;
using FootyStatMVC1.Models.FootyStat.Actions;

namespace FootyStatMVC1.Models.FootyStat.Factory
{
    // MediatorColleague (MC) factory for creating MC's with just one input parameter (field_name)
    public class SimpleMCActionFactory : MediatorColleagueFactory
    {
        public SimpleMCActionFactory(SnapViewDirector svd)
            : base(svd)
        {
        }

        public override MediatorColleague createMC(string mc_type, MC_Input mci)
        {

            Field f = svd.findInDict(mci.field_name);

            
            if (mc_type == "IndexMC")
            {
                return new IndexMC(svd, new IndexingAction(f));
            }
            if (mc_type == "TotallingMC")
            {
                return new TotallingMC(svd, new TotallingAction(f));
            }

            


            // If got here and haven't returned - we have been passed an invalid mc_type

            // Throw a "Unknown MediatorColleague type" excepiton
            return null;

        }

    }

}