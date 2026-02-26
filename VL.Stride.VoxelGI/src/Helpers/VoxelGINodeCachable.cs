using Stride.Rendering.Materials;

namespace VL.Stride.VoxelGI.Helpers
{
    public class VoxelGINodeCachable<TInstance, T>
        where TInstance : new()
    {
        private static bool ReferenceEquals(T instance1, T instance2) =>
            object.ReferenceEquals(instance1, instance2);

        protected TInstance Instance;
        protected Func<TInstance, T> Getter;
        protected Action<TInstance, T> Setter;
        protected Func<T, T, bool> EqualsFunc;

        protected T LastValue;

        public VoxelGINodeCachable(
            VoxelGINodeBase<TInstance> instance,
            Func<TInstance, T> getter,
            Action<TInstance, T> setter,
            Func<T, T, bool> equals = default
        )
            : this(instance, getter, setter, getter(instance.Output), equals) { }

        public VoxelGINodeCachable(
            VoxelGINodeBase<TInstance> instance,
            Func<TInstance, T> getter,
            Action<TInstance, T> setter,
            T initialValue,
            Func<T, T, bool> equals = default
        )
        {
            Instance = instance.Output;
            Getter = getter;
            Setter = setter;

            EqualsFunc =
                equals
                ?? (
                    typeof(T).IsAssignableTo(typeof(IMaterialShaderGenerator))
                        ? ReferenceEquals
                        : EqualityComparer<T>.Default.Equals
                );

            LastValue = initialValue;
        }

        public T Value => Getter(Instance);

        public void SetValue(T value)
        {
            if (!EqualsFunc(LastValue, value))
            {
                LastValue = value;
                Setter(Instance, value);
            }
        }
    }
}
