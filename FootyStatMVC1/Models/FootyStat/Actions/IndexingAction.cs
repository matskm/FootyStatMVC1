using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootyStatMVC1.Models.FootyStat.SnapViewNS;

namespace FootyStatMVC1.Models.FootyStat.Actions
{
    // Index of all possible values of a particluar column (e.g., all team names)
    // Base class is BaseAction
    public class IndexingAction : BaseAction
    {
        // The index data structure (p868 of Herb)
        SortedSet<string> set;

        // Target field
        //Field field;



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


        public override void print_me()
        {
            Console.WriteLine("IndexingAction:print_me()");
            foreach (string s in set)
            {
                Console.WriteLine("   Set element: " + s);
            }//foreach
        }//print_me


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
