using System;
using ExampleMod.UserInterface.Framework;
using Mafi;
using Mafi.Core;
using Mafi.Localization;
using Mafi.Unity.InputControl;
using Mafi.Unity.Ui.Library;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library;
using Mafi.Unity.UiToolkit.Themes;

namespace ExampleMod.UserInterface.RateCalculator;

[GlobalDependency(RegistrationMode.AsEverything)]
public class RateCalculatorWindowUi : Window
{
    private StatsSummery _statsSummery;

    private void SetStats(StatsSummery statsSummery)
    {
        _statsSummery = statsSummery;
    }

    public RateCalculatorWindowUi() : base(new LocStrFormatted("Rate Calculator"), true)
    {
        WindowSize(1000.px(), 900.px());
        MakeMovable();
        EnablePinning();
        
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


        var tableSection = UiFramework.StartNewSection(new LocStrFormatted("Table"));
        var tableUi = new TableUi.CellRow();
        tableUi.Add(c => c.JustifyItemsCenter().MinHeight(34.px()).Hide());
        tableUi.Add(new Label("No data yet".AsLoc()).FontItalic());
        tableSection.Add(tableUi);
        
        
        var statsSection = UiFramework.StartNewSection(new LocStrFormatted("Stats for selection"), new[] {maintenanceRow, powerRow, workersRow, computingRow });
        var overviewPanel = UiFramework.StartNewPanel(new[] { statsSection, tableSection });
        Body.Add(overviewPanel);
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
            Log.Info(statsSummery.ComputingRequired.ToString());
        }
    }
}