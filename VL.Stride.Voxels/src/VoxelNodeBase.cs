using Stride.Rendering.Materials;
using VL.Core.Import;

namespace VL.Stride.Voxels
{
    /// <summary>
    /// Base class for vvvv nodes wrapping an external Stride datatype.
    /// Subclass <see cref="VoxelNodeMutable{TInstance}"/> for in-place mutation
    /// or <see cref="VoxelNodeImmutable{TInstance}"/> for copy-on-write semantics.
    /// </summary>
    [ProcessNode]
    public abstract class VoxelNodeBase<TInstance>
        where TInstance : new()
    {
        /// <summary>
        /// The wrapped instance.
        /// </summary>
        public TInstance Output { get; protected set; } = new TInstance();

        #region Cachable<T>

        /// <summary>
        /// Tracks a single property. Only applies to the instance when the value actually changes.
        /// </summary>
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

        #endregion

        #region CachableList<T>

        /// <summary>
        /// Tracks a list property. Synchronizes contents only when the sequence differs.
        /// </summary>
        protected class CachableList<T>
        {
            private readonly TInstance _instance;
            private readonly Func<TInstance, IList<T>> _getter;
            private IReadOnlyList<T> _lastValue;

            public CachableList(VoxelNodeBase<TInstance> node, Func<TInstance, IList<T>> getter)
            {
                _instance = node.Output;
                _getter = getter;
                _lastValue = getter(node.Output)?.ToList() ?? [];
            }

            public IReadOnlyList<T> Value => (IReadOnlyList<T>)_getter(_instance);

            public void SetValue(IReadOnlyList<T> value)
            {
                if (!SequenceEqual(_lastValue, value))
                {
                    _lastValue = value;
                    var current = _getter(_instance);
                    current.Clear();
                    if (value != null)
                        foreach (var item in value)
                            if (item != null)
                                current.Add(item);
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

        #endregion
    }

    /// <summary>
    /// Mutable wrapper — a single TInstance lives for the node's lifetime.
    /// Properties are mutated in-place via Cachable setters. Downstream sees
    /// the same reference; changes are detected by the properties themselves.
    /// </summary>
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

    /// <summary>
    /// Immutable wrapper — emits a new TInstance reference whenever any
    /// tracked property changes, so downstream ReferenceEquals detects it.
    /// </summary>
    [ProcessNode]
    public abstract class VoxelNodeImmutable<TInstance> : VoxelNodeBase<TInstance>
        where TInstance : new()
    {
        private bool _isDirty = true;

        /// <summary>Whether any input changed since last rebuild.</summary>
        protected bool IsDirty => _isDirty;

        /// <summary>Signal that a rebuild is needed.</summary>
        protected void MarkDirty() => _isDirty = true;

        /// <summary>
        /// Tracks a property and auto-marks the node dirty on change.
        /// </summary>
        protected new class Cachable<T>
        {
            private static bool ReferenceEquals(T a, T b) => object.ReferenceEquals(a, b);

            private readonly VoxelNodeImmutable<TInstance> _node;
            private readonly Action<TInstance, T> _setter;
            private readonly Func<T, T, bool> _equals;
            private T _lastValue;

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
            }

            public T LastValue => _lastValue;

            public void SetValue(T value)
            {
                if (!_equals(_lastValue, value))
                {
                    _lastValue = value;
                    _node.MarkDirty();
                }
            }

            /// <summary>Apply the cached value to a fresh instance.</summary>
            public void ApplyTo(TInstance instance) => _setter(instance, _lastValue);
        }

        /// <summary>
        /// Create a fresh TInstance, apply all cached values via <paramref name="configure"/>,
        /// assign to Output, and clear the dirty flag.
        /// </summary>
        protected void Rebuild(Action<TInstance> configure = null)
        {
            var instance = new TInstance();
            configure?.Invoke(instance);
            Output = instance;
            _isDirty = false;
        }
    }
}
