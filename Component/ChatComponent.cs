using Minecraft.Component.Converters;
using Minecraft.Component.Event;
using Minecraft.Component.Serializer;
using MongoDB.Bson.Serialization.Attributes;
using SharpNBT;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Xml.Linq;

namespace Minecraft.Component.Component;

[JsonDerivedType(typeof(StringComponent))]
[JsonDerivedType(typeof(TranslationComponent))]
public class ChatComponent
{
    private bool? bold;
    private bool? italic;
    private bool? underlined;
    private bool? strikethrough;
    private bool? obfuscated;
    private ChatColor color;
    private string font;
    private string insertion;
    private ClickEvent clickEvent;
    private HoverEvent hoverEvent;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("bold")]
    public bool? IsBold
    {
        get => bold ?? Parent?.bold;
        set => bold = value;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("italic")]
    public bool? IsItalic
    {
        get => italic ?? Parent?.italic;
        set => italic = value;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("underlined")]
    public bool? IsUnderlined
    {
        get => underlined ?? Parent?.underlined;
        set => underlined = value;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("strikethrough")]
    public bool? IsStrikeThrough
    {
        get => strikethrough ?? Parent?.strikethrough;
        set => strikethrough = value;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("obfuscated")]
    public bool? IsObfuscated
    {
        get => obfuscated ?? Parent?.obfuscated;
        set => obfuscated = value;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("color")]
    public ChatColor Color
    {
        get => color ?? Parent?.color;
        set => color = value;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("font")]
    public string Font
    {
        get => font ?? Parent?.font;
        set => font = value;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("insertion")]
    public string Insertion
    {
        get => insertion ?? Parent?.insertion;
        set => insertion = value;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("clickEvent")]
    public ClickEvent ClickEvent
    {
        get => clickEvent ?? Parent?.clickEvent;
        set => clickEvent = value;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("hoverEvent")]
    public HoverEvent HoverEvent
    {
        get => hoverEvent ?? Parent?.hoverEvent;
        set => hoverEvent = value;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("extra")]
    public ComponentCollection<ChatComponent> Extra { get; internal set; }

    [BsonIgnore]
    [JsonIgnore]
    public ChatComponent Parent { get; internal set; }

    public static JsonSerializerOptions JsonSerializerOptions { get; protected set; } = new JsonSerializerOptions();

    static ChatComponent()
    {
        JsonSerializerOptions.Converters.Add(new ChatColorConverter());
        JsonSerializerOptions.Converters.Add(new HoverEventActionConverter());
        JsonSerializerOptions.Converters.Add(new ClickEventActionConverter());
        JsonSerializerOptions.TypeInfoResolver = new DefaultJsonTypeInfoResolver { Modifiers = { Resolver } };
    }

    public ChatComponent()
    {
        Extra = new ComponentCollection<ChatComponent>(this);
    }

    public ChatComponent AppendText(string text)
    {
        Extra.Add(StringComponent.Create(text));
        return this;
    }

    public ChatComponent Append(ChatComponent component)
    {
        if (component != null)
            Extra.Add(component);

        return this;
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, JsonSerializerOptions);
    }

    public JsonObject ToJson()
    {
        return JsonNode.Parse(ToString()).AsObject();
    }

    public Tag ToNbt(string name = null)
    {
        return NbtComponentSerializer.Serialize(this, name);
    }

    public static ChatComponent FromJson(string json)
    {
        return JsonSerializer.Deserialize<ChatComponent>(json, JsonSerializerOptions);
    }

    public static ChatComponent FromLegacy(string legacy)
    {
        return LegacyComponentSerializer.Deserialize(legacy);
    }

    public static ChatComponent FromNbt(Tag tag)
    {
        return NbtComponentSerializer.Deserialize(tag);
    }

    private static void Resolver(JsonTypeInfo info)
    {
        if (!typeof(ChatComponent).IsAssignableFrom(info.Type))
            return;

        foreach (var property in info.Properties.Where(property => property.Name == "extra" || property.Name == "with"))
        {
            property.ShouldSerialize = (instance, value) =>
            {
                var component = (ChatComponent)instance;
                var count = (int)value.GetType().GetProperty(nameof(ComponentCollection.Count)).GetValue(value);

                return count > 0;
            };
        }
    }
}