using System.Text.Json.Serialization;

namespace Minecraft.Component.Component;

public class TranslationComponent : ChatComponent
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("translate")]
    public string Translate { get; set; }

    [JsonPropertyName("with")]
    public ComponentCollection<StringComponent> With { get; set; }

    public TranslationComponent(string translate) : base()
    {
        Translate = translate;
    }

    public TranslationComponent()
    {
        With = new ComponentCollection<StringComponent>(this);
    }
}