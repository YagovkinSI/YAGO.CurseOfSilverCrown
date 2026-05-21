using System;
using YAGO.World.Host.Controllers.Episodes;

namespace YAGO.World.Host.Controllers.Colonies.MyQuests
{
    public record MyQuest(
        Guid Id,
        string Title,
        string Progress,
        bool Completed,
        QuestTypeResponse Type,
        PrologueSlideResponse PrologueSlide);
}
