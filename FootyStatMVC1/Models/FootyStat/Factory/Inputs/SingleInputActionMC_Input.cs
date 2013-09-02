using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootyStatMVC1.Models.FootyStat.Factory.Inputs
{
    // MC_Input that has one extra parameter in addition to field_name
    public class SingleInputActionMC_Input : MC_Input
    {

        public SingleInputActionMC_Input(string field_name, string s)
            : base(field_name)
        {
            par1 = s;
        }


        public string par1 { get; set; }
    }
}