using System.Text.Json;
using System.Text.Json.Serialization;

namespace Minecraft.Component.Component;

public class StringComponent : ChatComponent
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;

    public StringComponent(string text)
    {
        Text = text;
    }

    public static StringComponent Empty() => new StringComponent(string.Empty);
    public static StringComponent Create(string text) => new StringComponent(text);

    public static new StringComponent FromJson(string json)
    {
        return JsonSerializer.Deserialize<StringComponent>(json, JsonSerializerOptions);
    }
}