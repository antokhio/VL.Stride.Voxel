using Stride.Rendering.Materials;
using VL.Core.Import;

namespace VL.Stride.Voxels
{
    [ProcessNode]
    public abstract class VoxelNodeBase<TInstance>
        where TInstance : new()
    {
        public TInstance Output { get; protected set; } = new TInstance();

        protected class Cachable<T>
        {
            private static bool ReferenceEquals(T a, T b) => object.ReferenceEquals(a, b);

            private readonly TInstance _instance;
            private readonly Func<TInstance, T> _getter;
            private readonly Action<TInstance, T> _setter;
            private readonly Func<T, T, bool> _equals;
            private T _lastValue;

            public Cachable(
                VoxelNodeBase<TInstance> node,
                Func<TInstance, T> getter,
                Action<TInstance, T> setter,
                Func<T, T, bool> equals = default
            )
                : this(node, getter, setter, getter(node.Output), equals) { }

            public Cachable(
                VoxelNodeBase<TInstance> node,
                Func<TInstance, T> getter,
                Action<TInstance, T> setter,
                T initialValue,
                Func<T, T, bool> equals = default
            )
            {
                _instance = node.Output;
                _getter = getter;
                _setter = setter;
                _equals =
                    equals
                    ?? (
                        typeof(T).IsAssignableTo(typeof(IMaterialShaderGenerator))
                            ? ReferenceEquals
                            : EqualityComparer<T>.Default.Equals
                    );
                _lastValue = initialValue;
            }

            public T Value => _getter(_instance);

            public void SetValue(T value)
            {
                if (!_equals(_lastValue, value))
                {
                    _lastValue = value;
                    _setter(_instance, value);
                }
            }
        }

        protected class CachableList<T>
        {
            private readonly TInstance _instance;
            private readonly Func<TInstance, IList<T>> _getter;
            private readonly Action<TInstance, IList<T>> _setter;
            private IReadOnlyList<T> _lastValue;

            public CachableList(VoxelNodeBase<TInstance> node, Func<TInstance, IList<T>> getter)
                : this(node, getter, null) { }

            public CachableList(
                VoxelNodeBase<TInstance> node,
                Func<TInstance, IList<T>> getter,
                Action<TInstance, IList<T>> setter
            )
            {
                _instance = node.Output;
                _getter = getter;
                _setter = setter;
                _lastValue = getter(node.Output)?.ToList() ?? [];
            }

            public IReadOnlyList<T> Value => (IReadOnlyList<T>)_getter(_instance);

            public void SetValue(IReadOnlyList<T> value)
            {
                if (!SequenceEqual(_lastValue, value))
                {
                    _lastValue = value;
                    if (_setter != null)
                    {
                        var list = new List<T>();
                        if (value != null)
                            foreach (var item in value)
                                if (item != null)
                                    list.Add(item);
                        _setter(_instance, list);
                    }
                    else
                    {
                        var current = _getter(_instance);
                        current.Clear();
                        if (value != null)
                            foreach (var item in value)
                                if (item != null)
                                    current.Add(item);
                    }
                }
            }

            public void SetValue(T value)
            {
                IReadOnlyList<T> asList = value is not null ? [value] : [];
                SetValue(asList);
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
    }

    [ProcessNode]
    public abstract class VoxelNodeMutable<TInstance> : VoxelNodeBase<TInstance>, IDisposable
        where TInstance : new()
    {
        public virtual void Dispose()
        {
            if (Output is IDisposable disposable)
                disposable.Dispose();
        }
    }

    [ProcessNode]
    public abstract class VoxelNodeImmutable<TInstance> : VoxelNodeBase<TInstance>
        where TInstance : new()
    {
        private bool _isDirty = true;
        private readonly List<Action<TInstance>> _applicators = new();

        protected bool IsDirty => _isDirty;

        protected void MarkDirty() => _isDirty = true;

        protected new class Cachable<T>
        {
            private static bool ReferenceEquals(T a, T b) => object.ReferenceEquals(a, b);

            private readonly VoxelNodeImmutable<TInstance> _node;
            private readonly Action<TInstance, T> _setter;
            private readonly Func<T, T, bool> _equals;
            private T _lastValue;
            private bool _hasExplicitValue;

            public Cachable(
                VoxelNodeImmutable<TInstance> node,
                Action<TInstance, T> setter,
                T initialValue = default,
                Func<T, T, bool> equals = default
            )
            {
                _node = node;
                _setter = setter;
                _equals =
                    equals
                    ?? (
                        typeof(T).IsAssignableTo(typeof(IMaterialShaderGenerator))
                            ? ReferenceEquals
                            : EqualityComparer<T>.Default.Equals
                    );

                _lastValue = initialValue;

                // Prevent stomping constructor defaults with implicit null on ref types.
                _hasExplicitValue = typeof(T).IsValueType || initialValue is not null;

                if (_hasExplicitValue)
                    _setter(_node.Output, _lastValue);

                _node._applicators.Add(instance =>
                {
                    if (_hasExplicitValue)
                        _setter(instance, _lastValue);
                });
            }

            public T LastValue => _lastValue;

            public void SetValue(T value)
            {
                if (!_equals(_lastValue, value))
                {
                    _lastValue = value;
                    _hasExplicitValue = true; // explicit assignment (including null)
                    _node.MarkDirty();
                }
            }
        }

        public void Update()
        {
            if (!_isDirty)
                return;

            var instance = new TInstance();

            foreach (var apply in _applicators)
                apply(instance);

            OnBuildInstance(instance);

            Output = instance;
            _isDirty = false;
        }

        protected virtual void OnBuildInstance(TInstance instance) { }
    }
}
