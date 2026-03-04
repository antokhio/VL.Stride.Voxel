<img src="hero.png" width="100%" alt="VL.Stride.Voxels Banner">

# VL.Stride.Voxels

[Stride](https://github.com/stride3d/stride) voxel rendering and compositing integration for [vvvv](https://vvvv.org).

This project brings [Voxel Cone Tracing Global Illumination](https://doc.stride3d.net/4.2/en/Manual/graphics/lights-and-shadows/voxel-cone-tracing-gi.html) to the visual programming environment, exposing the `Stride.Voxels` functionality as VL nodes. It allows developers and creative coders working in vvvv gamma to implement voxel-based Stride rendering techniques natively within their workflows by installing the NuGet package.

⚠️ **This project is currently in development** and may be subject to breaking changes, deprecations, and unstable features.

## Packages

- **VL.Stride.Voxels** - Wrapper for **vvvv gamma**. Provides a node set to construct voxel compositors and rendering pipelines interactively.

## Dependencies

- **Stride.Voxels** - Core C# library containing the Stride voxel rendering implementation. Can be used in any Stride game or .NET application.

### Installation

As of now, you must install **two** packages manually:

```sh
// Install the VL wrapper
nuget install VL.Stride.Voxels

// Install the core Stride package without dependencies (to avoid downloading packages already included in vvvv)
nuget install Stride.Voxels -Version 4.2.1.2487 -DependencyVersion Ignore
```

**Compatibility**: Compatible with **>= vvvv_gamma_7.1-0174**

**Notice**: The `Stride.Voxels` version should match the Stride version vvvv is currently using. If you are on the latest pre-release version of vvvv, check [StrideVersion](https://github.com/vvvv/VL.StandardLibs/blob/main/Directory.Packages.props). If you are on a stable release, check the vvvv **About** dialog.

### Getting Started

Help patches for [vvvv](https://vvvv.org) are available via the Help Browser under the `Stride Voxels` category.

## Contributing

Contributions are welcome!
Open [issues](https://github.com/antokhio/VL.Stride.Voxels/issues) or submit pull requests.
Questions are welcome on the [vvvv forum](https://forum.vvvv.org).

## Known Issues

Please check the [issue tracker](https://github.com/antokhio/VL.Stride.Voxels/issues) for known bugs and current limitations.

## Credits

- [tebjan](https://github.com/tebjan) - all the heavy lifting to assemble a working example.
- [bj-rn](https://github.com/bj-rn) - maintaining the branch.
- [antokhio](https://github.com/antokhio) - aligning the code with the vvvv API.

## License

The source code is published under the [MIT License](https://opensource.org/licenses/MIT). See the [LICENSE.md](LICENSE.md) file for details.