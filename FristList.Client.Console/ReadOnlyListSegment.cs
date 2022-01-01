using System;
using System.Collections;
using System.Collections.Generic;

namespace FristList.Client.Console;

public class ReadOnlyListSegment<T> : IReadOnlyList<T>
{
    public IReadOnlyList<T> Data { get; }
    
    public int Offset { get; }

    public int Count { get; }

    public ReadOnlyListSegment(IReadOnlyList<T> data, int offset, int count)
    {
        if (offset + count > data.Count)
            throw new ArgumentException("Incorrect count");
        
        Data = data;
        Offset = offset;
        Count = count;
    }

    public ReadOnlyListSegment(IReadOnlyList<T> data, int offset = 0)
        : this(data, offset, data.Count - offset)
    {
        
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (var i = 0; i < Count; i++)
            yield return Data[i];
    }

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    public T this[int index] => Data[Offset + index];
}