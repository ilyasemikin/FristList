using System;
using System.Collections.Generic;
using System.Linq;
using FristList.Client.Console.Services.Static;

namespace FristList.Client.Console.Message.Console
{
    public class ColoredConsoleDecoratorMessageNode : MessageNodeBase
    {
        public string ColorName { get; }
        public MessageNodeBase Child { get; }

        public ColoredConsoleDecoratorMessageNode(ConsoleColor color, MessageNodeBase child)
        {
            Child = child;
            if (ConsoleColors.TryGetName(color, out var name))
                ColorName = name;
            else
                throw new InvalidOperationException("Incorrect color");
        }

        public override IEnumerable<string> ConstructLines()
        {
            var lines = Child.ConstructLines()
                .ToArray();

            if (lines.Length == 0)
                yield return string.Empty;
            
            if (lines.Length > 1)
            {
                yield return $"[color:{ColorName}]{lines[0]}";

                for (int i = 1; i < lines.Length - 1; i++)
                    yield return lines[i];

                yield return $"{lines[^1]}[/color]";
            }
            else
                yield return $"[color:{ColorName}]{lines[0]}[/color]";
        }
    }
}