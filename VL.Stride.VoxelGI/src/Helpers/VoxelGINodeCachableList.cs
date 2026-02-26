namespace VL.Stride.VoxelGI.Helpers
{
    public class VoxelGINodeCachableList<TInstance, T>
        where TInstance : new()
    {
        protected TInstance Instance;
        protected Func<TInstance, IList<T>> Getter;
        protected IReadOnlyList<T> LastValue;

        public VoxelGINodeCachableList(
            VoxelGINodeBase<TInstance> instance,
            Func<TInstance, IList<T>> getter
        )
        {
            Instance = instance.Output;
            Getter = getter;
            LastValue = getter(instance.Output)?.ToList() ?? [];
        }

        public IReadOnlyList<T> Value => (IReadOnlyList<T>)Getter(Instance);

        public void SetValue(IReadOnlyList<T> value)
        {
            if (!SequenceEqual(LastValue, value))
            {
                LastValue = value;
                var current = Getter(Instance);
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
            if (ReferenceEquals(a, b))
                return true;
            if (a is null || b is null || a.Count != b.Count)
                return false;
            return a.SequenceEqual(b);
        }
    }
}
