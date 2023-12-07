using System.Text.Json.Serialization;

namespace Minecraft.Component.Component;

public class ScoreComponent : ChatComponent
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("score")]
    public ScoreData Score { get; set; }

    public ScoreComponent(ScoreData score)
    {
        Score = score;
    }

    public static ScoreComponent Create(ScoreData data) => new ScoreComponent(data);
}

public class ScoreData
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("objective")]
    public string Objective { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("value")]
    public string Value { get; set; }
}