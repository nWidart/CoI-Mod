using System;
using ExampleMod.UserInterface.RateCalculator;
using Mafi;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Transports;
using Mafi.Core.Terrain;
using Mafi.Core.UiState;
using Mafi.Localization;
using Mafi.Unity;
using Mafi.Unity.Entities;
using Mafi.Unity.InputControl;
using Mafi.Unity.InputControl.AreaTool;
using Mafi.Unity.InputControl.Factory;
using Mafi.Unity.Terrain;
using Mafi.Unity.Trains;
using Mafi.Unity.Ui;
using Mafi.Unity.Ui.Controllers;
using Mafi.Unity.Ui.Controllers.LayoutEntityPlacing;
using Mafi.Unity.Ui.Controllers.Tools;
using Mafi.Unity.Ui.Controllers.Trains;
using Mafi.Unity.Ui.Hud;
using Mafi.Unity.Ui.Library;
using Mafi.Unity.UiStatic;
using Mafi.Unity.UiStatic.Cursors;
using Mafi.Unity.UiToolkit.Component;

namespace ExampleMod;

[GlobalDependency(RegistrationMode.AsEverything)]
public class Toolbar : BaseEntityCursorInputController<IStaticEntity>
{
    private readonly EntitiesManager _entitiesManager;
    private readonly RateCalculatorWindowUi.Controller _testWindowController;
    private readonly IUnityInputMgr InputManager;
    protected readonly Toolbox Toolbox;

    public Toolbar(
        ToolbarHud toolbar,
        UiContext context,
        CursorPickingManager cursorPickingManager,
        CursorManager cursorManager,
        NewInstanceOf<StaticEntityMassPlacer> entityPlacer,
        AreaSelectionToolFactory areaSelectionToolFactory,
        NewInstanceOf<DockedBuyPricePopup> pricePopup,
        NewInstanceOf<EntityHighlighter> highlighter,
        NewInstanceOf<TransportTrajectoryHighlighter> transportTrajectoryHighlighter,
        NewInstanceOf<TerrainAreaOutlineRenderer> terrainOutlineRenderer,
        EntitiesManager entitiesManager,
        EntitiesCloneConfigHelper configCloneHelper,
        TransportBuildController transportBuildController,
        TrainTrackBuildController trainTrackBuildController,
        TrackDirectionRenderer trackDirectionRenderer,
        HudStateManager hudState,
        RateCalculatorWindowUi.Controller testWindowController
    ) : base(toolbar,
        context,
        cursorPickingManager,
        cursorManager,
        areaSelectionToolFactory,
        terrainOutlineRenderer,
        entitiesManager,
        highlighter,
        transportTrajectoryHighlighter,
        IdsCore.Technology.CopyTool,
        CursorsStyles.Copy,
        "Assets/Unity/UserInterface/Audio/EntitySelect.prefab",
        (FilterToolbox)new FilterToolboxForCutCopy(hudState,
            nameof(Toolbar)))
    {
        _entitiesManager = entitiesManager;
        _testWindowController = testWindowController;

        InputManager = context.InputMgr;
        Toolbox = toolbar.CreateToolbox();
        toolbar.AddToolButton("CalcTool".AsLoc(),
            this,
            "Assets/Unity/UserInterface/Toolbar/Copy.svg",
            1031f,
            manager => ShortcutsMap.Instance.OpenCalc);
        InputManager.RegisterGlobalShortcut(map => ShortcutsMap.Instance.OpenCalc, this);
    }

    public override void Activate()
    {
        this.Toolbox.Show();
        base.Activate();
    }

    protected override bool Matches(IStaticEntity entity, bool isAreaSelection, bool isLeftClick)
    {
        switch (entity)
        {
            case ILayoutEntity layoutEntity:
                if (layoutEntity.Prototype.CloningDisabled)
                    return false;
                break;
            case TransportPillar _:
                return false;
        }

        return true;
    }

    protected override bool OnFirstActivated(IStaticEntity hoveredEntity, Lyst<IStaticEntity> selectedEntities, Lyst<SubTransport> selectedPartialTransports)
    {
        selectedEntities.Add(hoveredEntity);
        return true;
    }

    protected override void OnEntitiesSelected(IIndexable<IStaticEntity> selectedEntities, IIndexable<SubTransport> selectedPartialTransports,
        ImmutableArray<TileSurfaceCopyPasteData> selectedSurfaces, ImmutableArray<TileSurfaceCopyPasteData> selectedDecals, bool isAreaSelection,
        bool isLeftMouse, RectangleTerrainArea2i? area)
    {
        var statsSummery = new StatsSummery();

        foreach (var selectedEntity in selectedEntities)
        {
            switch (selectedEntity)
            {
                case Machine machine:
                    var maintenancePerMonth = machine.Maintenance.Costs.MaintenancePerMonth;
                    var powerRequired = machine.ElectricityConsumer.Value.PowerRequired;
                    statsSummery.IncrementTotalMaintenancePerMonth(maintenancePerMonth);
                    statsSummery.IncrementTotalPowerRequired(powerRequired);
                    statsSummery.IncrementTotalWorkersAssigned();

                    var entity = this._entitiesManager.GetEntity(machine.Id);
                    Log.Info($"Machine: {entity.Value.GetTitle()}, maintenance/month: {maintenancePerMonth}, power required: {powerRequired}");

                    break;
            }
        }

        Log.Info("Total maintenance costs/month: " + statsSummery.TotalMaintenancePerMonth);
        Log.Info("Total power required: " + statsSummery.TotalPowerRequired + " KW");
        Log.Info("Total workers assigned: " + statsSummery.TotalWorkersAssigned);

        _testWindowController.SetStats(statsSummery);
        _testWindowController.Open();
    }
}