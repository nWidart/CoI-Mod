using Mafi.Unity.InputControl;
using UnityEngine;

namespace ExampleMod;

public class ShortcutsMap
{
    public static ShortcutsMap Instance { get; } = new();
    
    [Kb(KbCategory.Tools, "open_train_management_window", "Open train management", "Open train management", true, false)]
    public KeyBindings OpenTrainsWindow { get; set; } = KeyBindings.FromKey(KbCategory.Tools, ShortcutMode.Game, KeyCode.F8);
}