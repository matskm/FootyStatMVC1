using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FootyStatMVC1.Models.FootyStat.Mediator;

namespace FootyStatMVC1.Models.FootyStat.SnapViewCommand.Commands
{
    // Base class for primitive commands
    public abstract class Command
    {

        public Command(SnapViewDirector svd)
        {
            reciever = svd;
        }

        public abstract void execute();
       
        // Reciever is always the SVD (SnapViewDirector) so put in base class.
        public SnapViewDirector reciever;

    }
}