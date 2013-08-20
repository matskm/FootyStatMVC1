using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootyStatMVC1.Models.FootyStat.SnapViewNS;

namespace FootyStatMVC1.Models.FootyStat.Actions
{
    // Description:
    //    - Base class is BaseAction
    //    - Totals up two columns and divides
    //    - e.g., Goals/min
    //    - Terminology: Goals=numer; Min=denom
    class AveTotallingAction : BaseAction
    {
        
        // Constructor
        public AveTotallingAction(Field first_field, Field second_field)
            : base(first_field)
        {
            numer_tot = 0;
            denom_tot = 0;

            //numer_field = first_field;
            denom_field = second_field;
        }
        
        // Numerator running total
        double numer_tot;

        // denonminator running total
        double denom_tot;

        // There is already a Field in BaseAction
        // Numerator target field
        //Field numer_field;

        // Denominator target field
        Field denom_field;

        public override void doAction(SVRow r){

        }

        public override void print_me()
        {
        }//print_me



    }//class
}//namespace
