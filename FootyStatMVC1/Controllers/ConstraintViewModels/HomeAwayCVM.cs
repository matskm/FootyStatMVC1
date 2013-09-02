using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FootyStatMVC1.Models.FootyStat.Mediator.Colleagues;
using FootyStatMVC1.Models.FootyStat.Mediator;
using FootyStatMVC1.Models.FootyStat.Actions.Constraints;
using FootyStatMVC1.Models.FootyStat.SnapViewNS;

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

        // Generate ConstraintMC
        public override ConstraintMC generate_ConstraintMC(SnapViewDirector svd)
        {
            Field f = svd.findInDict(FieldDictionary.fname_homeAway);
            HomeAwayConstraintAdapter adapter = new HomeAwayConstraintAdapter(f, IsHome);
            return new ConstraintMC(svd, adapter.adaptee);
        }
    }
}