using System.Linq;
using YAGO.World.Application.Events;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.GameEvents.Episodes;
using YAGO.World.Host.Controllers.Colonies;
using YAGO.World.Host.Controllers.Common.GameRequirements;
using YAGO.World.Host.Controllers.Common.GameVisibleEffects;

namespace YAGO.World.Host.Controllers.Episodes
{
    public static class EpisodeResponseMapping
    {
        public static EpisodeResponse ToEpisodeResponse(this ColonyEventPrivateDto source)
        {
            var eventCode = source.GameEvent.Code;
            return new EpisodeResponse(
                [.. source.GameEvent.Slides.Select(x => x.ToResponse(source.ColonyState, isChange: true, eventCode))]);
        }

        public static SlideResponse ToResponse(this Slide source, ColonyState colonyStats, bool isChange, string eventCode)
        {
            var requirements = source.Buttons.SelectMany(x => x.Requirements).ToList();
            var requirementsResponse = requirements.Select(x => x.ToResponse(colonyStats)).ToList();
            var visibleEffects = source.ParameterChanges.ToVisibleEffectsResponse();

            return new SlideResponse(
                source.Id,
                source.Title,
                source.ImageName,
                source.Text,
                visibleEffects,
                requirementsResponse,
                [.. source.Buttons.Select(x => x.ToResponse(colonyStats, eventCode))],
                source.TextInput?.ToResponse());
        }

        private static TextInputResponse ToResponse(this SlideTextInput source)
        {
            return new TextInputResponse();
        }
    }
}
