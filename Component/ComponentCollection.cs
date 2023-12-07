using System.Collections;
using System.Collections.Generic;

namespace Minecraft.Component.Component;

public class ComponentCollection<T> : IEnumerable<T> where T : ChatComponent
{
    private readonly ChatComponent parent;
    private readonly ICollection<T> values = new List<T>();

    public int Count => values.Count;

    public ComponentCollection(ChatComponent parent)
    {
        this.parent = parent;
    }

    public void Add(T item)
    {
        if (item != null)
        {
            item.Parent = parent;
            values.Add(item);
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        return values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}