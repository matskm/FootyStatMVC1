using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace FootyStatMVC1.Models.FootyStat.Filters.KeepBehaviours
{
    // Base class for the filter behaviours: KeepIfLessThan, KeepIfMoreThan, KeepIfEquals
    public abstract class KeepBehaviour
    {
        abstract public bool keepIf(string current_field, CutValues cut_vals);
    }
}
