using Mafi.Unity.InputControl;
using UnityEngine;

namespace ExampleMod;

public class ShortcutsMap
{
    public static ShortcutsMap Instance { get; } = new();
    
    [Kb(KbCategory.Tools, "open_train_management_window", "Open train management", "Open train management", true, false)]
    public KeyBindings OpenTrainsWindow { get; set; } = KeyBindings.FromKey(KbCategory.Tools, ShortcutMode.Game, KeyCode.F8);
    
    [Kb(KbCategory.Tools, "openTestWindow", "Test Window", "Test Window", true, false)]
    public KeyBindings OpenRateCalcWindow { get; set; } = KeyBindings.FromKey(KbCategory.Tools, ShortcutMode.Game, KeyCode.F9);
    
    [Kb(KbCategory.Tools, "calc", "calc", "calc", true, false)]
    public KeyBindings OpenCalc { get; set; } = KeyBindings.FromKey(KbCategory.Tools, ShortcutMode.Game, KeyCode.Colon);
}