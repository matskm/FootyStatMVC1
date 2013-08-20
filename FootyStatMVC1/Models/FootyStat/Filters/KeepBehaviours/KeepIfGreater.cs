using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootyStatMVC1.Models.FootyStat.Filters.KeepBehaviours
{
    // Keep if the current field is greater than (or equal to) the cut value (attempts cast to numbers)
    public class KeepIfGreater : KeepBehaviour
    {

        public override bool keepIf(string current_field, CutValues cut_vals)
        {
            if (cut_vals is SingleCutVal)
            {
                SingleCutVal scv = (SingleCutVal)cut_vals;
                double current_field_d = Convert.ToDouble(current_field);
                double cut_val_d = Convert.ToDouble(scv.cut_val);

                return (current_field_d >= cut_val_d);
            }
            else
            {
                // Throw exception
                return false;
            }
        }

    }
}