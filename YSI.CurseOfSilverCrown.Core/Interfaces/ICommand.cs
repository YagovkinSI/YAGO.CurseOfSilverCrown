using YSI.CurseOfSilverCrown.Core.MainModels.Commands;
using YSI.CurseOfSilverCrown.Core.MainModels.Domains;

namespace YSI.CurseOfSilverCrown.Core.Interfaces
{
    public interface ICommand
    {
        int Id { get; }
        int DomainId { get; }
        public int Coffers { get; set; }
        public int Warriors { get; set; }
        public int TypeInt { get; set; }
        public int? TargetDomainId { get; }
        public int? Target2DomainId { get; }
        public int InitiatorPersonId { get; }
        public enCommandStatus Status { get; set; }

        public Domain Domain { get; }
        public Domain Target { get; }
        public Domain Target2 { get; }
    }
}
