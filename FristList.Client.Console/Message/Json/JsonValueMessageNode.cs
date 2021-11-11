using System;
using System.Collections.Generic;

namespace FristList.Client.Console.Message.Json
{
    public class JsonValueMessageNode : MessageNodeBase
    {
        public string Value { get; }
        
        public JsonValueMessageNode(string value)
        {
            Value = value;
        }

        public override IEnumerable<string> ConstructLines()
        {
            yield return Value;
        }

        public static JsonValueMessageNode CreateNumber(int value)
            => new JsonValueMessageNode($"{value}");

        public static JsonValueMessageNode CreateDateTime(DateTime value)
            => new JsonValueMessageNode(value.ToString("O"));

        public static JsonValueMessageNode CreateString(string value)
            => new JsonValueMessageNode($"\"{value}\"");

        public static JsonValueMessageNode CreateBoolean(bool value)
            => new JsonValueMessageNode(value ? "true" : "false");
        
        public static JsonValueMessageNode CreateNull()
            => new JsonValueMessageNode("null");
    }
}