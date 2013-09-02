using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FootyStatMVC1.Models.FootyStat.Mediator.Colleagues;
using FootyStatMVC1.Models.FootyStat.Mediator;

namespace FootyStatMVC1.Controllers.ConstraintViewModels
{
    // Base class for Constraint ViewModels
    //    - All constraints can be active or not (i.e., applied to the snapview or not)
    public abstract class BaseConstraintViewModel
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

        // Interface for creating ConstraintMC
        public abstract ConstraintMC generate_ConstraintMC(SnapViewDirector svd);

    }
}