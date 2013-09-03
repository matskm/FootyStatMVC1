using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FootyStatMVC1.Models.FootyStat.Mediator;

namespace FootyStatMVC1.Controllers.SessionWrapper
{
    // This approach is from LukLed, on StackOverflow question 5060804
    public interface ISessionWrapper
    {
        SnapViewDirector svd { get; set; }
        
        // Initialise the svd
        void init_svd();
    }
}
