using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootyStatMVC1.Models.FootyStat.Mediator.Colleagues;
using FootyStatMVC1.Models.FootyStat.Actions;

namespace FootyStatMVC1.Models.FootyStat.SnapViewNS
{
    // Description: 
    // - Class providing Meta-data for an element of the SnapView row
    // - E.g., this class contains the integer address of this field within the row
    // - Objects are constructed during setup when the initial database is read in and SnapView created.
    public class Field
    {
        // Name of field (e.g., TeamName) read in from database/sandbox-data
        public string name { get; set; }

        // Display string of field (e.g., Team Name   note space)
        public string displayName { get; set; }

        // Description of field
        public string desc { get; set; }

        // Hash/Array address of this field within the SnapView row
        // (Note: this is very important data member for the overall design)
        int rowAddress;

        

        // We are associated to a snapview object for the purpose of building an index.
        // Relies on the snapview existing before the field is created.
        SnapView sView;



        // Access to the Index for this field (cached by the controller so we don't have to do >1 snapview iterate)
        // NOTE: potential failure mode is assuming this is a valid index, when the snapview has changed underneath.
        //          - Plan to build in a "valid" protection and update - probably by reorganising.
        IndexingAction index;

        public void set_index(IndexingAction i)
        {
            index = i;
        }

        // Pass the reference to List<string> by reference because it 
        // needs to change!
        public bool check_and_get_index(ref List<string> rtn_idx)
        {
            if (index == null)
            {
                rtn_idx = null;
                return false;
            }
            else
            {
                rtn_idx = index.getStrLst();
                return true;
            }
        }

        public List<string> get_index()
        {
            if (index == null)
            {
                // Throw an exception

                // Return an empty list for now (nasty!)
                return new List<string>(10);
            }

            return index.getStrLst();
        }

        



        // "Projected out" 
        //     - If we have filtered the snapview on a single value (e.g., season="2011-2012") the season field
        //       will only ever be one value (for every single row)
        //     - Thus it will be "Projected out"
        //     - This is "state" information which needs to be kept by the framework. Store in the field.
        public bool projectedOut { get; set; }
        // Holds the projected out value if we have been projected out
        public string projectedVal { get; set; }


        

        // Simple constructor
        public Field(string name_val, string displayName_val, string desc_val, int rowAddress_val, SnapView sv)
        {
            name = name_val;
            displayName = displayName_val;
            desc = desc_val;
            rowAddress = rowAddress_val;
            sView = sv;

            // Initialise index to null, even though this is default to enhance readablity
            index = null;

            // Initialise proejctedOut to false
            projectedOut = false;

        }//Field

        // Wrapper constructor for use by FieldDictionary
        public Field(string name_val)
        {
            name = name_val;
        }//Field

        

        // Use to access the correct element of the SnapView row
        public int address()
        {
            return rowAddress;
        }

    }//class

    // Comparer class
    class FieldComparer : IComparer<Field>
    {
        public int Compare(Field x, Field y)
        {
            // Compare based on the Field:name data member.
            return (x.name.CompareTo(y.name));
        }//Compare

    }//FieldComparer

}//namespace
