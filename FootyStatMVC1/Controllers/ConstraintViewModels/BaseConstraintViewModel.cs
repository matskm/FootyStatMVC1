using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootyStatMVC1.Controllers.ConstraintViewModels
{
    // Base class for Constraint ViewModels
    //    - All constraints can be active or not (i.e., applied to the snapview or not)
    public class BaseConstraintViewModel
    {
        //public BaseConstraintViewModel(bool b)
        //{
        //    active = b;
        //}

        public BaseConstraintViewModel()
        {
            // Constraints default to false (disabled)
            active = false;
        }

        public bool active { get; set; }

    }
}