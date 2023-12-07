using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Minecraft.Component.Event
{
    public sealed class ClickEvent
    {
        [JsonPropertyName("action")]
        public ClickEventAction ActionType { get; }

        [JsonPropertyName("value")]
        public string Value { get; }

        public ClickEvent(ClickEventAction actionType, string value)
        {
            ActionType = actionType;
            Value = value;
        }
    }

    public sealed class ClickEventAction
    {
        private static readonly Dictionary<string, ClickEventAction> Values = new Dictionary<string, ClickEventAction>();

        public static readonly ClickEventAction OpenUrl = new ClickEventAction("open_url");
        public static readonly ClickEventAction OpenFile = new ClickEventAction("open_file");
        public static readonly ClickEventAction RunCommand = new ClickEventAction("run_command");
        public static readonly ClickEventAction SuggestCommand = new ClickEventAction("suggest_command");
        public static readonly ClickEventAction ChangePage = new ClickEventAction("change_page");
        public static readonly ClickEventAction CopyToClipboard = new ClickEventAction("copy_to_clipboard");

        private readonly string name;

        private ClickEventAction(string name)
        {
            this.name = name;
            Values[name] = this;
        }

        public static ClickEventAction FromName(string name)
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