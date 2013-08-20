using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootyStatMVC1.Models.FootyStat.Filters.KeepBehaviours
{
    // Keep if the current field falls in a number range
    public class KeepIfInRange : KeepBehaviour
    {
        public override bool keepIf(string current_field, CutValues cut_vals)
        {
            if (cut_vals is DoubleCutVal)
            {
                DoubleCutVal dcv = (DoubleCutVal)cut_vals;
                double cut_val_min = Convert.ToDouble(dcv.cut_val_1);
                double cut_val_max = Convert.ToDouble(dcv.cut_val_2);

                double current_field_d = Convert.ToDouble(current_field);

                return (current_field_d >= cut_val_min && current_field_d <= cut_val_max);

            }
            else
            {
                // Throw an exception...
                return false;
            }
        }

    }
}