using VL.Core.Import;

namespace VL.Stride.VoxelGI.Helpers
{
    [ProcessNode]
    public abstract class VoxelGINodeBase<TInstance>
        where TInstance : new()
    {
        public TInstance Output { get; protected set; } = new TInstance();

        protected class Cachable<T> : VoxelGINodeCachable<TInstance, T>
        {
            public Cachable(
                VoxelGINodeBase<TInstance> instance,
                Func<TInstance, T> getter,
                Action<TInstance, T> setter,
                Func<T, T, bool> equals = default
            )
                : base(instance, getter, setter, equals) { }

            public Cachable(
                VoxelGINodeBase<TInstance> instance,
                Func<TInstance, T> getter,
                Action<TInstance, T> setter,
                T initialValue,
                Func<T, T, bool> equals = default
            )
                : base(instance, getter, setter, initialValue, equals) { }
        }

        protected class CachableList<T> : VoxelGINodeCachableList<TInstance, T>
        {
            public CachableList(
                VoxelGINodeBase<TInstance> instance,
                Func<TInstance, IList<T>> getter
            )
                : base(instance, getter) { }
        }
    }
}
