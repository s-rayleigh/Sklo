# Sklo

A lightweight UI framework for Unity that simplifies composing screens out of reusable layers and switching between them.

> [!WARNING]
> Sklo is a work in progress. The API is unstable and may change significantly between versions.

A *screen* is a plain C# class that declares which *layers* it consists of. Each layer is a piece of UI logic paired with a prefab view. The screen manager shows, hides and switches screens, reusing the layers that neighbouring screens share.

## Features

- Screens composed from reusable layers, declared in code
- Three display modes: **main** (one at a time, with history), **modal** and **overlay** (many at once, sorted by priority)
- Shared transitional layers stay on screen during a switch and get a transition callback instead of being recreated
- Back navigation through the history of main screens
- Typed arguments for screens and layers
- Sync API plus async variants for `Task` and [UniTask](https://github.com/Cysharp/UniTask), with cancellation support
- Pluggable layer view source (plain prefabs by default; Addressables, pooling, etc. via a custom provider)

## Purpose

UI screens in a typical project share a lot: backgrounds, headers, currency bars, and so on. This usually leads to duplicated prefabs or hand-written code that decides what to show, hide or keep on every screen change.

Sklo takes over that part. You describe what each screen consists of, and the framework works out which layers to create, keep, transition or release, while keeping the logic (layer) separate from the presentation (view).

## Installation

Requires Unity 2022.3 or newer. Add the package via *Package Manager → Add package from git URL*:

```
https://github.com/s-rayleigh/Sklo.git?path=Packages/com.rayleigh.sklo
```

The UniTask variant becomes available automatically when the `com.cysharp.unitask` package is installed.

## Main components

| Component | Role |
|---|---|
| `Screen` / `Screen<TArgs>` | Declares the layers of a screen in `OnCompose`. |
| `ScreenLayer<TLayer, TView>` | Logic of one layer, with `OnShow`, `OnTransition` and `OnHide` callbacks. |
| `ScreenLayerView<TView, TLayer>` | `MonoBehaviour` on the layer prefab: the visual part. |
| `ScreenManager` | Shows and hides screens: `ToggleMain`, `ToggleBack`, `ShowModal`, `ShowOverlay`, … |
| `ScreenManagerHost` | Scene component that creates the manager and holds the parent for views. |
| `ILayerViewProvider` | Supplies and releases view instances. `DefaultLayerViewProvider` instantiates from a list of prefabs. |
| `AsyncScreen`, `AsyncScreenLayer`, `AsyncScreenManager` | Awaitable counterparts in `Rayleigh.Sklo.Async.Tasks` and `Rayleigh.Sklo.Async.UniTasks`. |

## Usage

Define a layer and its view, then put the view on a prefab:

```csharp
public sealed class SettingsLayer : ScreenLayer<SettingsLayer, SettingsLayerView>
{
    protected override void OnShow() { /* ... */ }
}

public sealed class SettingsLayerView : ScreenLayerView<SettingsLayerView, SettingsLayer>
{
}
```

Compose a screen from layers:

```csharp
public sealed class SettingsScreen : Screen
{
    protected override void OnCompose(ScreenCompositor<ScreenLayer> compositor)
    {
        compositor.AddLayer<BackgroundLayer>();
        compositor.AddLayer<SettingsLayer>();
    }
}
```

Add a `ScreenManagerHost` to a UI object in the scene, register the layer prefabs in its `DefaultLayerViewProvider`, and use the manager:

```csharp
var manager = screenManagerHost.Manager;

manager.ToggleMain<MainScreen>();
manager.ToggleMain<SettingsScreen>(); // BackgroundLayer is kept and transitioned
manager.ToggleBack();                 // back to MainScreen

manager.ShowModal<MessageScreen, MessageData>(new MessageData { Text = "Hello" });
manager.HideModal();

var overlay = manager.ShowOverlay<DebugScreen>();
manager.HideOverlay(overlay);
```

More examples are in [Assets/Scripts/Examples](Assets/Scripts/Examples).

## Name

"Sklo" comes from the Ukrainian word "скло", meaning "glass": the material screens are made of, and one that can be stacked in sheets just like layers in the framework.

## License

Distributed under the [MIT License](LICENSE.md).
