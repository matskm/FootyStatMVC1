using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FootyStatMVC1.Models.FootyStat.Mediator;
using FootyStatMVC1.Models.FootyStat.SnapViewCommand;
using FootyStatMVC1.Models.FootyStat.SnapViewCommand.Commands;
using FootyStatMVC1.Models.FootyStat.SnapViewNS;
using FootyStatMVC1.Controllers;
using FootyStatMVC1.Models.FootyStat.Actions;
using FootyStatMVC1.Models.FootyStat.Mediator.Colleagues;

namespace FootyStatMVC1.Models.FootyStat.SnapViewCommand.Strategies
{
    public class UpdateAllStats_CS : CommandStrategy
    {

        public UpdateAllStats_CS(SnapViewDirector svd)
            : base(svd)
        {

        }

        public CurrentPlayerStatsViewModel execute()
        {
           
            string str1 = FieldDictionary.fname_goalScdPlayer;
            string str2 = FieldDictionary.fname_assistsPlayer;
            
            // Look for Valid TotallingActions for each stat.
            // Treat them as a "block" of TotallingActions for now.
            //   - If don't find them, create them and iterate.
            //   - If do find them but they're invalid - just iterate.
            //   - If do find them and they're valid - just pass back results.
            TotallingMC t1 = receiver.get_TotallingMC(str1);
            TotallingMC t2 = receiver.get_TotallingMC(str2);

            // If they don't exist yet.
            if (t1 == null && t2 == null)
            {
                // The form of this method is bound to  CurrentPlayerStatsViewModel
                CreateTotallingActionCommand cmd1 = new CreateTotallingActionCommand(receiver, str1);
                invoker.add_command(cmd1);

                CreateTotallingActionCommand cmd2 = new CreateTotallingActionCommand(receiver, str2);
                invoker.add_command(cmd2);

                invoker.execute_commands_and_iterate();
            }
            // They exist, but they're not valid (iterate)
            else if (!t1.isValid && !t2.isValid)
            {
                // No commands needed. Just iterate (so this executes an empty command list)
                invoker.execute_commands_and_iterate();
            }
            else
            {
                // Test to see if they exist, and they're valid
                if (!(t1.isValid && t2.isValid))
                {
                    // SHouldn't be here - throw exception!
                    return null;
                }
            }//else

            // These are guaranteed to exist now:
            TotallingAction post_t1 = (TotallingAction)receiver.get_TotallingMC(str1).get_action();
            TotallingAction post_t2 = (TotallingAction)receiver.get_TotallingMC(str2).get_action();

            
            return new CurrentPlayerStatsViewModel(
                                                   Convert.ToInt16(post_t1.total), 
                                                   Convert.ToInt16(post_t2.total)
                                                  );
            


        }//execute

    }
}