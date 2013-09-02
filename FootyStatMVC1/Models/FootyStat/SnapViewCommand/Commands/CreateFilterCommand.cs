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
    public class CreateFilterCommand : Command
    {
        public CreateFilterCommand(SnapViewDirector svd, string the_filter_field_name, string the_filter_value)
            : base(svd)
        {
            filter_field_name = the_filter_field_name;
            filter_value = the_filter_value;
        }

        public override void execute()
        {
            FilterMC fmc = (FilterMC)MCFactory.create_singleInput_MC(reciever, "StringEqualsFilterMC", filter_field_name, filter_value);
            reciever.Attach(fmc);
        }

        // Name of field to create filter for
        string filter_field_name;
        string filter_value;

    }
}