using Mafi;
using Mafi.Core.Console;
using Mafi.Core.Trains;
using UnityEngine;

namespace ExampleMod;

[GlobalDependency(RegistrationMode.AsSelf)]
public class MyFirstCommand
{
    private readonly TrainLinesManager _trainLinesManager;

    public MyFirstCommand(TrainLinesManager trainLinesManager)
    {
        _trainLinesManager = trainLinesManager;
    }

    [ConsoleCommand(true, false, null, "hello")]
    GameCommandResult helloWorld()
    {
        for (var i = 0; i < _trainLinesManager.Lines.Count; i++)
        {
            var trainLineMembers = _trainLinesManager.Lines[i].Trains;
            
            Log.Info(_trainLinesManager.Lines[i].ToString());
            
            Log.Info(trainLineMembers.ToString());
        }

        return GameCommandResult.Success("Hello World!");
    }
}