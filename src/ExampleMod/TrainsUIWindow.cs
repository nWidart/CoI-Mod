using Mafi;
using Mafi.Core.Input;
using Mafi.Core.Trains;
using Mafi.Localization;
using Mafi.Unity.Camera;
using Mafi.Unity.InputControl;
using Mafi.Unity.Ui;
using Mafi.Unity.Ui.Trains;
using Mafi.Unity.UiToolkit.Library;

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
        TrainLinesManager mTrainLinesManager) : base(new LocStrFormatted("Trains Management"))
    {
        _trainsManager = trainsManager;

        this.WindowSize(1000.px(), 900.px());
        this.MakeMovable();
        this.EnablePinning();
        this.Title(new LocStrFormatted("Trains Management"));

        Column trainsColumn = new TrainUiColumn(mInputScheduler,
            mInspectorsManager,
            mCameraController,
            mTrainLinesManager,
            mTrainDesigner,
            () => _trainsManager.Trains.AsEnumerable());
        this.Body.Add(trainsColumn);
    }

    [GlobalDependency(RegistrationMode.AsEverything)]
    public class Controller : WindowController<TrainsUIWindow>
    {
        public Controller(ControllerContext controllerContext) : base(controllerContext)
        {
            controllerContext.UiRoot.AddDependency(this);
        }

        public void Open() => this.ActivateSelf();
    }
}