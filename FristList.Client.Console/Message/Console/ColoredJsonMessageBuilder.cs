using System;
using System.Collections;
using FristList.Client.Console.Message.Json;

namespace FristList.Client.Console.Message.Console
{
    public class ColoredJsonMessageBuilder : JsonMessageBuilder
    {
        protected override MessageNodeBase BuildNumber(int value)
            => new ColoredConsoleDecoratorMessageNode(ConsoleColor.Green, base.BuildNumber(value));

        protected override MessageNodeBase BuildString(string value)
            => new ColoredConsoleDecoratorMessageNode(ConsoleColor.Green, base.BuildString(value));

        protected override MessageNodeBase BuildBoolean(bool value)
            => new ColoredConsoleDecoratorMessageNode(ConsoleColor.Green, base.BuildBoolean(value));

        protected override MessageNodeBase BuildDateTime(DateTime value)
            => new ColoredConsoleDecoratorMessageNode(ConsoleColor.Green, base.BuildDateTime(value));

        protected override MessageNodeBase BuildNull()
            => new ColoredConsoleDecoratorMessageNode(ConsoleColor.Red, base.BuildNull());

        protected override MessageNodeBase BuildKeyValue(string name, MessageNodeBase node)
            => new ColoredConsoleDecoratorMessageNode(ConsoleColor.Yellow, base.BuildKeyValue(name, node));

        protected override MessageNodeBase BuildArray(IEnumerable values)
            => new ColoredConsoleDecoratorMessageNode(ConsoleColor.Cyan, base.BuildArray(values));

        protected override MessageNodeBase BuildObject(object obj)
            => new ColoredConsoleDecoratorMessageNode(ConsoleColor.White, base.BuildObject(obj));
    }
}