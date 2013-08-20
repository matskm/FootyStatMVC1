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


        


        // Registration now managed by SVD - this is the right design as of Aug 20th. Hence these methods disabled.

        //// Registration code
        //public void register_me()
        //{
        //    svd.Attach(this);

            
        //}

        //public void remove_me()
        //{
        //    svd.Detach(this);
        //}

        // Valid field which will trigger recalculation in the SnapView
        public bool isValid { get; set; }

        // Changed interface
        public void Changed()
        {
            // Set our isValid flag to false
            isValid = false;

            //// Call Director ColleagueChanged()
            //svd.ColleagueChanged(this);
            
        }

    }
}