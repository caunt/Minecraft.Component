using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Minecraft.Component.Converters;

public class ChatColorConverter : JsonConverter<ChatColor>
{
    public override ChatColor Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
            throw new InvalidOperationException();

        var name = reader.GetString();

        return name[0] == '#' ? ChatColor.FromHexadecimal(name) : ChatColor.FromName(name);
    }

    public override void Write(Utf8JsonWriter writer, ChatColor value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}