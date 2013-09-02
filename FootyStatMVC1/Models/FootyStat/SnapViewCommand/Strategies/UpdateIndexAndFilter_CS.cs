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
    // Updates an index and filters at the same time
    public class UpdateIndexAndFilter_CS : CommandStrategy
    {

        public UpdateIndexAndFilter_CS(SnapViewDirector receiver)
            : base(receiver)
        {

        }

        public List<string> execute(
                     string idx_field_name,
                     string filter_field_name,
                     string filter_value
                    )
        {
            Field f_idx = receiver.findInDict(idx_field_name);
            Field f_ftr = receiver.findInDict(filter_field_name);
            List<string> rtn_idx = receiver.get_index(idx_field_name);

            if (rtn_idx == null)
            {
                //CommandInvoker invoker = new CommandInvoker(receiver);

                CreateIndexCommand cmd1 = new CreateIndexCommand(receiver, idx_field_name);
                invoker.add_command(cmd1);

                CreateFilterCommand cmd2 = new CreateFilterCommand(receiver, filter_field_name, filter_value);
                invoker.add_command(cmd2);

                invoker.execute_commands_and_iterate();

                rtn_idx = receiver.get_index(idx_field_name);

            }

            // By now this should either be set from the cache value
            // or recalculated.
            if (rtn_idx != null) return rtn_idx;
            else return null; // throw exception
            


        }

    }
}