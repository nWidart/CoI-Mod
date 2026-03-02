using System;
using System.Collections.Generic;
using System.Linq;
using ExampleMod.UserInterface.Framework;
using Mafi;
using Mafi.Collections;
using Mafi.Core.Products;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.InputControl;
using Mafi.Unity.Ui.Library;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library;

namespace ExampleMod.UserInterface.RateCalculator;

[GlobalDependency(RegistrationMode.AsEverything)]
public class RateCalculatorWindowUi : Window
{
    private StatsSummery _statsSummery;
    private Dict<ProductProto, int> inputDictionary;
    private Dict<ProductProto, int> outputDictionary;

    private void SetStats(StatsSummery statsSummery)
    {
        _statsSummery = statsSummery;
    }

    private void SetProducts(Dict<ProductProto, int> inputDictionary, Dict<ProductProto, int> outputDictionary)
    {
        this.inputDictionary = inputDictionary;
        this.outputDictionary = outputDictionary;
    }

    public RateCalculatorWindowUi() : base(new LocStrFormatted("Rate Calculator"), true)
    {
        WindowSize(1000.px(), Px.Auto);
        MakeMovable();
        EnablePinning();

        var tableSection = UiFramework.StartNewSection(new LocStrFormatted("Table"));
        var tableUi = new TableUi.CellRow();
        tableUi.Add(c => c.JustifyItemsCenter().MinHeight(34.px()).Hide());
        tableUi.Add(new Label("No data yet".AsLoc()).FontItalic());
        tableSection.Add(tableUi);

        var statsPanel = UiFramework.StartNewPanel(new[] { GetStatsSection() });
        var tablePanel = UiFramework.StartNewPanel(new[] { tableSection });

        var ingredientsPanel = UiFramework.StartNewPanel(new[] { UiFramework.StartNewSection("Ingredients".AsLoc()) });
        var productsPanel = GetProductsPanel();
        var intermediatesPanel = UiFramework.StartNewPanel(new[] { UiFramework.StartNewSection("Intermediates".AsLoc()) });

        Body.Add(statsPanel, tablePanel, ingredientsPanel, productsPanel, intermediatesPanel);
    }

    // privateProperty = a Column
    // privateProperty.Observe<GameDate>((Func<GameDate>)(() => this.m_calendar.CurrentDate))
    //     .Do((Action<GameDate>)(date => dateDisplay.Value<Mafi.Unity.Ui.Library.Display>(date.FormatLong()
    //         .AsLoc())));
    private Panel GetProductsPanel()
    {
        var productsSection = UiFramework.StartNewSection("Products".AsLoc());
        var startNewPanel = UiFramework.StartNewPanel(new[] { productsSection });

        if (inputDictionary != null)
        {
            var icons = new List<Column>();

            foreach (var product in inputDictionary.Keys)
            {
                var icon = new Icon(product)
                    .Size(ProductQuantityUi.ICON_HEIGHT)
                    .MarginRight(10.px());
                var column = new Column(10.px()) { icon };
                // .Observe((Func<Column>)(() => new Column(10.px())));
                //  .Do(col => col.Add(new Icon(product)));
                icons.Add(column);
            }

            startNewPanel.Body.Add(new Row(10.px()) { icons.ToArray() });
        }

        return startNewPanel;
    }

    private Column GetStatsSection()
    {
        var maintenanceLabel = UiFramework.NewLabel("Total maintenance costs/month:");
        UiComponent maintenance1Display = new DisplayWithIcon()
            .IconValue("Assets/Base/Products/Icons/Maintenance1.svg")
            .ObserveValue(() => _statsSummery.TotalMaintenance1PerMonth.ToStringRounded());
        UiComponent maintenance2Display = new DisplayWithIcon()
            .IconValue("Assets/Base/Products/Icons/Maintenance2.svg")
            .ObserveValue(() => _statsSummery.TotalMaintenance2PerMonth.ToStringRounded());
        UiComponent maintenance3Display = new DisplayWithIcon()
            .IconValue("Assets/Base/Products/Icons/Maintenance3.svg")
            .ObserveValue(() => _statsSummery.TotalMaintenance3PerMonth.ToStringRounded());
        var maintenanceRow = UiFramework.StartNewRow(new[] { maintenanceLabel, maintenance1Display, maintenance2Display, maintenance3Display });

        var powerLabel = UiFramework.NewLabel("Total power required: ");
        UiComponent powerDisplay = new DisplayWithIcon()
            .IconValue("Assets/Unity/UserInterface/General/Electricity.svg")
            .Tooltip("Total power required to run selection".AsLoc())
            .ObserveValue(() => _statsSummery.TotalPowerRequired.Format());
        var powerRow = UiFramework.StartNewRow(new[] { powerLabel, powerDisplay });

        var workersLabel = UiFramework.NewLabel("Total workers required: ");
        UiComponent workersDisplay = new DisplayWithIcon()
            .IconValue("Assets/Unity/UserInterface/General/WorkerSmall.svg")
            .ObserveValue(() => _statsSummery.TotalWorkersAssigned);
        var workersRow = UiFramework.StartNewRow(new[] { workersLabel, workersDisplay });

        var computingLabel = UiFramework.NewLabel("Total computing required: ");
        UiComponent computingDisplay = new DisplayWithIcon()
            .IconValue("Assets/Unity/UserInterface/General/Computing128.png")
            .ObserveValue(() => _statsSummery.ComputingRequired.Format());
        var computingRow = UiFramework.StartNewRow(new[] { computingLabel, computingDisplay });

        return UiFramework.StartNewSection(new LocStrFormatted("Statistics"), new[] { maintenanceRow, powerRow, workersRow, computingRow });
    }

    [GlobalDependency(RegistrationMode.AsEverything)]
    public class Controller : WindowController<RateCalculatorWindowUi>
    {
        public Controller(ControllerContext controllerContext) : base(controllerContext)
        {
            controllerContext.UiRoot.AddDependency(this);
            controllerContext.InputManager
                .RegisterGlobalShortcut(_ => ShortcutsMap.Instance.OpenRateCalcWindow, this);
        }

        public void Open() => ActivateSelf();

        public void SetStats(StatsSummery statsSummery)
        {
            Window.SetStats(statsSummery);
        }

        public void SetProducts(Dict<ProductProto, int> inputDic, Dict<ProductProto, int> outputDic)
        {
            Window.SetProducts(inputDic, outputDic);
        }
    }
}