using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootyStatMVC1.Models.FootyStat.SnapViewNS;

namespace FootyStatMVC1.Models.FootyStat.Actions
{
    // Description:
    //   - Base class for TotallingAction, IndexingAction, AveTotallingAction 
    //   - A collection of these live inside SnapView, and are executed on each row 
    //     as it is considered.
    //   - E.g., for an indexing action, there will be an internal unique set which 
    //     represents the "index" for this row (all the different values it can take)
    //   - Totalling actions just total up the elements in a given column for each row.
    public abstract class BaseAction
    {
        // name is Read-only (setter is private)
        //public string name { get; private set; }

        // Every action must be able to be "done" by the containing 
        // snapview. Derived classes have the actual implementation.
        // Parameter is a "row" of the snapView.
        
        //public abstract void doAction(string[] r);

        public abstract void doAction(SVRow r);

        public abstract void print_me();

        // The Field which this action operates on (provides the right address in the SVRow)
        // Make this read-only because it should never be changed.
        public Field field { get; private set; }

        // Constructor takes the field
        public BaseAction(Field f)
        {
            field = f;
        }

    }
}
