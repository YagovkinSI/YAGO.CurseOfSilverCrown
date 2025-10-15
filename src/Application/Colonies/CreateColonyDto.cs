using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Application.Colonies
{
    public record CreateColonyDto
    {
        public long UserId { get; }
        public string Name { get; }
        public decimal SolarsIncome { get; }
        public decimal Reputation { get; }
        public int Population { get; }

        public CreateColonyDto(
            long userId,
            string name,
            ColonyPresetType presetType)
        {
            UserId = userId;
            Name = name;
            SolarsIncome = GetParameter(presetType, CreateColonyParameterType.SolarsIncome);
            Reputation = GetParameter(presetType, CreateColonyParameterType.Reputation);
            Population = (int)GetParameter(presetType, CreateColonyParameterType.Population);
        }

        private decimal GetParameter(ColonyPresetType colonyPresetType, CreateColonyParameterType parameterType)
        {
            return colonyPresetType switch
            {
                ColonyPresetType.Unknown => throw new YagoUnknownTypeException(nameof(ColonyPresetType)),
                ColonyPresetType.Humanist => GetHumanistParameter(parameterType),
                ColonyPresetType.Pragmatist => GetPragmatistParameter(parameterType),
                ColonyPresetType.Dictator => GetDictatorParameter(parameterType),
                _ => throw new System.NotImplementedException(),
            };
        }

        private decimal GetHumanistParameter(CreateColonyParameterType parameterType)
        {
            return parameterType switch
            {
                CreateColonyParameterType.Unknown => throw new YagoUnknownTypeException(nameof(CreateColonyParameterType)),
                CreateColonyParameterType.SolarsIncome => 50,
                CreateColonyParameterType.Reputation => 400,
                CreateColonyParameterType.Population => 160,
                _ => throw new System.NotImplementedException(),
            };
        }

        private decimal GetPragmatistParameter(CreateColonyParameterType parameterType)
        {
            return parameterType switch
            {
                CreateColonyParameterType.Unknown => throw new YagoUnknownTypeException(nameof(CreateColonyParameterType)),
                CreateColonyParameterType.SolarsIncome => 60,
                CreateColonyParameterType.Reputation => 0,
                CreateColonyParameterType.Population => 200,
                _ => throw new System.NotImplementedException(),
            };
        }

        private decimal GetDictatorParameter(CreateColonyParameterType parameterType)
        {
            return parameterType switch
            {
                CreateColonyParameterType.Unknown => throw new YagoUnknownTypeException(nameof(CreateColonyParameterType)),
                CreateColonyParameterType.SolarsIncome => 70,
                CreateColonyParameterType.Reputation => -400,
                CreateColonyParameterType.Population => 240,
                _ => throw new System.NotImplementedException(),
            };
        }
    }
}
