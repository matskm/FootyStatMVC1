using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootyStatMVC1.Models.FootyStat.Filters.KeepBehaviours
{
    public class SingleCutVal : CutValues
    {

        public SingleCutVal(string s)
        {
            cut_val = s;
        }

        public string cut_val { get; set; }

    }
}