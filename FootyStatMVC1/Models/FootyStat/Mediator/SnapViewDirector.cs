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
            
        }

        // Method to expose the findInDict method of the SnapView (access to fields)
        public Field findInDict(string str)
        {
            return get_snapview().findInDict(str);
        }

        // Utility for grabbing the snapview from the list of Colleagues
        // Not public because should not directly expose member MC snapview.
        SnapView get_snapview()
        {
            if (mySnapView != null) return mySnapView;

            // If we are here - that means we have to search for SnapView
            foreach (MediatorColleague mc in mcList) if (mc is SnapView) mySnapView = (SnapView)mc;
            
            // Either return the now non-null mySnapView or throw an exception and return.
            if (mySnapView != null) return mySnapView;
            else throw new SnapViewNotFoundException("Cannot find SnapView.");
        }
 
        // Most important MediatorColleague by far is the SnapView, so cache it.
        SnapView mySnapView;

        

        // This method polls the snapview to see whether it is "invalid" and if it is, triggers an interate
        public void iterate_snapview()
        {
            
            //// NEED TO THINK ABOUT WHETHER I NEED THIS..
            //// Poll all the MC's - if find >= 1 which is isValid==false then trigger iteration
            //bool all_mcs_valid = true;

            //foreach (MediatorColleague mc in mcList)
            //{
            //    if (!mc.isValid) all_mcs_valid = false;
            //}

            //// If the snapview is "not" valid - then we need to iterate
            //if (!all_mcs_valid)
            //{
                // Two kinds of iteration:
                //    1) If no filters involved, just iterate without changing snapview
                //    2) If there are filters involved, we have to create a new snapview

                bool filters_involved = false;
                
                // First collect all the "inValid" actions, and do any initialising

                List<MCAction> mca_list = new List<MCAction>(10);
                foreach(MediatorColleague mc in mcList){
                    if (mc is MCAction)
                    {
                        MCAction mca = (MCAction)mc;
                        
                        // ONLY add if this MC is inValid. If it's valid, it doesn't need to be re-calculated in the iteration
                        if (mca.isValid == false)
                        {


                            mca_list.Add(mca);

                            // test for filters
                            if (mca.get_action() is BaseFilter) filters_involved = true;

                            // NASTY: doing both class and interface versions of this. ONLY DO INTERFACE
                            // Call inits if necessary (E.g., all Stat calclation actions need to be re-initialised)
                            if (mca.get_action() is InitAction )
                            {
                                InitAction ia = (InitAction)(mca.get_action());
                                ia.init();
                            }
                            if (mca.get_action() is InitActionIface)
                            {
                                InitActionIface ia = (InitActionIface)(mca.get_action());
                                ia.init();
                            }


                        }// if isValid==false

                    }// if MCAction
                    
                }// foreach

                try
                {

                    // Do the iteration
                    if (!filters_involved)
                    {
                        get_snapview().iterate(mca_list);
                    }
                    else
                    {
                        get_snapview().iterate_and_filter(mca_list);
                    }

                }//try
                catch(SnapViewNotFoundException exc)
                {
                    System.Diagnostics.Debug.WriteLine(exc.ToString());
                }

                // Iterate over MC's and perform any Post methods
                // (E.g., caching the index in a field)
                foreach (MCAction mca in mca_list)
                {
                    
                    if (mca.get_action() is PostActionIface)
                    {
                        PostActionIface pa = (PostActionIface)mca.get_action();
                        pa.post();
                    }

                }

                // We have just done a fresh iteration. So set all MC's to isValid==true
                foreach (MediatorColleague mc in mcList)
                {
                    mc.isValid = true;
                }

                // Prob relocate this: Remove all FilterMC's because makes no sense to reuse
                var mc_node = mcList.First;
                while (mc_node != null)
                {
                    var nextNode = mc_node.Next;
                    if (mc_node.Value is FilterMC)
                    {
                        mcList.Remove(mc_node);
                    }
                    mc_node = nextNode;
                }


            //}//if !(all mc's valid)

          

        }// iterate_snapview



        // Pull out an action of type "mytype"
        // Return with the first one matching field_name
        // DEPRECATED : see get_totalling_action(..) below
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



 


        // Search for a valid index with the supplied field_name. If can't find, return null.
        public List<string> get_index(string idx_field_name)
        {
            List<string> rtn_idx = null;

            // Look for an IndexMC with a Field name matching idx_field_name.
            foreach (MediatorColleague mc in mcList)
            {
                if (mc is MCAction)
                {
                    MCAction mca = (MCAction)mc;
                    // Three parts to the condition: field name must match, must be an Index, and it must be valid
                    if (mca.get_action().field.name == idx_field_name && mca.get_action() is IndexingAction && mca.isValid == true)
                    {
                        IndexingAction ia = (IndexingAction)mca.get_action();
                        rtn_idx = ia.getStrLst();
                    }
                }
            }

            return rtn_idx;
        }


        // Search for a totalling action with the supplied field_name. If can't find, return null.
        public TotallingMC get_TotallingMC(string field_name)
        {
          
            // Look for an IndexMC with a Field name matching idx_field_name.
            foreach (MediatorColleague mc in mcList)
            {
                if (mc is MCAction)
                {
                    MCAction mca = (MCAction)mc;
                    // Two parts to the condition: field name must match, must be a TotallingAction
                    if (mca.get_action().field.name == field_name && mca is TotallingMC)
                        return (TotallingMC)mca;

                }// if
            }//foreach

            // If got here and not returned - throw exception and return
            return null;
        }

        public ConstraintMC get_matching_ConstraintMC(ConstraintMC target_cmc)
        {
            foreach (MediatorColleague mc in mcList)
            {
                if (mc is ConstraintMC)
                {
                    ConstraintMC cmc = (ConstraintMC)mc;

                    if (cmc.get_action().GetType().Name == target_cmc.get_action().GetType().Name) return cmc;

                    
                }
            }

            return null;
        }


        // **********************
        // Colleague registration
        // **********************

        // Linked List of Colleagues
        LinkedList<MediatorColleague> mcList;

        // Attach and detach for Colleague registration
        public void Attach(MediatorColleague mc)
        {
            // Before we add, need to check to see if other MC's need updating.
            // This is a key part of the SnapViewDirector "mediator" role - to coordinate its Colleagues.
            updateExistingMC(mc);
            
            mcList.AddLast(mc);
        }

        public void Detach(MediatorColleague mc)
        {
            // Before we detach, need to update other MC's (e.g., a removed Constraint invalidates stat MC's)
            // This is a key part of the SnapViewDirector "mediator" role - to coordinate its Colleagues.
            updateExistingMC(mc);

            mcList.Remove(mc);
            // Need exception for trying to remove something that isn't there
        }



        // Update existing MC's depending on what is about to be attached
        void updateExistingMC(MediatorColleague mc_change)
        {
            // If this is a filter or constraint - every MC gets set to "inValid" (subject to local overrides)
            // [Note: I may well end up putting every MC type in here - in which case change it]
            if (mc_change is FilterMC || mc_change is ConstraintMC)
            {
                foreach (MediatorColleague mc_other in mcList)
                {
                    mc_other.isValid = false;
                }
            }
        }



        // Exceptions
        
        // Cannot find associated SnapView
        class SnapViewNotFoundException : Exception
        {
            public SnapViewNotFoundException() : base() { }
            public SnapViewNotFoundException(string message) : base(message) { }
            public SnapViewNotFoundException(string message, Exception innerException) : base(message, innerException) { }
            protected SnapViewNotFoundException(System.Runtime.Serialization.SerializationInfo info,
                System.Runtime.Serialization.StreamingContext context)
                : base(info, context) { }

        }
        

    }// Class
}// Namespace