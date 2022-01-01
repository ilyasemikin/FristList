using System;
using System.Collections.Generic;

namespace FristList.Client.Console;

public record CommandRequest(string Name, IReadOnlyList<string> Parameters)
{
    public static CommandRequest Empty { get; }

    static CommandRequest()
    {
        Empty = new CommandRequest(string.Empty, ArraySegment<string>.Empty);
    }
}