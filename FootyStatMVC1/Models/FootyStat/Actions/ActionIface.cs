using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FootyStatMVC1.Models.FootyStat.SnapViewNS;

namespace FootyStatMVC1.Models.FootyStat.Actions
{
    // Action interface to ensure the snapview can "process" Colleagues (i.e., must provide a do method)
    interface ActionIface
    {
        void do_action(SVRow row);
    }
}
