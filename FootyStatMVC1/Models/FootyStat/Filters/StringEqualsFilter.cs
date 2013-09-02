using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootyStatMVC1.Models.FootyStat.SnapViewNS;
using FootyStatMVC1.Models.FootyStat.Actions;
using FootyStatMVC1.Models.FootyStat.Filters.KeepBehaviours;


namespace FootyStatMVC1.Models.FootyStat.Filters
{
    // Filter based on strings
    class StringEqualsFilter : EqualsFilter
    {
        // value of the filter (i.e., if its EQ "MNU" the val is "MNU")
        string val;
        // Target field
        //Field field;

        

        public StringEqualsFilter(string name, Field f, string v)
            : base(f, name)
        {
            val = v;
            //field = f;

            // Default to keep stuff.
            decision = true;
        }//Constructor





        // Uses polymorphism and strategy pattern to call the right 
        // filter behaviour
        //public override bool try_filter(SVRow r)
        //{
        //    return kBehaviour.keepIf(r.row[field.address()], val);
        //}

        public override void doAction(SVRow r)
        {
            // This we know this is a single cut val filter because all Equals filters are
            decision = kBehaviour.keepIf(r.row[field.address()], new SingleCutVal(val));
        }

        

    }
}
