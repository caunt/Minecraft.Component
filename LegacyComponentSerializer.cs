using Minecraft.Component.Component;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace Minecraft.Component.Chat
{
    public enum LegacyTextFormat
    {
        BLACK = '0',
        DARK_BLUE = '1',
        DARK_GREEN = '2',
        DARK_AQUA = '3',
        DARK_RED = '4',
        DARK_PURPLE = '5',
        GOLD = '6',
        GRAY = '7',
        DARK_GRAY = '8',
        BLUE = '9',
        GREEN = 'a',
        AQUA = 'b',
        RED = 'c',
        LIGHT_PURPLE = 'd',
        YELLOW = 'e',
        WHITE = 'f',

        OBFUSCATED = 'k',
        BOLD = 'l',
        STRIKETHROUGH = 'm',
        UNDERLINED = 'n',
        ITALIC = 'o',

        RESET = 'r'
    }

    public static partial class LegacyComponentSerializer
    {
        private static Regex DEFAULT_URL_PATTERN = new Regex("(?:(https?)://)?([-\\w_.]+\\.\\w{2,})(/\\S*)?");
        private static Regex URL_SCHEME_PATTERN = new Regex("^[a-z][a-z0-9+\\-.]*:");

        public static ChatComponent Deserialize(string input)
        {
            var extra = new List<ChatComponent>();

            var reset = false;
            var lastComponent = (StringComponent)null;

            var idx = input.Length - 1;
            var prevIdx = input.Length;

            while (idx > 0 && (idx = input.LastIndexOf('&', idx - 1)) != -1)
            {
                var code = input.ElementAt(idx + 1);

                if (!Enum.IsDefined(typeof(LegacyTextFormat), (int)code))
                    continue;

                int from = idx + 2;
                var nextComponent = StringComponent.Create(input.Substring(from, prevIdx - from));

                if (lastComponent != null)
                {
                    if (reset)
                    {
                        extra.Add(lastComponent);
                        reset = false;
                    }
                    else
                    {
                        nextComponent.Extra.Add(lastComponent);
                    }
                }

                lastComponent = nextComponent;

                if (!reset)
                    reset = ApplyFormat(lastComponent, (LegacyTextFormat)code);

                prevIdx = idx;
            }

            if (lastComponent != null)
                extra.Add(lastComponent);

            if (prevIdx == 0 && extra.Count == 1)
                return extra[0];

            extra.Reverse();

            var component = StringComponent.Create(input.Substring(0, prevIdx));
            extra.ForEach(component.Extra.Add);

            return component;
        }

        private static bool ApplyFormat(StringComponent component, LegacyTextFormat format)
        {
            switch (format)
            {
                case LegacyTextFormat.BLACK:
                case LegacyTextFormat.DARK_BLUE:
                case LegacyTextFormat.DARK_GREEN:
                case LegacyTextFormat.DARK_AQUA:
                case LegacyTextFormat.DARK_RED:
                case LegacyTextFormat.DARK_PURPLE:
                case LegacyTextFormat.GOLD:
                case LegacyTextFormat.GRAY:
                case LegacyTextFormat.DARK_GRAY:
                case LegacyTextFormat.BLUE:
                case LegacyTextFormat.GREEN:
                case LegacyTextFormat.AQUA:
                case LegacyTextFormat.RED:
                case LegacyTextFormat.LIGHT_PURPLE:
                case LegacyTextFormat.YELLOW:
                case LegacyTextFormat.WHITE:
                    var colorName = Enum.GetName(typeof(LegacyTextFormat), format) ?? throw new InvalidEnumArgumentException(format.ToString());
                    component.Color = ChatColor.FromName(colorName.ToLower());
                    return true;

                case LegacyTextFormat.OBFUSCATED:
                    component.IsObfuscated = true;
                    return false;
                case LegacyTextFormat.BOLD:
                    component.IsBold = true;
                    return false;
                case LegacyTextFormat.STRIKETHROUGH:
                    component.IsStrikeThrough = true;
                    return false;
                case LegacyTextFormat.UNDERLINED:
                    component.IsUnderlined = true;
                    return false;
                case LegacyTextFormat.ITALIC:
                    component.IsItalic = true;
                    return false;

                case LegacyTextFormat.RESET:
                    return true;

                default:
                    throw new ArgumentException($"Unknown format '{format}'");
            }
        }
    }
}