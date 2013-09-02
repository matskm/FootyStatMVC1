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

        // Override isValid property because there is a case (>1?) where
        // an Index is valid even if the filter changes (i.e., a degenerate index)
        public override bool isValid
        {
            get { return valid; }

            // Checks if index is degenerate, and overrides to isValid=true in this case
            set
            {
                // This should always pass because IndexMC is hardcoded for IndexingAction
                if (get_action() is IndexingAction)
                {
                    IndexingAction ia = (IndexingAction)get_action();
                    // If index only contains 1 value (i.e., degenerate) it has to be valid
                    if (ia.size() == 1)
                    {
                        valid = true;
                    }
                    // Otherwise if size>1 or 0 allow valid to be true or false (size 0 means index initialised, but not iterated yet)
                    else if (ia.size() > 1 || ia.size() == 0)
                    {
                        valid = value;
                    }
                    else
                    {
                        // Throw an exception - should never get in here 
                    }
                }
            }
        }//isValid property

        // Implement stuff like GetSelection() and SetText() in GOF example
    }
}