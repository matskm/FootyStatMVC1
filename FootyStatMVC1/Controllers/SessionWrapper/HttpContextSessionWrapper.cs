using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FootyStatMVC1.Models.FootyStat.Mediator;
using FootyStatMVC1.Models.FootyStat.SnapViewNS;
using FootyStatMVC1.Models.FootyStat.Init;

namespace FootyStatMVC1.Controllers.SessionWrapper
{
    // This approach is from LukLed, on StackOverflow question 5060804
    public class HttpContextSessionWrapper : ISessionWrapper
    {
        private T GetFromSession<T>(string key)
        {
            return (T)HttpContext.Current.Session[key];
        }

        private void SetInSession(string key, object value)
        {
            HttpContext.Current.Session[key] = value;
        }

        // Here is the implementation of the ISessionWrapper interface:
        public SnapViewDirector svd
        {
            get { return GetFromSession<SnapViewDirector>("svd"); }
            set { SetInSession("svd", value); }
        }

        // Initialise the svd
        public void init_svd()
        {
            // Only init if there is not already an entry for the svd in the current session:
            if (GetFromSession<SnapViewDirector>("svd") == null)
            {
            
                // Order is important as snapview needs svd in its constructor -> So create it before snapview
                SnapViewDirector svd = new SnapViewDirector();
                SnapView snapview = loadData(svd);

                // And register the snapview with the director           
                svd.Attach(snapview);

            
                // Set this for the current session
                SetInSession("svd", svd);
            }

            
        }

        public SnapView loadData(SnapViewDirector svd)
        {
            // Create root level snapview here (passing in the SnapViewDirector)
            SnapView snapView = new SnapView(svd);

            // Initialise it
            XmlInit svInit = new XmlInit();
            svInit.initSnapView(snapView);

            return snapView;

        }

    }
}