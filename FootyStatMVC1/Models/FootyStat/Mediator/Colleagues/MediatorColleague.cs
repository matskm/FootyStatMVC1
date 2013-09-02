using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FootyStatMVC1.Models.FootyStat.Actions;
using FootyStatMVC1.Models.FootyStat.SnapViewNS;

namespace FootyStatMVC1.Models.FootyStat.Mediator.Colleagues
{
    // Base class for Mediator colleagues (IndexMC, FilterMC, ConstraintMC etc)
    public abstract class MediatorColleague
    {
        // Needs a reference to Director
        SnapViewDirector svd;

        // Constructor
        public MediatorColleague(SnapViewDirector the_svd)
        {
            svd = the_svd;

            // Default to isValid == false
            Changed();
        }

        // Valid field which will trigger recalculation in the SnapView
        // Make this virtual so sub-classes can override how this is set (see IndexMC)
        protected bool valid;
        public virtual bool isValid {
            get { return valid;}
            set { valid = value; } 
        }//isValid


        // Changed interface
        public void Changed()
        {
            // Set our isValid flag to false
            isValid = false;
            
        }

    }
}