using System;
using System.Buffers;

namespace Ember.Utils;

public class CircularArray<T>(int capacity) : IDisposable
{
    //FIELD only available in .net 10
    public int Index
    {
        get;
        set => field = value % Capacity;
    }
    public T[] Data = ArrayPool<T>.Shared.Rent(capacity);
    public int Capacity => Data.Length;

    public void Dispose()
    {
        ArrayPool<T>.Shared.Return(Data);
    }

    public void Add(T item)
    {
        Data[Index] = item;
        Index++;
    }

    public void Resize(int newCapacity)
    {
        ArrayPool<T>.Shared.Return(Data);
        Data = ArrayPool<T>.Shared.Rent(newCapacity);
        Index = 0;
    }

    public T GetCurrent()
    {
        return Data[Index];
    }

    public T GetNext()
    {
        Index++;
        return Data[Index];
    }

    public T this[int i] => Data[i];
}