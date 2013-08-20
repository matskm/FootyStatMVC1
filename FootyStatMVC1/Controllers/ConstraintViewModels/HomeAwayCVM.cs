using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootyStatMVC1.Controllers.ConstraintViewModels
{
    // Constraint ViewModel for HomeAway constraint
    public class HomeAwayCVM : BaseConstraintViewModel
    {

        //public HomeAwayCVM(bool b = false)
        //    : base(b)
        //{

        //}

        public HomeAwayCVM()
            : base()
        {
            
        }

        // Value member
        public bool IsHome { get; set; }
    }
}