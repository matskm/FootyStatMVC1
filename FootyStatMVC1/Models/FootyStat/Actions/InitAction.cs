using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FootyStatMVC1.Models.FootyStat.SnapViewNS;

namespace FootyStatMVC1.Models.FootyStat.Actions
{
    // Actions which need an "init" before every iteration over snapview
    public abstract class InitAction : BaseAction
    {

        public abstract void init();

        public InitAction(Field f)
            : base(f)
        {

        }

        

    }
}