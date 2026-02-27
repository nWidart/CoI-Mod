using System;
using Mafi;
using Mafi.Core.Input;
using Mafi.Core.Trains;
using Mafi.Localization;
using Mafi.Unity.Camera;
using Mafi.Unity.InputControl;
using Mafi.Unity.Ui;
using Mafi.Unity.Ui.Trains;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library;
using UnityEngine;

namespace ExampleMod;

[GlobalDependency(RegistrationMode.AsEverything)]
public class TrainsUIWindow : Window
{
    private readonly TrainsManager _trainsManager;

    public TrainsUIWindow(TrainsManager trainsManager,
        IInputScheduler mInputScheduler,
        InspectorsManager mInspectorsManager,
        CameraController mCameraController,
        TrainDesignerWindow.Controller mTrainDesigner,
        TrainLinesManager mTrainLinesManager,
        ShortcutsManager shortcutsManager
    ) : base(new LocStrFormatted("Trains Management"))
    {
        _trainsManager = trainsManager;

        WindowSize(1000.px(), 900.px());
        MakeMovable();
        EnablePinning();
        Title(new LocStrFormatted("Trains Management"));

        var allTrainsTab = new AllTrainsTab(trainsManager, mInputScheduler, mInspectorsManager, mCameraController, mTrainDesigner, mTrainLinesManager);
        var stuckTrainsTab = new StuckTrainsTab(trainsManager, mInputScheduler, mInspectorsManager, mCameraController, mTrainDesigner, mTrainLinesManager);
        
        var tabs = new TabContainer()
        {
            {
                "Trains".AsLoc(),
                allTrainsTab
            },
            {
                "Stuck Trains".AsLoc(),
                stuckTrainsTab
            }
        };

        Body.Add(tabs);

        var shortcut = shortcutsManager.ResolveShortcutToString("Trains Window",
            _ => ShortcutsMap.Instance.OpenTrainsWindow);
        ShortcutToShow(shortcut);
    }

    [GlobalDependency(RegistrationMode.AsEverything)]
    public class Controller : WindowController<TrainsUIWindow>
    {
        public Controller(ControllerContext controllerContext) : base(controllerContext)
        {
            controllerContext.UiRoot.AddDependency(this);
            controllerContext.InputManager
                .RegisterGlobalShortcut(_ => ShortcutsMap.Instance.OpenTrainsWindow, this);
        }

        public void Open() => this.ActivateSelf();
    }
}