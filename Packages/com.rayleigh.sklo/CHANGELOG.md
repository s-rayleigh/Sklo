# Changelog

All notable changes to the Sklo package are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/). The project is in early development, so minor versions may contain breaking changes.

## [Unreleased]

## [0.4.0] - 2026-11-11

### Added

- Typed arguments for screens: `Screen<TArgs>` / `AsyncScreen<TArgs>` and screen manager overloads that accept arguments. A screen keeps its arguments when shown again from the history.
- Typed arguments for layers: `ILayerWithArgs<TArgs>` and `ScreenCompositor.AddLayer<T, TArgs>(args)`.
- Optional [UniTask](https://github.com/Cysharp/UniTask) support (`Rayleigh.Sklo.Async.UniTasks`), enabled automatically when the UniTask package is installed.
- `CancellationToken` support in async screen manager methods and in the `On*` callbacks of async screens and layers, so animations and transitions can be skipped.
- `ILayerViewProvider` for custom sources of layer views (Addressables, pooling, etc.), with `DefaultLayerViewProvider` that instantiates views from a list of prefabs.
- Optional key for layers, used to request a matching view from the provider.
- `ScreenManagerHost` component that creates the screen manager in a scene.
- `BaseScreen.TryGetLayer` method.

### Changed

- **Breaking:** screen managers are now plain C# classes instead of `MonoBehaviour`s. Use `ScreenManagerHost` or construct them with a layer view provider and a parent for views.
- **Breaking:** namespaces and folders are rearranged (`Rayleigh.Sklo.Screens`, `.Layers`, `.LayerViews`, `.ScreenManagers`); task-based async classes moved to `Rayleigh.Sklo.Async.Tasks`.
- **Breaking:** `Notify*` methods of async screens and layers are now internal.
- Async support is split into separate assemblies.
- Minimum tested Unity version is 2022.3.62.

### Removed

- **Breaking:** "init" callbacks of the methods that display screens. Use screen arguments instead.

### Fixed

- Operation id was not incremented correctly for nested operations.
- Releasing unused layers could skip some of them.
- Layer views were not released correctly.
- Hiding a modal screen in async screen managers.
- A synchronous cancellation in an async callback could leave an operation unfinished.

## [0.3.1] - 2026-04-27

### Added

- Show, hide and toggle operations can be started from within callbacks of another operation (recursive operations).

### Fixed

- Display type was not set correctly on layers, including when a screen is being hidden.
- Parent object of an overlay screen was not destroyed after the screen was hidden.

## [0.3.0] - 2025-02-26

### Added

- Overlay screens: several can be displayed at once on top of the main and modal screens, in both sync and task-based async screen managers.
- `AfterScreenShow` and `AfterScreenHide` events of the screen manager.
- `DisplayedScreens` property of the screen manager.
- `Layers` property of the screen.

## [0.2.0] - 2025-02-17

First tagged release.

### Added

- Screens composed from layers, with layer transitions when switching between main screens.
- History of main screens with `ToggleBack`, and methods to hide the main screen.
- Task-based async screens, layers and screen manager.
- Modal screens, in both sync and async screen managers.
- `BeforeLayerShow`, `BeforeLayerHide`, `BeforeScreenShow` and `BeforeScreenHide` events of the screen manager.
- Optional "init" callbacks for the methods that display screens.
- `DisplayType` property of screens and layers.
- `ParentContainer` property of the screen.
- Access to the layer directly from its view.
- Framework code is distributed as a separate package, under the MIT license.

### Fixed

- Screen container could have an undesired offset or not fill the canvas.
- Layers were not hidden and their views were not destroyed correctly.
- `ToggleBack` behaviour.
- A leak in the async screen manager.

[Unreleased]: https://github.com/s-rayleigh/Sklo/compare/v0.4.0...HEAD
[0.4.0]: https://github.com/s-rayleigh/Sklo/compare/v0.3.1...v0.4.0
[0.3.1]: https://github.com/s-rayleigh/Sklo/compare/v0.3.0...v0.3.1
[0.3.0]: https://github.com/s-rayleigh/Sklo/compare/v0.2.0...v0.3.0
[0.2.0]: https://github.com/s-rayleigh/Sklo/releases/tag/v0.2.0
