using System;
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
    private Option<string> _searchText;
    private readonly Label _nothingFoundInfo;
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
        FetchTrains();

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
        searchRow.Add((this._nothingFoundInfo = new Label()).AlignSelfCenter().Hide());

        Column trainsColumn = new TrainUiColumn(mInputScheduler,
            mInspectorsManager,
            mCameraController,
            mTrainLinesManager,
            mTrainDesigner,
            () => this._trains);

        this.Add(searchRow.AlignSelfStretch());
        this.Add(new HorizontalDivider().AlignSelfStretch());
        this.Add(trainsColumn.AlignSelfStretch());
    }

    private IEnumerable<Train> FetchTrains()
    {
        this._trains = _trainsManager.Trains.AsEnumerable();
        return this._trains;
    }

    private void Update()
    {
        if (!this._searchText.HasValue)
        {
            this._trains = FetchTrains();
            this._nothingFoundInfo.Show();
            return;
        }

        this._nothingFoundInfo.Hide();
        this._trains = _trainsManager.Trains
            .Where(train =>
                train.Name.ToLower().Contains(this._searchText.Value.ToLower())
            );
    }

    private void Search(string text)
    {
        this._searchText = string.IsNullOrWhiteSpace(text) ? Option<string>.None : (Option<string>)text;
        Update();
    }
}