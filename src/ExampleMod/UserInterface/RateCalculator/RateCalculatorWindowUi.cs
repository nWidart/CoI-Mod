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
    
    public RateCalculatorWindowUi() : base(new LocStrFormatted("Rate Calculator"), true)
    {
        WindowSize(1000.px(), 900.px());
        
        _maintenanceLabel = UiFramework.NewLabel("");
        _powerLabel = UiFramework.NewLabel("");
        _workersLabel = UiFramework.NewLabel("");
        
        Body.Add(UiFramework.StartNewSection(new LocStrFormatted("Stats"), new[]
        {
            UiFramework.StartNewRow(new[]
            {
                _maintenanceLabel,
                _powerLabel,
                _workersLabel
            })
        }));
    }

    private void RefreshStatsLabels()
    {
        var maintenance = _statsSummery.TotalMaintenancePerMonth.ToString();
        var power = _statsSummery.TotalPowerRequired.ToString();
        var workers = _statsSummery.TotalWorkersAssigned.ToString();
        
        _maintenanceLabel = UiFramework.NewLabel($"Total maintenance costs/month: {maintenance}");
        _powerLabel = UiFramework.NewLabel($"Total power required: {power} KW");
        _workersLabel = UiFramework.NewLabel($"Total workers assigned: {workers}");
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
                .RegisterGlobalShortcut(_ => ShortcutsMap.Instance.OpenTestWindow, this);
        }
        
        public void Open() => ActivateSelf();

        public void SetStats(StatsSummery statsSummery)
        {
            Window.SetStats(statsSummery);
        }
    }
}