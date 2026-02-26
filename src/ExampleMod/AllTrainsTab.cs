using Mafi.Core.Input;
using Mafi.Core.Trains;
using Mafi.Unity.Camera;
using Mafi.Unity.Ui;
using Mafi.Unity.Ui.Trains;
using Column = Mafi.Unity.UiToolkit.Library.Column;

namespace ExampleMod;

public class AllTrainsTab : Column
{
    private readonly TrainsManager _trainsManager;

    public AllTrainsTab(
        TrainsManager trainsManager,
        IInputScheduler mInputScheduler,
        InspectorsManager mInspectorsManager,
        CameraController mCameraController,
        TrainDesignerWindow.Controller mTrainDesigner,
        TrainLinesManager mTrainLinesManager)
    {
        _trainsManager = trainsManager;

        Column trainsColumn = new TrainUiColumn(mInputScheduler,
            mInspectorsManager,
            mCameraController,
            mTrainLinesManager,
            mTrainDesigner,
            () => _trainsManager.Trains.AsEnumerable());
        
        this.Add(trainsColumn);
    }
}