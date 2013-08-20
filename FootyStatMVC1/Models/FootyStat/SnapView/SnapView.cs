using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using FootyStatMVC1.Models.FootyStat.Mediator.Colleagues;
using FootyStatMVC1.Models.FootyStat.Mediator;
using FootyStatMVC1.Models.FootyStat.Actions;
using FootyStatMVC1.Models.FootyStat.Actions.Constraints;
using FootyStatMVC1.Models.FootyStat.Filters;


namespace FootyStatMVC1.Models.FootyStat.SnapViewNS
{
    // Description:
    //  - SnapView is probably the most important class in FootyStat
    //  - It contains a "table" representing a "SnapShot View" of the data
    //  - It is initialised when the database joins are done on startup. 
    //  - This first version of SnapView is kept as a starting reference point.
    //  - Internally, SnapView is a List< string[] > :
    //       - Dynamically grow-able array of string arrays.
    //5
    //  - Shallow copy the meta-data because its the same for every version of 
    //    a SnapView object.
    //  - Deep copy the action data because it could be different for subsequent 
    //    SnapView versions.
    // Usage:
    //  - SnapView is used by iterating over the table, and producing a new SnapView
    //  - Filters are thus implemented.
    //
    //
    //  Note: SnapView inherits from MediatorColleague so it can be registered with
    //        SnapViewDirector, which provides the interface with actions etc.


    public class SnapView : MediatorColleague
    {
        // **************
        // Data members:
        // **************

        // Main data structure: a dynamically grow-able List of string-arrays.
        List<SVRow> table;

        // Meta data (shallow copy) 
        FieldDictionary dict;

        // actions (deep copy or just reinit)
        //LinkedList<BaseAction> actions;

        // filters (deep copy or just reinit)
        //LinkedList<BaseFilter> filters;


        // ***************
        // Constructors
        // ***************

        // Copy constructor to Clone this snapview only used by FootyStatInit to save a copy right at the start.
        public SnapView(SnapViewDirector svd, SnapView previous_sv) : base(svd)
        {
            // Copy the table (expensive)
            table = previous_sv.table;

            // Create FieldDictionary object
            dict = previous_sv.dict;

            // isValid default to true for snapview (different to actions)
            isValid = true;
        }


        // Constructor default at the moment 
        public SnapView(SnapViewDirector svd) : base(svd)
        {
            // First lines here are what belongs in the constructor.

            // Create the table - initial size of 100, but List<> can grow so this doesn't matter too much.
            table = new List<SVRow>(100);

            //actions = new LinkedList<BaseAction>(); 

            //filters = new LinkedList<BaseFilter>(); // initial assumption is 10 filters. Will grow as necessary.

            // isValid default to true for snapview (different to actions)
            isValid = true;

        }//snapView constructor







        // ******************
        // Getters & Setters
        // ******************

        // Find a field (interface to FieldDicitonary)
        // (String str is the name of the field)
        public Field findInDict(string str)
        {
            return dict.find(str);
        }//findInDict

        //public void addAction(BaseAction a)
        //{
        //    actions.AddLast(a);
        //}//addAction

        //public void addFilter(BaseFilter f)
        //{
        //    filters.AddLast(f);
        //}

        // should this be public??
        void insert_row(SVRow str_ar)
        {
            table.Add(str_ar);
        }//insert_row

        // Add a row to the table data-structure
        public void addRow(SVRow row)
        {
            table.Add(row);
        }

        // Set the dictionary member
        public void setDict(FieldDictionary d)
        {
            dict = d;
        }





        // *****************************
        // Iteration Methods
        // (iterate over the main table)
        // *****************************

        // Default iteration over snapView that doesn't copy, or therefore filter. Just does the actions.
        public void default_iterate()
        {
            //foreach(string [] str_ar in table){

            //    // NOTE: this next little loop is very important: as we consider each row - do all the actions.
            //    // Do the actions
            //    foreach (BaseAction a in actions)
            //    {
            //        a.doAction(str_ar);
            //    }//foreach

            //}//foreach
        }//default_iterate



        void cache_projected_columns(List<MCAction> mca_list)
        {
            // Do this for every EqualsFilter we can find.

            foreach (MCAction mca in mca_list)
            {
                if (mca.get_action() is EqualsFilter)
                {
                    EqualsFilter ef = (EqualsFilter)mca.get_action();
                    Field f = ef.field;
                    SVRow first_row = table[0];
                    string val = first_row.row[f.address()];
                    ef.cache_projected_val(val);
                }
            }//foreach

        }


