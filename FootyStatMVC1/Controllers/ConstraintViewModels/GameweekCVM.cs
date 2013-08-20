using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;


namespace FootyStatMVC1.Controllers.ConstraintViewModels
{
    public class GameweekCVM : BaseConstraintViewModel
    {

        // Inherit default deactivated
        public GameweekCVM()
            : base()
        {
            min = 0;
            max = 38; //should get this from locale constant
        }

        // Gameweek members
        [DisplayName("Min")]
        [Range(1, 38)]
        [Integer(ErrorMessage="This is needs to be integer")]
        public int min { get; set; }

        [DisplayName("Max")]
        [Range(1, 38)]
        [Integer(ErrorMessage = "This is needs to be integer")]
        public int max { get; set; }

        

    }
}