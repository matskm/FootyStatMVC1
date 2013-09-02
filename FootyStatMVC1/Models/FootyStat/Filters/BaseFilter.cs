using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootyStatMVC1.Models.FootyStat.SnapViewNS;
using FootyStatMVC1.Models.FootyStat.Actions;
using FootyStatMVC1.Models.FootyStat.Filters.KeepBehaviours;

namespace FootyStatMVC1.Models.FootyStat.Filters
{
    //Base class for the filters applied to the snapView
    public class BaseFilter : BaseAction
    {

        // name is Read-only (setter is private)
        public string name { get; private set; }

        // Strategy pattern: keep behaviour can be
        // Keep if:
        //   - Equals
        //   - Greater than
        //   - Less than
        protected KeepBehaviour kBehaviour;

        // Decision of the filter
        // Filter decision
        public bool decision { get; protected set; }




        public BaseFilter(Field f, string n, KeepBehaviour k)
            : base(f)
        {
            name = n;
            kBehaviour = k;
        }

        

        // Every Filter must be able to be "done" by the containing 
        // snapview. Derived classes have the actual implementation.
        // Parameter is a "row" of the snapView.

        //abstract public bool try_filter(SVRow r);
        public override void doAction(SVRow r)
        {
            // Throw an exception as this doesn't do anything
        }

        
    }
}
