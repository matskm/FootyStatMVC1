using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FootyStatMVC1.Models.FootyStat.SnapViewNS
{
    // Description:
    //   - Class containing a list of Field objects
    //   - A Field object tells us about an element in a SnapView row
    //   - A reference to an instantiation of this class lives inside SnapView
    //   - So SnapView contains a "Dictionary" that describes its data
    //   - Interface: 
    //        - have to be able to add Fields
    public class FieldDictionary
    {
        // The internal data-structure (array of Field objects)
        public List<Field> dict;

        // Sorted flag.
        bool sorted = false;

        // Comparer helper
        FieldComparer fcomp;

        public FieldDictionary(int initial_size)
        {
            dict = new List<Field>(initial_size);
            fcomp = new FieldComparer();
        }

        public void print_me(){

            foreach(Field f in dict){
                Console.WriteLine("  Field Name: " + f.name);
                Console.WriteLine("  Field displayName: " + f.displayName);
                Console.WriteLine("  Field desc: " + f.desc);
                Console.WriteLine("  Field address: " + f.address());
                Console.WriteLine("");
            }//foreach
        }

        public Field find(string s)
        {
            // Create wrapper Field object just with s string in name
            // because search compare methods only operate on the name string
            Field f = new Field(s);
            return find(f);

        }//find on string

        public Field find(Field f)
        {
            // If not sorted, sort the table.
            if (!sorted) sort_table();

            return dict[ dict.BinarySearch(f,fcomp) ];

        }//find

        private void sort_table()
        {
            // Very compact sort: use lambda expression to indicate that only sorting on the "name" member of Field.
            dict.Sort((t1, t2) => t1.name.CompareTo(t2.name));

            sorted = true;
        }//sort_table


    }//class


    
    

}//nameapce
