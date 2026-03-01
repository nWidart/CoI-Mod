using ExampleMod.UserInterface.Framework;
using Mafi;
using Mafi.Localization;
using Mafi.Unity.InputControl;
using Mafi.Unity.UiToolkit.Library;

namespace ExampleMod.UserInterface.RateCalculator;

[GlobalDependency(RegistrationMode.AsEverything)]
public class RateCalculatorWindowUi: Window
{
    private StatsSummery _statsSummery;
    
    private Label _maintenanceLabel;
    private Label _powerLabel;
    private Label _workersLabel;
    private Column _mainSection;

    public RateCalculatorWindowUi() : base(new LocStrFormatted("Rate Calculator"), true)
    {
        WindowSize(1000.px(), 900.px());
        
        _maintenanceLabel = UiFramework.NewLabel("");
        _powerLabel = UiFramework.NewLabel("");
        _workersLabel = UiFramework.NewLabel("");

        _mainSection = UiFramework.StartNewSection(new LocStrFormatted("Stats"));
        Body.Add(_mainSection);
    }

    private void RefreshStatsLabels()
    {
        var maintenance = _statsSummery.TotalMaintenancePerMonth.ToString();
        var power = _statsSummery.TotalPowerRequired.ToString();
        var workers = _statsSummery.TotalWorkersAssigned.ToString();
        
        _maintenanceLabel = UiFramework.NewLabel($"Total maintenance costs/month: {maintenance}");
        _powerLabel = UiFramework.NewLabel($"Total power required: {power} KW");
        _workersLabel = UiFramework.NewLabel($"Total workers assigned: {workers}");

        var maintRow = UiFramework.StartNewRow(new[] { _maintenanceLabel });
        var powerRow = UiFramework.StartNewRow(new[] { _powerLabel });
        var workersRow = UiFramework.StartNewRow(new[] { _workersLabel });
        
        _mainSection.Clear();
        _mainSection.Add(maintRow, powerRow, workersRow);
    }

    private void SetStats(StatsSummery statsSummery)
    {
        _statsSummery = statsSummery;
        RefreshStatsLabels();
    }

    [GlobalDependency(RegistrationMode.AsEverything)]
    public class Controller: WindowController<RateCalculatorWindowUi>
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
    }
}