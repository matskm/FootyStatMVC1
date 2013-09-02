using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FootyStatMVC1.Models.FootyStat.Mediator;
using FootyStatMVC1.Models.FootyStat.Factory;
using FootyStatMVC1.Models.FootyStat.Mediator.Colleagues;
// Alias
using MCFactory = FootyStatMVC1.Models.FootyStat.Factory.MCFactoryWrapper;

namespace FootyStatMVC1.Models.FootyStat.SnapViewCommand.Commands
{
    public class CreateTotallingActionCommand : Command
    {

        public CreateTotallingActionCommand(SnapViewDirector svd, string s)
            : base(svd)
        {
            field_name = s;
        }

        public override void execute()
        {
            TotallingMC tmc = (TotallingMC)MCFactory.create_simple_MC(reciever,"TotallingMC", field_name);
            reciever.Attach(tmc);
        }

        string field_name;


    }
}