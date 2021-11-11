using System.Collections.Generic;

namespace FristList.Client.Console.Message.PlainText
{
    public class UnorderedListMessageNode : MessageNodeBase
    {
        public IReadOnlyList<MessageNodeBase> Nodes { get; }
        public string Mark { get; }
        public string EmptyMark { get; }

        public UnorderedListMessageNode(IReadOnlyList<MessageNodeBase> nodes, string mark)
        {
            Nodes = nodes;
            Mark = mark;
            EmptyMark = new string(' ', Mark.Length);
        }

        public override IEnumerable<string> ConstructLines()
        {
            foreach (var node in Nodes)
            {
                var mark = Mark;
                foreach (var line in node.ConstructLines())
                {
                    yield return $"{mark} {line}";

                    mark = EmptyMark;
                }
            }
        }
    }
}