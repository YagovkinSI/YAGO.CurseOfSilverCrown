using System.Text.Json.Serialization;

namespace YAGO.World.Host.Controllers.Episodes
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
    [JsonDerivedType(typeof(DilemmaSelectResponse))]
    [JsonDerivedType(typeof(DilemmaTextInputResponse))]
    public record DilemmaResponse(
        string DilemmaType);
}
