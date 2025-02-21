using YAGO.World.Host.Database.Units;

namespace YAGO.World.Host.Helpers.Map.Routes
{
    public class RouteFindParameters
    {
        public int UnitId { get; }
        public int UnitDomainId { get; }
        public int FromDomainId { get; }
        public int ToDomainId { get; }
        public enMovementReason MovementReason { get; }
        public int? SupportingDomainId { get; }

        public RouteFindParameters(Unit unit, enMovementReason movementReason, int? toDomainId = null)
        {
            UnitId = unit.Id;
            UnitDomainId = unit.DomainId;
            FromDomainId = unit.PositionDomainId.Value;
            MovementReason = movementReason;
            ToDomainId = toDomainId ?? unit.TargetDomainId.Value;
            SupportingDomainId = MovementReason == enMovementReason.SupportAttack
                ? unit.Target2DomainId ?? unit.TargetDomainId
                : null;
        }

        public bool NeedIntoTarget => MovementReason switch
        {
            enMovementReason.Defense => true,
            enMovementReason.Atack => false,
            enMovementReason.SupportAttack => false,
            enMovementReason.Moving => true,
            enMovementReason.Retreat => true,
            _ => throw new System.NotImplementedException()
        };
    }
}
