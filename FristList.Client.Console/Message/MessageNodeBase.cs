using System.Collections.Generic;

namespace FristList.Client.Console.Message
{
    public abstract class MessageNodeBase
    {
        public string Construct(string symbol = "\n")
            => string.Join(symbol, ConstructLines());

        public abstract IEnumerable<string> ConstructLines();
    }
}