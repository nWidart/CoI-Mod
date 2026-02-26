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
    private readonly TextField searchBox;
    private Option<string> searchText;
    private readonly Label nothingFoundInfo;
    private IEnumerable<Train> trains;

    public AllTrainsTab(
        TrainsManager trainsManager,
        IInputScheduler mInputScheduler,
        InspectorsManager mInspectorsManager,
        CameraController mCameraController,
        TrainDesignerWindow.Controller mTrainDesigner,
        TrainLinesManager mTrainLinesManager)
    {
        _trainsManager = trainsManager;
        InitTrains();

        var searchRow = new Row();
        var searchColumn = new Column();
        var textField = new TextField()
            .ClearFocusOnEscape()
            .OnValueChanged(this.Search)
            .Placeholder((LocStrFormatted)Tr.Search)
            .AlignSelfStretch();
        var searchField = new PanelSubHeader()
            .BodyAdd(textField.CharLimit(60).AddClearButton());
        searchColumn.Add(searchField);
        searchColumn.Add(c => c.Width(100.Percent()));

        searchRow.Add(searchColumn.AlignSelfStretch());
        searchRow.Add((this.nothingFoundInfo = new Label()).AlignSelfCenter().Hide());
        
        Column trainsColumn = new TrainUiColumn(mInputScheduler,
            mInspectorsManager,
            mCameraController,
            mTrainLinesManager,
            mTrainDesigner,
            () => this.trains);
        var trainRow = new Row { trainsColumn.AlignSelfStretch() };

        this.Add(searchRow.AlignSelfStretch());
        this.Add(new HorizontalDivider().AlignSelfStretch());
        this.Add(trainRow.AlignSelfStretch());
    }

    private IEnumerable<Train> InitTrains()
    {
        this.trains = _trainsManager.Trains.AsEnumerable();
        return this.trains;
    }

    private void Update()
    {
        if (this.searchText.HasValue)
        {
            this.nothingFoundInfo.Hide();
            this.trains = _trainsManager.Trains.Where(train => train.Name.ToLower().Contains(this.searchText.Value.ToLower()));
        }
        else
        {
            this.trains = InitTrains();
            this.nothingFoundInfo.Show();
        }
    }

    private void Search(string text)
    {
        this.searchText = string.IsNullOrWhiteSpace(text) ? Option<string>.None : (Option<string>)text;
        Log.Info(text);
        Update();
    }
}