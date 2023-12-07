using Minecraft.Component.Component;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Minecraft.Component.Event
{
    public sealed class HoverEvent
    {
        [JsonPropertyName("action")]
        public HoverEventAction ActionType { get; }

        [JsonPropertyName("value")]
        public StringComponent Value { get; }

        public HoverEvent(HoverEventAction actionType, StringComponent value)
        {
            ActionType = actionType;
            Value = value;
        }
    }

    public sealed class HoverEventAction
    {
        private static readonly Dictionary<string, HoverEventAction> Values = new Dictionary<string, HoverEventAction>();

        public static readonly HoverEventAction ShowText = new HoverEventAction("show_text");
        public static readonly HoverEventAction ShowItem = new HoverEventAction("show_item");
        public static readonly HoverEventAction ShowEntity = new HoverEventAction("show_entity");

        private readonly string name;

        private HoverEventAction(string name)
        {
            this.name = name;
            Values[name] = this;
        }

        public static HoverEventAction FromName(string name)
        {
            if (Values.TryGetValue(name, out var action))
                return action;

            throw new KeyNotFoundException();
        }

        public override string ToString()
        {
            return name;
        }
    }
}