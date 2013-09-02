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
    // Command strategy for updating an index (either gets a cached value, or creates a new one)
    public class UpdateIndex_CS : CommandStrategy
    {

        public UpdateIndex_CS(SnapViewDirector svd)
            : base(svd)
        {

        }

        public List<string> execute(string idx_field_name)
        {
            List<string> rtn_idx = receiver.get_index(idx_field_name);
            
            if(rtn_idx == null){
            
                //CommandInvoker invoker = new CommandInvoker(svd);

                CreateIndexCommand cmd = new CreateIndexCommand(receiver, idx_field_name);

                invoker.add_command(cmd);

                invoker.execute_commands_and_iterate();

                rtn_idx = receiver.get_index(idx_field_name);
            }
            
            // By now this should either be set from the cache value
            // or recalculated.
            if (rtn_idx != null)
            {
                return rtn_idx;
            }
            else
            {
                // throw exception
                return null;
            }

        }// execute

    }
}