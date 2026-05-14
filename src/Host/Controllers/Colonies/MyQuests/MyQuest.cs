using System;

namespace YAGO.World.Host.Controllers.Colonies.MyQuests
{
    public record MyQuest(
        Guid Id,
        string Name,
        string Progress,
        QuestTypeResponse Type);
}
