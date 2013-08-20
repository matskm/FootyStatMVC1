using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FootyStatMVC1.Models.FootyStat.Actions.Constraints;
using FootyStatMVC1.Models.FootyStat.Actions;

namespace FootyStatMVC1.Models.FootyStat.Mediator.Colleagues
{
    public class ConstraintMC : MCAction
    {
        // Explicitly giveing this a BaseConstraint is what makes it
        // a ConstratinMC
        public ConstraintMC(SnapViewDirector svd, BaseConstraint a)
            : base(svd, a)
        {

        }

    }
}