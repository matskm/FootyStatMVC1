using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FootyStatMVC1.Models.FootyStat.Actions;

namespace FootyStatMVC1.Models.FootyStat.Mediator.Colleagues
{
    // Concrete MediatorColleague implementing indices
    //  - Inherits from MCAction, which inherits from MediatorColleague
    public class IndexMC : MCAction
    {
        // Constructor
        // Supplying this with an IndexingAction behaviour is what makes this behave like an "Index"
        public IndexMC(SnapViewDirector svd, IndexingAction a)
            : base(svd, a)
        {

        }



        // Implement stuff like GetSelection() and SetText() in GOF example
    }
}