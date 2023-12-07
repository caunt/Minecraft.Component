using System;
using System.Collections.Generic;
using System.Drawing;

namespace Minecraft.Component
{
    public sealed class ChatColor
    {
        private static readonly Dictionary<string, ChatColor> Colors = new Dictionary<string, ChatColor>();

        public static readonly ChatColor Black = CreateWithName("black");
        public static readonly ChatColor DarkBlue = CreateWithName("dark_blue");
        public static readonly ChatColor DarkGreen = CreateWithName("dark_green");
        public static readonly ChatColor DarkAqua = CreateWithName("dark_aqua");
        public static readonly ChatColor DarkRed = CreateWithName("dark_red");
        public static readonly ChatColor DarkPurple = CreateWithName("dark_purple");
        public static readonly ChatColor DarkGray = CreateWithName("dark_gray");
        public static readonly ChatColor Gold = CreateWithName("gold");
        public static readonly ChatColor Gray = CreateWithName("gray");
        public static readonly ChatColor Blue = CreateWithName("blue");
        public static readonly ChatColor Green = CreateWithName("green");
        public static readonly ChatColor Aqua = CreateWithName("aqua");
        public static readonly ChatColor Red = CreateWithName("red");
        public static readonly ChatColor LightPurple = CreateWithName("light_purple");
        public static readonly ChatColor Yellow = CreateWithName("yellow");
        public static readonly ChatColor White = CreateWithName("white");
        public static readonly ChatColor Reset = CreateWithName("reset");

        private readonly string name;

        private ChatColor(string name)
        {
            this.name = name;
        }

        private static ChatColor CreateWithName(string colorName)
        {
            var output = new ChatColor(colorName);
            Colors[colorName] = output;
            return output;
        }

        public override string ToString()
        {
            return name;
        }

        public static ChatColor FromColor(Color color)
        {
            return new ChatColor($"{color.R:X2}{color.G:X2}{color.B:X2}");
        }

        public static ChatColor FromRgb(byte r, byte g, byte b)
        {
            return FromColor(Color.FromArgb(r, g, b));
        }

        public static ChatColor FromHexadecimal(string name)
        {
            var length = name.Length;
            var correctLength = name[0] == '#' ? 7 : 6;

            if (length != correctLength)
            {
                throw new InvalidOperationException("Invalid hexadecimal code");
            }

            return new ChatColor(name);
        }

        public static ChatColor FromName(string name)
        {
            if (Colors.TryGetValue(name, out var color))
                return color;

            throw new KeyNotFoundException();
        }
    }
}