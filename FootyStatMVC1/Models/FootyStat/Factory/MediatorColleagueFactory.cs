using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FootyStatMVC1.Models.FootyStat.Mediator.Colleagues;
using FootyStatMVC1.Models.FootyStat.Factory.Inputs;
using FootyStatMVC1.Models.FootyStat.Mediator;

namespace FootyStatMVC1.Models.FootyStat.Factory
{
    // Base class for factories making MediatorColleague objects
    public abstract class MediatorColleagueFactory
    {

        // This class heirarchy implements the Factory Method DP
        //  - But the full functionality is not used (i.e., we know the concrete factory classes in the client)
        //  - For the future, the full DP functionality can be utilised by putting creation steps common to all MediatorColleagues in 
        //    a base class method (e.g., makeMC) in this class which polymorphically calls derived class createMC methods)
        //  - Then the interface would be makeMC instead of createMC

        public MediatorColleagueFactory(SnapViewDirector the_svd)
        {
            svd = the_svd;
        }

        public abstract MediatorColleague createMC(string mc_type, MC_Input mci);

        protected SnapViewDirector svd;

    }
}