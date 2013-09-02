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
    public class CreateIndexCommand : Command
    {

        public CreateIndexCommand(SnapViewDirector svd, string s)
            : base(svd)
        {
            field_name = s;
        }

        public override void execute()
        {
            IndexMC imc = (IndexMC) MCFactory.create_simple_MC(reciever, "IndexMC", field_name);
            reciever.Attach(imc);
        }

        // Name of the field to create the index for
        string field_name;

    }
}