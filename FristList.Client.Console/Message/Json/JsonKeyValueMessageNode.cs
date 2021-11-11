using System.Collections.Generic;
using System.Linq;

namespace FristList.Client.Console.Message.Json
{
    public class JsonKeyValueMessageNode : MessageNodeBase
    {
        public string Key { get; }
        public MessageNodeBase Value { get; }

        public JsonKeyValueMessageNode(string key, MessageNodeBase value)
        {
            Key = key;
            Value = value;
        }
        
        public override IEnumerable<string> ConstructLines()
        {
            var valueLines = Value.ConstructLines()
                .ToArray();

            if (valueLines.Length > 1)
            {
                yield return $"\"{Key}\":";
                foreach (var line in valueLines)
                    yield return $" {line}";
            }
            else
                yield return $"\"{Key}\": {valueLines[0]}";
        }
    }
}