        public bool we_should_discard(List<MCAction> mca_list, SVRow svr)
        {
            bool skip = false;
            foreach (MCAction mca in mca_list)
            {
                if (mca.get_action() is BaseFilter)
                {
                    mca.doAction(svr);

                    // If the filter fails, skip this iteration.
                    if (
                        !(
                            (BaseFilter)mca.get_action()
                         ).decision
                        ) skip = true;

                }
            }//foreach

            return skip;
        }

        // Violates DRY - as method above is almost identical
        public bool we_should_skip(List<MCAction> mca_list, SVRow svr)
        {
            bool skip = false;
            foreach (MCAction mca in mca_list)
            {
                if (mca.get_action() is BaseConstraint)
                {
                    mca.doAction(svr);

                    // If the filter fails, skip this iteration.
                    if (
                        !(
                            (BaseConstraint)mca.get_action()
                         ).decision
                        ) skip = true;

                }
            }//foreach

            return skip;
        }



         

        public void iterate_and_filter(List<MCAction> mca_list)
        {
            

            // Need empty table local which will get filled with rows which pass the filters.
            List<SVRow> filtered_table = new List<SVRow>(100);

            // Need to update string[] to SVRow...
            foreach (SVRow svr in table)
            {

                // First consider filters, and if any fail, just continue to the next row without copying, indexing or calculating.
                
                if (we_should_discard(mca_list,svr)) continue;       

                

                // Copy the row now - but still need to do other actions . . . 
                filtered_table.Add(svr);

                // "Filter" on the Constraints without actually harming the data
                if (we_should_skip(mca_list, svr)) continue; 

                // Next consider indices
                foreach (MCAction mca in mca_list)
                {

                    mca.doAction(svr);
                    
                    //if (mca is IndexMC)
                    //{
                    //    IndexMC imc = (IndexMC)mca;
                    //    imc.doAction(svr);
                    //} 

                    //// Now consider constraints ...

                }//foreach



            }//foreach

            //Finally replace the table with the filtered version (note: this overwrites the table).
            table = filtered_table;

            // And cache any projected out columns
            cache_projected_columns(mca_list);

            // This is now handled by the SVD iterate
            //// And set the valid flag to true because this snapview is up to date with all its actions
            //isValid = true;

        }

        // Iterate over table, and only keep each row if filters pass
        public void filter_iterate(SnapView new_sv)
        {

            //// Loop over each row of the main table data structure
            //foreach (SVRow str_ar in table)
            //{

            //    bool keep = true;

            //    //// NOTE: this next little loop is very important: as we consider each row - do all the filters.
            //    //foreach (BaseFilter f in filters)
            //    //{
            //    //    if (!f.try_filter(str_ar)) keep = false;
            //    //}//foreach

            //    // Now we have the right value of keep
            //    if (keep)
            //    {
            //        new_sv.insert_row(str_ar);
                    
            //    }//if


            //}//foreach
        }//filter_iterate

        // Mediator iterate version
        public void iterate(List<MCAction> mca_list)
        {

            // Now we're in the iterate, armed with our action list

            // Need to update string[] to SVRow...
            foreach (SVRow svr in table)
            {

                // "Filter" on the Constraints without actually harming the data
                if (we_should_skip(mca_list, svr)) continue; 


                foreach (MCAction mca in mca_list)
                {

                    //// Next consider indices (will need to consider other kinds of actions here too]
                    //if (mca is IndexMC)
                    //{
                    //    IndexMC imc = (IndexMC)mca;
                    //    imc.doAction(svr);
                    //}

                    // Now Everything else... (prob don't need the test above hence commented out)
                    mca.doAction(svr);

                }//foreach
            }//foreach

            // This is now handled by the SVD iterate method
            //// And set the valid flag to true because this snapview is up to date with all its actions
            //isValid = true;

        }


        // *********************
        // Print methods (debug)
        // *********************

        public void print_table()
        {
            int row_n = 0;
            Console.WriteLine("SnapView:print_table()");

            foreach (SVRow arr in table)
            {
                Console.WriteLine("  Row[" + row_n + "]");
                foreach (string str in arr.row)
                {
                    Console.WriteLine("   " + str);
                }//foreach
                Console.WriteLine("");
                row_n++;
            }//loop over rows
        }//print_table

        //public void print_actions()
        //{
        //    foreach (BaseAction a in actions)
        //    {
        //        a.print_me();
        //    }//foreach
        //}//print_actions

    }//SnapView class
}//namespace
