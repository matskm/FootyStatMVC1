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
            // Only allow one [this-key,any-value] pair per session - so remove an existing object before adding the new one
            if (GetFromSession<SnapViewDirector>(key) != null)
            {
                HttpContext.Current.Session.Remove(key);
            }

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
            // Only the initial init if there is not already an entry for the svd in the current session:
            if (GetFromSession<SnapViewDirector>("svd") == null)
            {

                // Order is important as snapview needs svd in its constructor -> So create it before snapview
                SnapViewDirector svd_local = new SnapViewDirector();

                SnapView snapview = null;

                // If the _initial_SnapView static member is null - read from disk (once per app instance)
                // Otherwise copy the _initial_SnapView (once per new session)
                if (_initial_SnapView == null)
                {
                    snapview = loadData(svd_local);
                }
                else
                {
                    // Copy the initial snapview (contains a deep copy of the table, but shallow copy of the dict)
                    snapview = new SnapView(svd_local, _initial_SnapView);
                }

                // And register the snapview with the director           
                svd_local.Attach(snapview);


                // Set this for the current session
                SetInSession("svd", svd_local);
            }
            else
            {
                // If init has been called, but an svd already exists - reinitialise this svd.
                // (This occurs if the user closes the window in the session, and navigates to the root SelectPlayer page)

                // The setter automatically cleans out an exisiting svd in the session if its there.
                svd = new SnapViewDirector();

                // Property getter.
                SnapViewDirector svd_ref = svd;


                SnapView snapview = new SnapView(svd, _initial_SnapView);
                svd.Attach(snapview);
            }

            
        }

        public SnapView loadData(SnapViewDirector svd)
        {
            // Create root level snapview here (passing in the SnapViewDirector)
            _initial_SnapView = new SnapView(svd);

            // Initialise it
            XmlInit svInit = new XmlInit();
            svInit.initSnapView(_initial_SnapView);

            return _initial_SnapView;

        }

        // Static expt
        static SnapView _initial_SnapView;

    }
}