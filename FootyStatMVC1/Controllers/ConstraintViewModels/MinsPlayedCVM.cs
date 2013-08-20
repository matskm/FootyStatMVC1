using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;

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

       



    }
}