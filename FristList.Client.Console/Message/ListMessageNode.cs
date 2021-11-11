using System.Collections.Generic;
using System.Linq;

namespace FristList.Client.Console.Message
{
    public class ListMessageNode : MessageNodeBase
    {
        public IReadOnlyList<MessageNodeBase> Nodes { get; }

        public ListMessageNode(IReadOnlyList<MessageNodeBase> nodes)
        {
            Nodes = nodes;
        }

        public override IEnumerable<string> ConstructLines()
            => Nodes.Select(n => n.Construct());
    }
}