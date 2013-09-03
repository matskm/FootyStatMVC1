using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using System.Reflection;
using FootyStatMVC1.Controllers.SessionWrapper;
using System.Web.SessionState;

namespace FootyStatMVC1.Controllers
{
    // Controller factory to provide one parameter in the constructor for injecting dependency into the controller
    public class FootyStatControllerFactory : IControllerFactory
    {

        public IController CreateController(RequestContext requestContext, string controllerName)
        {
         
            IKernel kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());

            // Get the concrete ISessionWrapper implementation from Ninject
            ISessionWrapper sessionWrapper = kernel.Get<ISessionWrapper>();
            PlayerStatController controller = new PlayerStatController(sessionWrapper);

            return controller;
            
        }

        public void ReleaseController(IController controller)
        {
            var disposable = controller as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }

        public SessionStateBehavior GetControllerSessionBehavior(RequestContext requestContext, string controllerName)
        {
            return SessionStateBehavior.Default;
        }

    }//class
}//namespace