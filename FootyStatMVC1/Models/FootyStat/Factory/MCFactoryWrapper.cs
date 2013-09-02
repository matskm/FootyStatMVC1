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
    // Wrapper class for MC factory which defines static methods (accessible from anywhere)
    //   - These static methods create their own local factories (object instances)
    public class MCFactoryWrapper
    {

        public static MediatorColleague create_simple_MC(SnapViewDirector svd, string mc_name, string field_name)
        {
            SimpleMCActionFactory factory = new SimpleMCActionFactory(svd);
            return factory.createMC(mc_name, new SimpleActionMC_Input(field_name));
        }

        public static MediatorColleague create_singleInput_MC(SnapViewDirector svd, string mc_name, string field_name, string par1)
        {
            SingleInputMCActionFactory factory = new SingleInputMCActionFactory(svd);
            return factory.createMC(mc_name, new SingleInputActionMC_Input(field_name, par1));
        }

        public static MediatorColleague create_doubleInput_MC(SnapViewDirector svd, string mc_name, string field_name, string par1, string par2)
        {
            DoubleInputMCActionFactory factory = new DoubleInputMCActionFactory(svd);
            return factory.createMC(mc_name, new DoubleInputActionMC_Input(field_name, par1, par2));
        }

    }
}