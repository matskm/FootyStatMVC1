using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

// Base class for objects containing input parameters to the MC factories
namespace FootyStatMVC1.Models.FootyStat.Factory.Inputs
{
    public abstract class MC_Input
    {

        public MC_Input(string s)
        {
            field_name = s;
        }

        // All MC_Inputs have field_name
        public string field_name { get; set; }

    }
}