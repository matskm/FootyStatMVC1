using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootyStatMVC1.Models.FootyStat.SnapViewNS;

namespace FootyStatMVC1.Models.FootyStat.Actions
{
    // Index of all possible values of a particluar column (e.g., all team names)
    // Base class is BaseAction, implments PostActionIface interface.
    public class IndexingAction : BaseAction, InitActionIface
    {
        // The index data structure (p868 of Herb)
        SortedSet<string> set;

        // Constructor
        public IndexingAction(Field f)
            : base(f)
        {
            //field = f;
            set = new SortedSet<string>(new IASetComparer());

        }//Constructor


        // Process one row of the SnapView
        public override void doAction(SVRow r){
            // Input the target element of the row string
            // If it exists in the Set, don't add it. Otherwise add it.

            // Get the Field address:
            int addr = field.address();
            
            // Add value at address to the index
            set.Add(r.row[addr]);
        }

        
        // Get a representation of the set as just an array of strings
        public string[] getStrArr()
        {
            string[] strArr = new string[set.Count()];
            int i=0;
            foreach (string s in set)
            {
                strArr[i] = s;
                i++;
            }

            return strArr;
        }

        // Return size of index set
        public int size()
        {
            return set.Count;
        }

        // List version
        public List<string> getStrLst()
        {
            List<string> strLst = new List<string>(10);
            int i = 0;
            foreach (string s in set)
            {
                strLst.Add(s);
                i++;
            }

            return strLst;
        }


        // Initialisation
        public void init()
        {
            // Reinitialise the set
            set = new SortedSet<string>(new IASetComparer());
        }

  
    }//class

    // Helper comparer for the SortedSet<string>
    public class IASetComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            return x.CompareTo(y);
        }//Compare
    }//class

}// namespace
