using System;
using System.Collections.Generic;
using System.Linq;

namespace FristList.Client.Console.Services.Static
{
    public static class ConsoleColors
    {
        public static readonly IReadOnlyDictionary<ConsoleColor, string> Names;
        public static readonly IReadOnlyDictionary<string, ConsoleColor> Colors;

        static ConsoleColors()
        {
            var names = new Dictionary<ConsoleColor, string>();
            
            foreach (var value in Enum.GetValues<ConsoleColor>())
                names.Add(value, Enum.GetName(value));

            Names = names;
            Colors = Names.ToDictionary(kv => kv.Value, kv => kv.Key);
        }

        public static bool TryGetColor(string name, out ConsoleColor color)
            => Colors.TryGetValue(name, out color);

        public static bool TryGetName(ConsoleColor color, out string name)
            => Names.TryGetValue(color, out name);
    }
}