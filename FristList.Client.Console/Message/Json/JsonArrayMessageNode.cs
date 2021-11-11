using System.Collections.Generic;
using System.Linq;

namespace FristList.Client.Console.Message.Json
{
    public class JsonArrayMessageNode : MessageNodeBase
    {
        public string Indent { get; }
        public IReadOnlyList<MessageNodeBase> Nodes { get; }

        public JsonArrayMessageNode(IReadOnlyList<MessageNodeBase> nodes)
        {
            Indent = new string(' ', 2);
            Nodes = nodes;
        }
        
        public override IEnumerable<string> ConstructLines()
        {
            if (Nodes.Count == 0)
            {
                yield return "[]";
                yield break;
            }
            
            yield return "[";
            
            for (int i = 0; i < Nodes.Count; i++)
            {
                var lines = Nodes[i].ConstructLines()
                    .ToArray();

                for (int j = 0; j < lines.Length; j++)
                {
                    if (j == lines.Length - 1 && i < Nodes.Count - 1)
                        yield return $"{Indent}{lines[j]},";
                    else
                        yield return $"{Indent}{lines[j]}";
                }
            }

            yield return "]";
        }
    }
}