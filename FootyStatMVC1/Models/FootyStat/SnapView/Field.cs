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


        // Simple constructor
        public Field(string name_val, string displayName_val, string desc_val, int rowAddress_val, SnapView sv)
        {
            name = name_val;
            displayName = displayName_val;
            desc = desc_val;
            rowAddress = rowAddress_val;
            sView = sv;

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
