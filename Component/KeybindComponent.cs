using System.Text.Json.Serialization;

namespace Minecraft.Component.Component;

public class KeybindComponent : ChatComponent
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("keybind")]
    public string Keybind { get; }

    public KeybindComponent(string keybind) 
    {
        Keybind = keybind;
    }

    public static KeybindComponent Empty() => new KeybindComponent(string.Empty);
    public static KeybindComponent Create(string keybind) => new KeybindComponent(keybind);
}