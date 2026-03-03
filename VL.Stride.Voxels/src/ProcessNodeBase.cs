using Stride.Rendering.Materials;
using VL.Core.Import;

namespace VL.Stride.Rendering.Voxels
{
    [ProcessNode]
    public abstract class ProcessNodeBase<TInstance> : IDisposable
        where TInstance : new()
    {
        protected TInstance Instance { get; set; }

        protected virtual Func<TInstance> Initialize { get; } = () => new();
        protected List<Action<TInstance>> Setters { get; set; } = new();
        protected bool IsDirty { get; set; }
        protected virtual bool IsImmutable { get; } = true;

        public ProcessNodeBase()
        {
            Instance = Initialize();
        }

        protected abstract class CachableBase
        {
            private readonly WeakReference<ProcessNodeBase<TInstance>> _nodeRef;

            protected CachableBase(ProcessNodeBase<TInstance> node)
            {
                _nodeRef = new(node);
            }

            protected ProcessNodeBase<TInstance> Node
            {
                get
                {
                    if (_nodeRef.TryGetTarget(out var node))
                        return node;
                    throw new InvalidOperationException(
                        "The associated node has been garbage collected."
                    );
                }
            }
        }

        protected class Cachable<T> : CachableBase
        {
            private static bool ReferenceEquals(T a, T b) => object.ReferenceEquals(a, b);

            private readonly Func<TInstance, T> _getter;
            private readonly Action<TInstance, T> _setter;
            private readonly Func<T, T, bool> _equals;
            private T _lastValue;

            public Cachable(
                ProcessNodeBase<TInstance> node,
                Action<TInstance, T> setter,
                T initialValue = default,
                Func<T, T, bool> equals = default
            )
                : this(node, null, setter, initialValue, equals) { }

            public Cachable(
                ProcessNodeBase<TInstance> node,
                Func<TInstance, T> getter,
                Action<TInstance, T> setter,
                T initialValue = default,
                Func<T, T, bool> equals = default
            )
                : base(node)
            {
                _getter = getter;
                _setter = setter;
                _lastValue = initialValue;
                _equals =
                    equals
                    ?? (
                        typeof(T).IsAssignableTo(typeof(IMaterialShaderGenerator))
                            ? ReferenceEquals
                            : EqualityComparer<T>.Default.Equals
                    );

                // Set's initial value to the instance
                _setter(node.Instance, _lastValue);

                // Registers setter to be called on node update
                node.Setters.Add(instance =>
                {
                    _setter(instance, _lastValue);
                });
            }

            public T Value => _getter != null ? _getter(Node.Instance) : _lastValue;

            public void SetValue(T value)
            {
                if (!_equals(_lastValue, value))
                {
                    // Sets value that is setted by setter
                    _lastValue = value;

                    // Mark node as dirty to trigger update
                    Node.IsDirty = true;
                }
            }
        }

        protected class CachableList<T> : CachableBase
        {
            private readonly Func<TInstance, IList<T>> _getter;
            private readonly Action<TInstance, IList<T>> _setter;
            private IReadOnlyList<T> _lastValue;

            public CachableList(
                ProcessNodeBase<TInstance> node,
                Action<TInstance, IList<T>> setter,
                IReadOnlyList<T> initialValue = null
            )
                : this(node, null, setter, initialValue) { }

            public CachableList(
                ProcessNodeBase<TInstance> node,
                Func<TInstance, IList<T>> getter,
                Action<TInstance, IList<T>> setter = null,
                IReadOnlyList<T> initialValue = null
            )
                : base(node)
            {
                _getter = getter;
                _setter = setter;

                // Use the provided initial value, or fallback to the instance's default list
                _lastValue =
                    initialValue ?? getter?.Invoke(node.Instance)?.ToList() ?? new List<T>();

                // Defines how to apply the list
                Action<TInstance> applyList = instance =>
                {
                    if (_setter != null)
                    {
                        var list = new List<T>();
                        if (_lastValue != null)
                            foreach (var item in _lastValue)
                                if (item != null)
                                    list.Add(item);
                        _setter(instance, list);
                    }
                    else if (_getter != null)
                    {
                        var current = _getter(instance);
                        current.Clear();
                        if (_lastValue != null)
                            foreach (var item in _lastValue)
                                if (item != null)
                                    current.Add(item);
                    }
                };

                // 2. Apply immediately
                applyList(node.Instance);

                // 3. Register for updates
                node.Setters.Add(applyList);
            }

            public void SetValue(IReadOnlyList<T> value)
            {
                if (!SequenceEqual(_lastValue, value))
                {
                    _lastValue = value;
                    Node.IsDirty = true;
                }
            }

            private static bool SequenceEqual(IReadOnlyList<T> a, IReadOnlyList<T> b)
            {
                if (object.ReferenceEquals(a, b))
                    return true;
                if (a is null || b is null || a.Count != b.Count)
                    return false;
                return a.SequenceEqual(b);
            }
        }

        public virtual void Update(out TInstance output)
        {
            if (IsDirty)
            {
                if (IsImmutable)
                    Instance = Initialize();

                foreach (var setter in Setters)
                    setter(Instance);

                IsDirty = false;
            }

            output = Instance;
        }

        public virtual void Dispose()
        {
            if (Instance is IDisposable disposable)
            {
                // TODO:
                // disposable.Dispose();
            }
        }
    }
}
