# Copilot Instructions — VL.Stride.Voxel

## Project Guidelines

- This project wraps external Stride.Voxels types as vvvv `[ProcessNode]` nodes using the `ProcessNodeBase` infrastructure in `VL.Core.Import`.

## ProcessNodeBase Pattern

### Overview

`ProcessNodeBase<TInstance>` is a generic base class for wrapping **external Stride datatype classes** as vvvv `[ProcessNode]` nodes. It handles the boilerplate of:

- Owning a `TInstance` (the wrapped Stride object)
- Exposing it as `public TInstance Output { get; }` — vvvv reads this automatically, no `Update(out ...)` needed
- Providing `Cachable<T>` and `CachableList<T>` inner helpers for per-property change detection

### Mutability via IsImmutable

By default, `ProcessNodeBase` treats `TInstance` as **immutable** (or needing a new reference on any change). It emits a new `TInstance` whenever any property changes.

To make a node **mutable** (modifying properties in-place without recreating the instance), you must override `IsImmutable`. This is absolutely essential for rendering components like PipelineProcessors, RenderFeatures, and ImageEffects which hold unmanaged GPU resources or state.

```csharp
protected override bool IsImmutable => false;
```
### Authoring a Node

#### Immutable Example (Default Behavior)

```csharp
[ProcessNode(Name = "VoxelLayout")]
public class VoxelLayoutNode : ProcessNodeBase<VoxelLayoutAnisotropic>
{
    private readonly Cachable<int> _resolution;

    public VoxelLayoutNode()
    {
        // Simplified constructor: Node, Setter, Default Value
        _resolution = new(this, (x, v) => x.Resolution = v, 64);
    }

    public void SetResolution(int resolution = 64) => _resolution.SetValue(resolution);
}
```

#### Mutable Example (Override IsImmutable)

```csharp
[ProcessNode(Name = "StorageClipmaps")]
public class StorageClipmapsNode : ProcessNodeBase<VoxelStorageClipmaps>
{
    // Mutates properties in-place instead of creating a new instance
    protected override bool IsImmutable => false; 

    private readonly Cachable<int> _clipMapCount;
    private readonly Cachable<float> _clipMapResolution;

    public StorageClipmapsNode()
    {
        _clipMapCount = new(this, (x, v) => x.ClipMapCount = v, 4);
        _clipMapResolution = new(this, (x, v) => x.ClipMapResolution = v, 128f);
    }

    public void SetClipMapCount(int clipMapCount = 4) => _clipMapCount.SetValue(clipMapCount);
    public void SetClipMapResolution(float clipMapResolution = 128f) => _clipMapResolution.SetValue(clipMapResolution);
}
```

#### Descriptor-Only (No Properties)
```csharp
[ProcessNode(Name = "MarchCones")]
public class MarchConesNode : ProcessNodeBase<VoxelMarchCones>
{
    // No Cachable fields, no Set methods — just wraps the type.
    // (Add `protected override bool IsImmutable => false;` if it is a stateful renderer/processor).
}
```

### Rules

1. **`Set*` method names** — each public `Set*` method becomes a vvvv input pin. Name it `Set` + PropertyName.
2. **Default values must match** — when a `Cachable` field is constructed with an initial value, the corresponding `Set*` parameter default MUST exactly match.
3. **No `Update(out TInstance ...)` needed** — `Output` is a public property; vvvv picks it up automatically.
4. **Node Naming** — The class should generally end in `Node` (e.g., `MyTypeNode`) but use `[ProcessNode(Name = "MyType")]` to display cleanly in vvvv.
5. **Cachable handles change detection** — never manually compare values. Just call `_field.SetValue(value)`.
6. **CachableList for collection properties** — use `CachableList<T>` when the Stride type exposes `IList<T>` or `List<T>`.
7. **Zero allocations in Set methods** — `Cachable.SetValue` only writes when the value differs.
8. **IsImmutable recreates Output** — if `IsImmutable` is true (default), `Output` is replaced with a new `TInstance` on change. Set `IsImmutable => false` for renderers, processors, and stateful GPU objects.
9. **XML doc comments** — add `<summary>` on the class (provides the tooltip in vvvv).
10. **Omit explicit null defaults** — do not specify `= null` for reference type parameters in `Set*` methods, as reference types default to null implicitly in vvvv.

```csharp
// Construction — always in the node constructor.
// The getter (x => x.Property) is no longer required for most simple types!
_field = new Cachable<T>(
    this,                          // the ProcessNodeBase instance
    (x, v) => x.Property = v,      // setter on TInstance
    initialValue                   // optional — default value of the property
);

// Usage — in a Set* method
_field.SetValue(value);  // only applies if value differs from last
```

### CachableList<T> API

```
// Construction — Needs the getter and a casted setter 
// (because vvvv provides an IReadOnlyList, but Stride requires a concrete List)
_items = new CachableList<T>(
    this, 
    x => x.Items, 
    (x, v) => x.Items = (List<T>)v
);

// Usage - in a Set* method taking IReadOnlyList<T>
_items.SetValue(readOnlyList);
```