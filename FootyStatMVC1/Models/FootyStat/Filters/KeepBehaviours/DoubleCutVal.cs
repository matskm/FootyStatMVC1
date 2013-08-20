using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootyStatMVC1.Models.FootyStat.Filters.KeepBehaviours
{
    public class DoubleCutVal : CutValues
    {

        public DoubleCutVal(string s1, string s2)
        {
            cut_val_1 = s1;
            cut_val_2 = s2;
        }

        public string cut_val_1 { get; set; }
        public string cut_val_2 { get; set; }

    }
}