using System.Collections.Generic;

namespace FristList.Client.Console.Message.PlainText
{
    public class PlainMessageNode : MessageNodeBase
    {
        public string Text { get; }

        public PlainMessageNode(string text)
        {
            Text = text;
        }
        
        public override IEnumerable<string> ConstructLines()
        {
            yield return Text;
        }
    }
}