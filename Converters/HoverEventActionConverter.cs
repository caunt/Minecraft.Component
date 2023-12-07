using Minecraft.Component.Event;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Minecraft.Component.Converters;

public class HoverEventActionConverter : JsonConverter<HoverEventAction>
{
    public override HoverEventAction Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
            throw new InvalidOperationException();

        return HoverEventAction.FromName(reader.GetString());
    }

    public override void Write(Utf8JsonWriter writer, HoverEventAction value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}