using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Database.Models;

namespace YSI.CurseOfSilverCrown.Core.Commands
{
    public abstract class BaseCommand : Command
    {
        public abstract string Name { get; }
        public abstract string[] Descriptions { get; }

        public abstract bool NeedTarget { get; }
        public abstract string TargetName { get; }

        public abstract bool NeedTarget2 { get; }
        public abstract string Target2Name { get; }

        public abstract bool NeedCoffers { get; }

        public abstract bool NeedWarriors { get; }


        public BaseCommand(Command command)
        {
            if (command == null)
                return;

            Id = command.Id;
            OrganizationId = command.OrganizationId;
            Coffers = command.Coffers;
            Warriors = command.Warriors;
            Type = command.Type;
            TargetOrganizationId = command.TargetOrganizationId;
            Target2OrganizationId = command.Target2OrganizationId;
            InitiatorOrganizationId = command.InitiatorOrganizationId;
            Status = command.Status;
        }
    }
}
