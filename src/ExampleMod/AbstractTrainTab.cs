using Mafi.Core.Input;
using Mafi.Core.Trains;
using Mafi.Unity.Camera;
using Mafi.Unity.Ui;
using Mafi.Unity.Ui.Trains;
using Column = Mafi.Unity.UiToolkit.Library.Column;

namespace ExampleMod;

public abstract class AbstractTrainTab: Column
{
    protected readonly TrainsManager TrainsManager;
    protected readonly IInputScheduler MInputScheduler;
    protected readonly InspectorsManager MInspectorsManager;
    protected readonly CameraController MCameraController;
    protected readonly TrainDesignerWindow.Controller MTrainDesigner;
    protected readonly TrainLinesManager MTrainLinesManager;
    
    protected AbstractTrainTab(TrainsManager trainsManager,
        IInputScheduler mInputScheduler,
        InspectorsManager mInspectorsManager,
        CameraController mCameraController,
        TrainDesignerWindow.Controller mTrainDesigner,
        TrainLinesManager mTrainLinesManager)
    {
        TrainsManager = trainsManager;
        MInputScheduler = mInputScheduler;
        MInspectorsManager = mInspectorsManager;
        MCameraController = mCameraController;
        MTrainDesigner = mTrainDesigner;
        MTrainLinesManager = mTrainLinesManager;
    }
}