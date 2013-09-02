using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FootyStatMVC1.Models.FootyStat.Mediator;
using FootyStatMVC1.Models.FootyStat.SnapViewNS;
using FootyStatMVC1.Models.FootyStat.Init;

namespace FootyStatMVC1
{
    // Behaviour: if either of SnapView or SnapViewDirector are null, both are recreated.
    public class FootyStatInit
    {
        // Static data member accessed by getter
        static SnapView snapview;

        // Snapshot of the inital snapview (for starting over)
        static SnapView initial_snapview;

        // Static Mediator controller object
        static SnapViewDirector svd;


        // Static getter (which create new snapview and svd if either one is null)
        public static SnapViewDirector get_svd()
        {
            if (svd != null) return svd;
            else
            {
                init_view_and_director();
                return svd;

            }
        }

        // Create SnapView and then SnapViewDirector
        //  - Register SnapView with director.
        static void init_view_and_director()
        {
            // Order is important as snapview needs svd in its constructor -> So create it before snapview
            svd = new SnapViewDirector();
            snapview = loadData();
            
            // And register the snapview with the director           
            svd.Attach(snapview);


            // Make a copy of the snapview for the initial view (only the table needed and default values - nothing else is filled at this point)
            initial_snapview = new SnapView(svd,snapview);

        }

        public static SnapView loadData()
        {
            // Create root level snapview here (passing in the SnapViewDirector)
            SnapView firstSnapView = new SnapView(svd);

            

            // Initialise it
            XmlInit svInit = new XmlInit();
            svInit.initSnapView(firstSnapView);

            return firstSnapView;

        }

    }
}