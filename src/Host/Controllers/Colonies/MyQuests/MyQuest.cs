using YAGO.World.Host.Controllers.Episodes;

namespace YAGO.World.Host.Controllers.Colonies.MyQuests
{
    public record MyQuest(
        string Id,
        string Name,
        string Progress,
        bool Completed,
        QuestTypeResponse Type,
        SlideResponse PrologueSlide);
}
