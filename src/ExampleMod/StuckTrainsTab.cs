using System.Collections.Generic;
using Mafi;
using Mafi.Core.Input;
using Mafi.Core.Trains;
using Mafi.Unity.Camera;
using Mafi.Unity.Ui;
using Mafi.Unity.Ui.Trains;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library;

namespace ExampleMod;

public class StuckTrainsTab: AbstractTrainTab
{
    private IEnumerable<Train> _trains;

    public StuckTrainsTab(TrainsManager trainsManager,
        IInputScheduler mInputScheduler,
        InspectorsManager mInspectorsManager,
        CameraController mCameraController,
        TrainDesignerWindow.Controller mTrainDesigner,
        TrainLinesManager mTrainLinesManager) : base(trainsManager,
        mInputScheduler,
        mInspectorsManager,
        mCameraController,
        mTrainDesigner,
        mTrainLinesManager)
    {
        Log.Info("StuckTrainsTab Loaded");
        FetchTrains();
        Add(GetTrainsRow().AlignSelfStretch());
    }
    
    private UiComponent GetTrainsRow()
    {
        Column trainsColumn = new TrainUiColumn(MInputScheduler,
            MInspectorsManager,
            MCameraController,
            MTrainLinesManager,
            MTrainDesigner,
            () => _trains);

        return trainsColumn;
    }
    
    private void FetchTrains()
    {
        foreach (var train in TrainsManager.Trains.AsEnumerable())
        {
            Log.Info(train.StateForUi.ToString());
        }

        _trains = TrainsManager.Trains.Where(train => train.StateForUi == TrainStateForUi.WaitingForFreeTrack);
    }
}