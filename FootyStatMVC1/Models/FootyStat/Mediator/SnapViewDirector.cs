using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FootyStatMVC1.Models.FootyStat.Mediator.Colleagues;
using FootyStatMVC1.Models.FootyStat;
using FootyStatMVC1.Models.FootyStat.SnapViewNS;
using FootyStatMVC1.Models.FootyStat.Filters;
using FootyStatMVC1.Models.FootyStat.Actions;
using System.Reflection;


namespace FootyStatMVC1.Models.FootyStat.Mediator
{
    // Implementing the "Mediator" pattern - this is the Mediator class.
    //   - Colleagues include the SnapView, and all filters/indices
    public class SnapViewDirector
    {

        public SnapViewDirector()
        {
            mcList = new LinkedList<MediatorColleague>();
            RegistrationCandidateList = new List<MediatorColleague>();
        }

        // Utility for grabbing the snapview from the list of Colleagues
        SnapView get_snapview()
        {
            SnapView tmp_sv = null;
            foreach(MediatorColleague mc in mcList){
                if (mc is SnapView) tmp_sv = (SnapView) mc;
            }

            if (tmp_sv == null)
            {
                // throw an exception
            }

            return tmp_sv;
        }
 

        // This is wrong design: the SVD will poll all its MC's to see if something has changed at the approriate time.

        //public void ColleagueChanged(MediatorColleague mc)
        //{
        //    // This is where the magic happens . . . (logic of what to do when stuff changes)

        //    // First version of behaviour: if we encounter a MCAction - set the snapview isValid flag to false.

        //    // Cast mc to a MCAction...
        //    // This covers Filters, Indices and Stats
        //    if(mc is MCAction){
        //        // If successful, set the snapview isValid to false - which will later trigger an iteration
        //        get_snapview().isValid = false;
        //    }

            
        //}

        // This method polls the snapview to see whether it is "invalid" and if it is, triggers an interate
        public void iterate_snapview()
        {
            
            // Poll all the MC's - if find >= 1 which is isValid==false then trigger iteration
            bool all_mcs_valid = true;

            foreach (MediatorColleague mc in mcList)
            {
                if (!mc.isValid) all_mcs_valid = false;
            }

            // If the snapview is "not" valid - then we need to iterate
            if (!all_mcs_valid)
            {
                // Two kinds of iteration:
                //    1) If no filters involved, just iterate without changing snapview
                //    2) If there are filters involved, we have to create a new snapview

                bool filters_involved = false;
                
                // First collect all the actions, and do any initialising

                List<MCAction> mca_list = new List<MCAction>(10);
                foreach(MediatorColleague mc in mcList){
                    if (mc is MCAction)
                    {
                        MCAction mca = (MCAction)mc;
                        
                        mca_list.Add(mca);

                        // test for filters
                        if (mca.get_action() is BaseFilter) filters_involved = true;

                        // Call inits if necessary
                        if (mca.get_action() is InitAction)
                        {
                            InitAction ia = (InitAction)(mca.get_action());
                            ia.init();
                        }

                    }
                    
                }

                // Do the iteration
                if (!filters_involved)
                {
                    get_snapview().iterate(mca_list);
                }
                else
                {
                    get_snapview().iterate_and_filter(mca_list);
                }

                // We have just done a fresh iteration. So set all MC's to isValid==true
                foreach (MediatorColleague mc in mcList)
                {
                    mc.isValid = true;
                }

            }//if

          

        }// iterate_snapview



        // Pull out an action of type "mytype"
        // Return with the first one matching field_name
        public TotallingAction pullOutTotallingAction(string field_name)
        {
            foreach(MediatorColleague mc in mcList){

                if (mc is MCAction)
                {
                    MCAction mca = (MCAction)mc;

                    BaseAction ba = mca.get_action();

                    

                    if (ba is TotallingAction && ba.field.name == field_name)
                    {
                        return ba as TotallingAction;
                    }

                }

            }

            // If not returned by now, haven't found one.
            return null;
        }



        // Clean out all ConstraintMC's ready for a new lot (these can change every time the page is loaded)
        public void remove_all_ConstraintMC(){

            
            // Use a do-while loop because we have to modify the collection underneath the iteration
            bool isNext = false;
            LinkedListNode<MediatorColleague> node = mcList.First;
            // If its not empty
            if (node != null)
            {
                do
                {
                    // First decide whether we need a further iteration, and store the next node.
                    if (node.Next != null) { 
                        // Trigger a further iteration
                        isNext = true; 
                        
                        // Prepare node for next iteration
                        node = node.Next; 

                        // Do the test for removal, and remove if this is a Constraint
                        if(node.Value is ConstraintMC ){                           
                            Detach(node.Value);
                        }
                    
                    }
                    else isNext = false;

                } while (isNext); // loop condition
            }// if the list isn't empty
            
            
        }





        // **********************
        // Colleague registration
        // **********************

        // Linked List of Colleagues
        LinkedList<MediatorColleague> mcList;

        // Attach and detach for Colleague registration
        public void Attach(MediatorColleague mc)
        {
            mcList.AddLast(mc);
        }

        public void Detach(MediatorColleague mc)
        {
            mcList.Remove(mc);
            // Need exception for trying to remove something that isn't there
        }

        // **********************
        // Colleague registration candidate framework
        // **********************

        // Design:
        //   - We need a way of deferring the iteration of the snapview until >1 MC's have been registered.
        //   - So have a "registration candidate" list of MediatorColleagues.
        //   - This is a "temporary" list which is added to by using AddRegistrationCandidate(MediatorColleague)
        //   - And when we're done building the temp list, we call RegisterCandidates()
        //        - This actually registers all the candidate with the SVD (this object)
        //        - It reinitialises the RegistrationCandidateList
        //        - And *crucially* it triggers the iteration.

        List<MediatorColleague> RegistrationCandidateList;

        // Add this mc to our candidate list 
        public void AddRegistrationCandidate(MediatorColleague mc)
        {
            RegistrationCandidateList.Add(mc);
        }

        // Go through the whole candidate list and register all of them, re-init the candidate list, and trigger iteration
        public void RegisterCandidates()
        {
            // Register all the candidates
            foreach (MediatorColleague mc in RegistrationCandidateList)
            {
                Attach(mc);
            }

            // Re-init the candidate list
            RegistrationCandidateList = new List<MediatorColleague>();


            // *************************
            // VERY IMPORTANT LINE:
            // Trigger the iteration
            // *************************
            iterate_snapview();

        }

    }
}