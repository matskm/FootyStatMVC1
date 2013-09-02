using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootyStatMVC1.Models.FootyStat.Factory.Inputs
{
    // Simplest possible MC_Input - just has the field_name which is already in the base class
    public class SimpleActionMC_Input : MC_Input
    {

        public SimpleActionMC_Input(string field_name)
            : base(field_name)
        {

        }

    }
}