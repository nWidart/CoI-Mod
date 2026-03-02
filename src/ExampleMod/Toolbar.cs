using System;
using System.Collections.Generic;
using System.Linq;
using ExampleMod.UserInterface.RateCalculator;
using Mafi;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Factory.Transports;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
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
    private readonly ProtosDb _protoDb;
    private readonly StatsSummeryService _statsSummeryService;
    private readonly RateCalculatorWindowUi.Controller _rateCalculatorWindowController;
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
        ProtosDb protoDb,
        StatsSummeryService statsSummeryService,
        RateCalculatorWindowUi.Controller rateCalculatorWindowController
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
        _protoDb = protoDb;
        _statsSummeryService = statsSummeryService;
        _rateCalculatorWindowController = rateCalculatorWindowController;

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
        var statsSummery = _statsSummeryService.GenerateFor(selectedEntities);


        var inputDic = new Dict<ProductProto, int>();
        var outputDic = new Dict<ProductProto, int>();

        foreach (var selectedEntity in selectedEntities)
        {
            if (selectedEntity is Machine machine)
            {
                foreach (var recipeProto in machine.RecipesAssigned.AsEnumerable())
                {
                    foreach (var recipeInput in recipeProto.AllInputs.AsEnumerable())
                    {
                        if (inputDic.TryGetValue(recipeInput.Product, out var existing))
                        {
                            inputDic[recipeInput.Product] = existing + recipeInput.Quantity.Value;
                        }
                        else
                        {
                            inputDic.Add(recipeInput.Product, recipeInput.Quantity.Value);
                        }
                    }

                    foreach (var recipeOutput in recipeProto.AllOutputs.AsEnumerable())
                    {
                        if (inputDic.TryGetValue(recipeOutput.Product, out var existing))
                        {
                            outputDic[recipeOutput.Product] = existing + recipeOutput.Quantity.Value;
                        }
                        else
                        {
                            outputDic.Add(recipeOutput.Product, recipeOutput.Quantity.Value);
                        }
                    }
                }
            }
        }

        Log.Info("---- Input Products:");
        inputDic
            .Select((id, quant) => $"Product.Id: {id} | Quantity: {quant}")
            .ToList()
            .ForEach(Log.Info);
        Log.Info("---- Output Products:");
        outputDic
            .Select((id, quant) => $"Product.Id: {id} | Quantity: {quant}")
            .ToList()
            .ForEach(Log.Info);

        _rateCalculatorWindowController.SetStats(statsSummery);
        _rateCalculatorWindowController.SetProducts(inputDic, outputDic);
        _rateCalculatorWindowController.Open();
    }
}