# Copilot Instructions — VL.Stride.Voxel

## Project Guidelines

- This project wraps external Stride.Voxels types as vvvv `[ProcessNode]` nodes using the `VoxelNodeBase` infrastructure in `VL.Stride.Voxel`.

## VoxelNodeBase Pattern

### Overview

`VoxelNodeBase<TInstance>` is a generic base class for wrapping **external Stride datatype classes** as vvvv `[ProcessNode]` nodes. It handles the boilerplate of:

- Owning a `TInstance` (the wrapped Stride object)
- Exposing it as `public TInstance Output { get; }` — vvvv reads this automatically, no `Update(out ...)` needed
- Providing `Cachable<T>` and `CachableList<T>` inner helpers for per-property change detection

### Two Subclasses

| Base Class | Use When | Output Behavior |
|---|---|---|
| `VoxelNodeMutable<TInstance>` | `TInstance` is **mutable** — has settable properties | Single instance created once; properties mutated in-place via `Cachable` setters |
| `VoxelNodeImmutable<TInstance>` | `TInstance` is **immutable** or needs a **new reference** on any change | New `TInstance` emitted whenever any property changes |

#### Descriptor-Only Nodes (no properties)

When `TInstance` has no configurable properties (just needs to exist), use `VoxelNodeMutable` with no `Cachable` fields and no `Set*` methods. The constructor creates the instance; `Output` exposes it.

### Authoring a Node

#### Mutable Example

```csharp
[ProcessNode(Name = "StorageClipmaps")]
public class StorageClipmaps : VoxelNodeMutable<VoxelStorageClipmaps>
{
    private readonly Cachable<int> _clipMapCount;
    private readonly Cachable<float> _clipMapResolution;

    public StorageClipmaps()
    {
        _clipMapCount = new(this, x => x.ClipMapCount, (x, v) => x.ClipMapCount = v, 4);
        _clipMapResolution = new(this, x => x.ClipMapResolution, (x, v) => x.ClipMapResolution = v, 128f);
    }

    public void SetClipMapCount(int clipMapCount) => _clipMapCount.SetValue(clipMapCount);
    public void SetClipMapResolution(float clipMapResolution) => _clipMapResolution.SetValue(clipMapResolution);
}
```

#### Immutable Example

```csharp
[ProcessNode(Name = "VoxelLayout")]
public class VoxelLayout : VoxelNodeImmutable<VoxelLayoutAnisotropic>
{
    private readonly Cachable<int> _resolution;

    public VoxelLayout()
    {
        _resolution = new(this, x => x.Resolution, (x, v) => x.Resolution = v, 64);
    }

    public void SetResolution(int resolution) => _resolution.SetValue(resolution);
}
```

#### Descriptor-Only (No Properties)

```csharp
[ProcessNode(Name = "MarchCones")]
public class MarchCones : VoxelNodeMutable<VoxelMarchCones>
{
    // No Cachable fields, no Set methods — just wraps the type
}
```

### Rules

1. **`Set*` method names** — each public `Set*` method becomes a vvvv input pin. Name it `Set` + PropertyName.
2. **Default values must match** — when a `Cachable` field is constructed with an initial value, the corresponding `Set*` parameter default MUST be the same value.
3. **No `Update(out TInstance ...)` needed** — `Output` is a public property; vvvv picks it up automatically.
4. **No "Node" suffix** — class name (or `ProcessNode.Name`) is the vvvv-visible name. Never include "Node".
5. **Cachable handles change detection** — never manually compare values. Just call `_field.SetValue(value)`.
6. **CachableList for collection properties** — use `CachableList<T>` when the Stride type exposes `IList<T>`.
7. **Zero allocations in Set methods** — `Cachable.SetValue` only writes when value differs.
8. **VoxelNodeImmutable recreates** `Output` — after any `Cachable.SetValue` detects a change, `Output` is replaced with a new `TInstance` and all cached values are re-applied. This is necessary when downstream consumers compare by reference.
9. **XML doc comments** — add `<summary>` on the class (tooltip in vvvv).
10. **Omit explicit null defaults** — do not specify `= null` for reference type parameters in `Set*` methods, as reference types default to null implicitly.

## Cachable<T> API

```csharp
// Construction — always in the node constructor
_field = new Cachable<T>(
    this,                          // the VoxelNodeBase instance
    x => x.Property,               // getter from TInstance
    (x, v) => x.Property = v,      // setter on TInstance
    initialValue                   // optional — default value of the property
);

// Usage — in a Set* method
_field.SetValue(value);  // only applies if value differs from last

// Reading current value
T current = _field.Value;
```

## CachableList<T> API

```csharp
// Construction
_items = new CachableList<T>(this, x => x.Items);

// Set from list
_items.SetValue(readOnlyList);

// Set single item (wraps in single-element list)
_items.SetValue(singleItem);
```

## Development Preferences

- Avoid using the .NET modernization experience/tool in this workflow.