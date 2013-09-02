using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FootyStatMVC1.Models.FootyStat.SnapViewCommand.Commands;
using FootyStatMVC1.Models.FootyStat.Mediator;

namespace FootyStatMVC1.Models.FootyStat.SnapViewCommand
{
    // Class containing Commands.
    //   - First, Add commands 
    //   - Then when all commands are added, call the execute method.
    //   - Execute does all commands in sequence, and then calls the iterate on SVD (and therefore snapview).
    //   - NOTE: this should be the only entry point to the iterate on SVD (is there a way of enforcing this?)
    public class CommandInvoker
    {
        public CommandInvoker(SnapViewDirector the_svd)
        {
            svd = the_svd;
            command_list = new List<Command>();
        }

        // Also needs the svd
        SnapViewDirector svd;

        // List of commands
        List<Command> command_list;

        public void add_command(Command c)
        {
            command_list.Add(c);
        }

        public void execute_commands_and_iterate()
        {
            foreach (Command c in command_list)
            {
                c.execute();
            }

            // VERY IMPORTANT LINE: univeral entry point to iterate 
            svd.iterate_snapview();

        }

    }
}