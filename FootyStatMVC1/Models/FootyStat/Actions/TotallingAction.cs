using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootyStatMVC1.Models.FootyStat.SnapViewNS;

namespace FootyStatMVC1.Models.FootyStat.Actions
{
    // Description:
    //    - Base class is BaseAction
    //    - Totals up a column
    public class TotallingAction : InitAction
    {
        // Default constructor
        public TotallingAction(Field f)
            : base(f)
        {
            // Initialise this to zero explicitly
            total = 0;
        }
        
        // The running total of the column
        public double total { get; private set; }



        

        public override void doAction(SVRow r){
            string goal_str = r.row[field.address()];
            double goal_dbl = Convert.ToDouble(goal_str);

            total += goal_dbl;
        }

        public override void init()
        {
            // Gets called when iteration over snapview starts
            total = 0;
        }

        

    }//class
}//namespace
