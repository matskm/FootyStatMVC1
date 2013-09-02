using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FootyStatMVC1.Models.FootyStat.Mediator;
using FootyStatMVC1.Models.FootyStat.SnapViewCommand;
using FootyStatMVC1.Models.FootyStat.SnapViewCommand.Commands;
using FootyStatMVC1.Models.FootyStat.SnapViewNS;

namespace FootyStatMVC1.Models.FootyStat.SnapViewCommand.Strategies
{
    public class Filter_CS : CommandStrategy
    {

        public Filter_CS(SnapViewDirector receiver)
            : base(receiver)
        {

        }

        public void execute(string filter_field_name, string filter_value)
        {
            //CommandInvoker invoker = new CommandInvoker(svd);

            CreateFilterCommand cmd = new CreateFilterCommand(receiver, filter_field_name, filter_value);
            invoker.add_command(cmd);

            invoker.execute_commands_and_iterate();
        }

    }
}