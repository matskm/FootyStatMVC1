using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;
using FootyStatMVC1.Models.FootyStat.Mediator.Colleagues;
using FootyStatMVC1.Models.FootyStat.Mediator;
using FootyStatMVC1.Models.FootyStat.Actions.Constraints;
using FootyStatMVC1.Models.FootyStat.SnapViewNS;

namespace FootyStatMVC1.Controllers.ConstraintViewModels
{
    // View model for Mins Played constraint
    public class MinsPlayedCVM : BaseConstraintViewModel
    {

        public MinsPlayedCVM()
            : base()
        {
            val = 90;
          
        }

        // Gameweek members
        [DisplayName("Mins Played")]
        [Range(0, 90)]
        [Integer(ErrorMessage = "This is needs to be integer")]
        public int val { get; set; }

       // Generate ConstraintMC
        public override ConstraintMC generate_ConstraintMC(SnapViewDirector svd)
        {
            // This should not be hardcoded - but factored out
            Field f = svd.findInDict(FieldDictionary.fname_minsPlayed);
            MinsPlayedConstraintAdapter adapter = new MinsPlayedConstraintAdapter(f, val);
            return new ConstraintMC(svd, adapter.adaptee);
        }

    }
}