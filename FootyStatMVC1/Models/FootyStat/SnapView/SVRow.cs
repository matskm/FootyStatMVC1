using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootyStatMVC1.Models.FootyStat.SnapViewNS
{
    public class SVRow
    {
        // Ideally would have used a C++ typedef but this is close
        // This class defines the format of the snapview internal representation.
        // And helps to decouple the actual implementation from the places it is referenced.
        public string[] row { get; set; }
    }
}