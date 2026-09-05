using System.Text.Json.Serialization;

namespace YAGO.World.Host.Controllers.GameActions
{
    [JsonConverter(typeof(JsonStringEnumConverter<GameActionType>))]
    public enum GameActionType
    {
        [JsonStringEnumMemberName("event")]
        Event,

        [JsonStringEnumMemberName("reform")]
        Reform,

        [JsonStringEnumMemberName("hireAdvisor")]
        HireAdvisor,

        [JsonStringEnumMemberName("endTurn")]
        EndTurn
    }
}