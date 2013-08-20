using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FootyStatMVC1.Models.FootyStat.Actions;


namespace FootyStatMVC1.Models.FootyStat.Mediator.Colleagues
{
    // Concrete MediatorColleague implementing Totalling stats
    public class TotallingMC : MCAction
    {

        public TotallingMC(SnapViewDirector svd, TotallingAction a)
            : base(svd, a)
        {

        }

    }
}