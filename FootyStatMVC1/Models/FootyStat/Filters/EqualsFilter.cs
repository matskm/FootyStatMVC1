using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FootyStatMVC1.Models.FootyStat.SnapViewNS;
using FootyStatMVC1.Models.FootyStat.Filters.KeepBehaviours;

namespace FootyStatMVC1.Models.FootyStat.Filters
{
    // Special filter which selects out single values of fields
    //   - Only filters of this type can "project out" a column of the table
    //   - When they are encountered by the SVD, the projected out value is 
    //     cached in the Field
    //   - Because this is by definition an "equals" filter, this behaviour 
    //     is hardwired.
    public class EqualsFilter : BaseFilter
    {
        
        // Constructor
        public EqualsFilter(Field f, string n)
            : base(f, n, new KeepIfEquals())
        {

        }
        
        // Cache the projected out value
        public void cache_projected_val(string val)
        {
            field.projectedOut = true;
            field.projectedVal = val;
        }
    }
}