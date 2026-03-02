using System;
using ExampleMod.Extensions;
using ExampleMod.UserInterface.Framework;
using Mafi;
using Mafi.Collections;
using Mafi.Core;
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
    private Dict<ProductProto, Fix32> inputDictionary;
    private Dict<ProductProto, Fix32> outputDictionary;

    private void SetStats(StatsSummery statsSummery)
    {
        _statsSummery = statsSummery;
    }

    private void SetProducts(Dict<ProductProto, Fix32> inputDictionary, Dict<ProductProto, Fix32> outputDictionary)
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

        var ingredientsPanel = GetIngredientsPanel();
        var productsPanel = GetProductsPanel();
        var intermediatesPanel = UiFramework.StartNewPanel(new[] { UiFramework.StartNewSection("Intermediates".AsLoc()) });

        Body.Add(statsPanel, ingredientsPanel, productsPanel, intermediatesPanel);
    }
    
    private Panel GetProductsPanel()
    {
        var host = this;
        var productWrapperRow = UiFramework.StartNewEmptyRow();
        var productsSection = UiFramework.StartNewSection("Products".AsLoc());
        productsSection.Add(productWrapperRow);
        
        var productsPanel = UiFramework.StartNewPanel(new[] { productsSection });
        
        this
            .Observe((Func<Dict<ProductProto, Fix32>>)(() => host.outputDictionary))
            .Do(dict =>
            {
                productWrapperRow.Clear();
                var row = new Row();
                foreach (var (productProto, quantity) in dict)
                {
                    //var productQuantityUi = new ProductQuantityUi();
                    //productQuantityUi.Value(productProto)
                    //new ProductQuantity(productProto, quantity);
                    //productQuantityUi.Values("Assets/Unity/UserInterface/General/Fertility.svg", quantity);
                    
                    var icon = new Icon(productProto)
                        .Size(ProductQuantityUi.ICON_HEIGHT)
                        .MarginRight(10.px());
                    row.Add(icon);
                    row.Add(new Label($"{quantity}".ToDoLoc()));
                }
                productWrapperRow.Add(row);
            });

        return productsPanel;
    }
    
    private Panel GetIngredientsPanel()
    {
        var host = this;
        var wrapperRow = UiFramework.StartNewEmptyRow();
        var productsSection = UiFramework.StartNewSection("Ingredients".AsLoc());
        productsSection.Add(wrapperRow);
        var productsPanel = UiFramework.StartNewPanel(new[] { productsSection });
        
        this
            .Observe((Func<Dict<ProductProto, Fix32>>)(() => host.inputDictionary))
            .Do(dict =>
            {
                wrapperRow.Clear();
                
                var row = new Row();
                foreach (var (productProto, quantity) in dict)
                {
                    var icon = new Icon(productProto)
                        .Size(ProductQuantityUi.ICON_HEIGHT)
                        .MarginRight(10.px());
                    row.Add(icon);
                    row.Add(new Label($"{quantity}".ToDoLoc()));
                }
                wrapperRow.Add(row);
            });

        return productsPanel;
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

        public void SetProducts(Dict<ProductProto, Fix32> inputDic, Dict<ProductProto, Fix32> outputDic)
        {
            Window.SetProducts(inputDic, outputDic);
        }
    }
}