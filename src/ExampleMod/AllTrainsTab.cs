using System.Collections.Generic;
using Mafi;
using Mafi.Core;
using Mafi.Core.Input;
using Mafi.Core.Trains;
using Mafi.Localization;
using Mafi.Unity.Camera;
using Mafi.Unity.Ui;
using Mafi.Unity.Ui.Library;
using Mafi.Unity.Ui.Trains;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library;
using Column = Mafi.Unity.UiToolkit.Library.Column;

namespace ExampleMod;

public class AllTrainsTab : Column
{
    private readonly TrainsManager _trainsManager;
    private readonly IInputScheduler _mInputScheduler;
    private readonly InspectorsManager _mInspectorsManager;
    private readonly CameraController _mCameraController;
    private readonly TrainDesignerWindow.Controller _mTrainDesigner;
    private readonly TrainLinesManager _mTrainLinesManager;
    private Option<string> _searchText;
    private Label _nothingFoundInfo;
    private IEnumerable<Train> _trains;

    public AllTrainsTab(
        TrainsManager trainsManager,
        IInputScheduler mInputScheduler,
        InspectorsManager mInspectorsManager,
        CameraController mCameraController,
        TrainDesignerWindow.Controller mTrainDesigner,
        TrainLinesManager mTrainLinesManager)
    {
        _trainsManager = trainsManager;
        _mInputScheduler = mInputScheduler;
        _mInspectorsManager = mInspectorsManager;
        _mCameraController = mCameraController;
        _mTrainDesigner = mTrainDesigner;
        _mTrainLinesManager = mTrainLinesManager;
        FetchTrains();

        Add(GetSearchRow().AlignSelfStretch());
        Add(new HorizontalDivider().AlignSelfStretch());
        Add(GetFilterRow().AlignSelfStretch());
        Add(new HorizontalDivider().AlignSelfStretch());
        Add(GetTrainsRow().AlignSelfStretch());
    }

    private UiComponent GetFilterRow()
    {
        // var buttonIconText = new ButtonIconText(Button.ToggleGroup, product.IconPath, (LocStrFormatted) product.Strings.Name);
        // var observeSelected = buttonIconText.ObserveSelected<ButtonIconText>((Func<bool>) (() => this.m_selectedProduct == product));
        // this.m_fuelTypeButtonsRow.Add((UiComponent) observeSelected.OnClick<ButtonIconText>((Action) (() => this.selectProduct((Option<ProductProto>) product))));

        var filterGroupButtons = new ToggleButtonGroupRow();

        var buttonOne = new ButtonText(Button.ToggleGroup, Tr.TrainStatus_NotSpawned.AsFormatted);
        buttonOne.OnClick(evt => Log.Info(evt.ToString()));
        var buttonTwo = new ButtonText(Button.ToggleGroup, Tr.TrainStatus_AtStation.AsFormatted);
        var buttonThree = new ButtonText(Button.ToggleGroup, Tr.TrainStatus_WaitingForFreeTrack.AsFormatted);
        var buttonFour = new ButtonText(Button.ToggleGroup, Tr.TrainStatus_ArrivalConditionsNotMet.AsFormatted);
        filterGroupButtons.Add(buttonOne, buttonTwo, buttonThree, buttonFour);


        var row = new Row
        {
            new Title(new LocStrFormatted("Train Status")).NoBorder(),
            new VerticalDivider().MarginLeftRight(2.pt()),
            filterGroupButtons
        };
        return row;
    }

    private UiComponent GetSearchRow()
    {
        _nothingFoundInfo = new Label().AlignSelfCenter().Hide();

        var searchRow = new Row();
        var searchColumn = new Column();
        var textField = new TextField()
            .ClearFocusOnEscape()
            .OnValueChanged(this.Search)
            .Placeholder((LocStrFormatted)Tr.Search)
            .AlignSelfStretch();

        searchColumn.Add(textField.CharLimit(60).AddClearButton());
        searchColumn.Add(c => c.Width(100.Percent()));

        searchRow.Add(searchColumn.AlignSelfStretch());
        searchRow.Add(this._nothingFoundInfo);

        return searchRow;
    }

    private UiComponent GetTrainsRow()
    {
        Column trainsColumn = new TrainUiColumn(_mInputScheduler,
            _mInspectorsManager,
            _mCameraController,
            _mTrainLinesManager,
            _mTrainDesigner,
            () => _trains);

        return trainsColumn;
    }

    private IEnumerable<Train> FetchTrains()
    {
        _trains = _trainsManager.Trains.AsEnumerable();
        return _trains;
    }

    private void Update()
    {
        if (!_searchText.HasValue)
        {
            _trains = FetchTrains();
            _nothingFoundInfo.Show();
            return;
        }

        _nothingFoundInfo.Hide();
        _trains = _trainsManager.Trains
            .Where(train =>
                train.Name.ToLower().Contains(_searchText.Value.ToLower())
            );
    }

    private void Search(string text)
    {
        _searchText = string.IsNullOrWhiteSpace(text) ? Option<string>.None : (Option<string>)text;
        Update();
    }
}