using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootyStatMVC1.Models.FootyStat.Actions
{
    // Interface to require the implmentation of an initialisation method
    // to be executed every time the action is recalculated.
    interface InitActionIface
    {

        void init();

    }
}
