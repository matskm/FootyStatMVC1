using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FootyStatMVC1.Models.FootyStat.Actions;
using FootyStatMVC1.Models.FootyStat.SnapViewNS;

namespace FootyStatMVC1.Models.FootyStat.Mediator.Colleagues
{
    // Extra class layer between MediatorColleague and derived classes
    // to ensure that SnapView doesn't have to have a BaseAction with a doAction method (which would make no sense)
    //    - Classes that derive from this one must have a BaseAction variable which must have a doAction method
    //    - So this class inherits from MediatorColleague and Has-A instance variable which inherits from BaseAction
    public abstract class MCAction : MediatorColleague
    {
        //public abstract void do_action(SVRow row);

        // Has-A relationship with BaseAction
        public BaseAction action;

        // Access to action
        public BaseAction get_action()
        { 
            return action;
        }

        public MCAction(SnapViewDirector svd, BaseAction a)
            : base(svd)
        {
            action = a;

            // This next line is very important: call the "Changed()" method of the MediatorColleague which 
            // in via the SnapViewDirector sets the snapview isvalid flag to false.
            Changed();
        }

        // Interface to the doAction method
        public void doAction(SVRow r)
        {
            action.doAction(r);
        }
    }
}