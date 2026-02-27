using Mafi;
using Mafi.Core.Console;
using Mafi.Core.Trains;
using UnityEngine;

namespace ExampleMod;

[GlobalDependency(RegistrationMode.AsSelf)]
public class MyFirstCommand
{
    private readonly TrainsManager _trainsManager;
    private readonly TrainsUIWindow.Controller _trainsUIWindowController;

    public MyFirstCommand(TrainsManager trainsManager, TrainsUIWindow.Controller trainsUIWindowController)
    {
        _trainsManager = trainsManager;
        _trainsUIWindowController = trainsUIWindowController;
    }

    [ConsoleCommand(true, false, null, "stuck_trains_ui")]
    GameCommandResult openTrains()
    {
        _trainsUIWindowController.Open();
        return GameCommandResult.Success("Trains opened.");
    }

    [ConsoleCommand(true, false, null, "stuck_trains")]
    GameCommandResult StuckTrains()
    {
        foreach (var train in _trainsManager.Trains)
        {
            Log.Info(train.ToString());
        }
     
        return GameCommandResult.Success("----");
    }
}