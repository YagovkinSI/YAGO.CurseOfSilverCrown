using YSI.CurseOfSilverCrown.Core.Database.Commands;
using YSI.CurseOfSilverCrown.Core.Database.Domains;

namespace YSI.CurseOfSilverCrown.Core.Helpers
{
    public interface ICommand
    {
        int Id { get; }
        int DomainId { get; }
        public int Gold { get; set; }
        public int Warriors { get; set; }
        public int TypeInt { get; set; }
        public int? TargetDomainId { get; }
        public int? Target2DomainId { get; }
        public int InitiatorCharacterId { get; }
        public CommandStatus Status { get; set; }

        public Domain Domain { get; }
        public Domain Target { get; }
        public Domain Target2 { get; }
    }
}
