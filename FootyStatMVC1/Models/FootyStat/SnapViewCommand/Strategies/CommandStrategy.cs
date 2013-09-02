using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FootyStatMVC1.Models.FootyStat.SnapViewCommand;
using FootyStatMVC1.Models.FootyStat.Mediator;

namespace FootyStatMVC1.Models.FootyStat.SnapViewCommand.Strategies
{
    // Base class for concrete CommandStrategy objects which define 
    // algorithms based on primitive commands.
    // Always uses a CommandInvoker to execute commands
    public class CommandStrategy
    {

        public CommandStrategy(SnapViewDirector svd)
        {
            invoker = new CommandInvoker(svd);
            receiver = svd;
        }

        // Always uses a CommandInvoker
        protected CommandInvoker invoker;
        protected SnapViewDirector receiver;
    }
}