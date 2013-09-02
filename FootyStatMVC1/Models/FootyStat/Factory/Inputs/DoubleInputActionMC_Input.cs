using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootyStatMVC1.Models.FootyStat.Factory.Inputs
{
    // MC_Input that has two extra parameters in addition to field_name
    public class DoubleInputActionMC_Input : MC_Input
    {
        public DoubleInputActionMC_Input(string field_name, string s1, string s2)
            : base(field_name)
        {
            par1 = s1;
            par2 = s2;
        }


        public string par1 { get; set; }

        public string par2 { get; set; }

    }
}