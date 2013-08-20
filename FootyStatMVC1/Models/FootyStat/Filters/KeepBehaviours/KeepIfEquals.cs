using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FootyStatMVC1.Models.FootyStat.Filters.KeepBehaviours
{
    // Keep if the current field is identical to the cut value.
    class KeepIfEquals : KeepBehaviour
    {
        public override bool keepIf(string current_field, CutValues cut_vals)
        {
            // Because we know this is a single-value cut - we can cast
            // to SingleCutVal
            if (cut_vals is SingleCutVal)
            {
                SingleCutVal scv = (SingleCutVal)cut_vals;
                return (current_field == scv.cut_val);
            }
            else
            {
                // Throw exception ...
                return false;
            }

            
        }//KeepIf
    }
}
