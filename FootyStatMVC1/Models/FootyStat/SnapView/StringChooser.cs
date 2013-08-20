using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FootyStatMVC1.Models.FootyStat.SnapViewNS
{
    // Helper class for console version of FootyStat
    //   - choose(...) accepts a string[] as input
    //   - presents the user with each string in the array numbered
    //   - accepts user choice
    //   - returns the string chosen
    class StringChooser
    {
        
        public string choose(string[] sArr){
            Console.WriteLine("Chooser . . .");
            int i=1;
            foreach(string s in sArr)
            {
                Console.WriteLine(i + ". " + s);
                i++;
            }

            Console.WriteLine("Please key in your choice . . . ");
            string sChoice = Console.ReadLine();

            int iChoice = Convert.ToInt16(sChoice);
            iChoice--;

            return sArr[iChoice];

        }//choose
    }
}
