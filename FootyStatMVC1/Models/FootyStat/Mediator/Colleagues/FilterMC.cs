using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FootyStatMVC1.Models.FootyStat.Filters;

namespace FootyStatMVC1.Models.FootyStat.Mediator.Colleagues
{
    // Concrete MediatorColleague implementing filters
    // - Inherits from MCAction, which inherits from MediatorColleague
    public class FilterMC : MCAction
    {
        // Constructor
        // Supplying this with BaseFilter behaviour (action) is what 
        // makes this behave like a filter
        public FilterMC(SnapViewDirector svd, BaseFilter a)
            : base(svd, a)
        {

        }

    }
}