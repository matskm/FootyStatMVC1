using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject.Modules;
using Ninject;
using FootyStatMVC1.Controllers.SessionWrapper; 

namespace FootyStatMVC1.App_Start
{
    // Define runtime dependencies for the FootyStat
    //  - In particular, the PlayerStatController has the concrete type of its constructor parameter 
    //    defined by this mechanism (Dependency Injection).
    public class FootyStatNinjectBindings : NinjectModule
    {

        public override void Load()
        {
            Bind<ISessionWrapper>().To<HttpContextSessionWrapper>();
        }

    }
}