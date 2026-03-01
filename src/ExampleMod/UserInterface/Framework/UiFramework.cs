using System;
using Mafi;
using Mafi.Localization;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library;

namespace ExampleMod.UserInterface.Framework;

public class UiFramework
{
    public static UiComponent StartNewSection(LocStrFormatted title, UiComponent[] children)
    {
        var component = new Column(2.pt())
        {
            c => c.AlignItemsStretch().PaddingBottom(4.pt()),
            new Title(title),
            children
        };
        return component;
    }

    public static UiComponent StartNewRow(UiComponent[] children)
    {
        var component = new Row(2.pt());
        component.Add(c => c.PaddingLeft(4.pt()).AlignItemsStretch());
        component.Add(children);
        return component;
    }

    public static ButtonText NewCheatButtonText(string text)
    {
        return new ButtonText(text.AsLoc()).MinWidth(45.Percent()).MaxWidth(45.Percent());
    }

    public static Label NewLabel(string text)
    {
        return new Label(text.AsLoc());
    }
}