using System;
using System.Collections.Concurrent;

#pragma warning disable MethodDocumentationHeader
#pragma warning disable ConstructorDocumentationHeader
#pragma warning disable ClassDocumentationHeader
#nullable enable

namespace OxyPlot.Rendering.Utilities
{
    internal sealed class ObjectPool<T> where T : class
    {
        private readonly ConcurrentBag<T> _objects;
        private readonly Func<T> _objectGenerator;

        public ObjectPool(Func<T> objectGenerator)
        {
            this._objects = new ConcurrentBag<T>();
            this._objectGenerator = objectGenerator ?? throw new ArgumentNullException(nameof(objectGenerator));
        }

        public T Get() => this._objects.TryTake(out T? item) ? item : this._objectGenerator();
        public void Return(T item) => this._objects.Add(item);
    }

    internal sealed class StructObjectPool<T> where T : struct
    {
        private readonly ConcurrentBag<T> _objects;
        private readonly Func<T> _objectGenerator;
        private readonly int _initialCapacity;

        public StructObjectPool(Func<T> objectGenerator, int initialCapacity = 0)
        {
            this._objects = new ConcurrentBag<T>();
            this._objectGenerator = objectGenerator;
            this._initialCapacity = initialCapacity;

            for (int i = 0; i < this._initialCapacity; i++)
            {
                this._objects.Add(this._objectGenerator());
            }
        }

        public T Get() => this._objects.TryTake(out T item) ? item : this._objectGenerator();
        public void Return(T item) => this._objects.Add(item);
    }
}
