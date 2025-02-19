using YAGO.World.Host.Database.Commands;
using YAGO.World.Host.Database.Domains;

namespace YAGO.World.Host.Helpers
{
    public interface ICommand
    {
        int Id { get; }
        ExecutorType ExecutorType { get; }
        int ExecutorId { get; }
        int DomainId { get; }
        public int Gold { get; set; }
        public int Warriors { get; set; }
        public int TypeInt { get; set; }
        public int? TargetDomainId { get; }
        public int? Target2DomainId { get; }
        public CommandStatus Status { get; set; }

        public Organization Domain { get; }
        public Organization Target { get; }
        public Organization Target2 { get; }
    }
}
