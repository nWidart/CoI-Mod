using Mafi;
using Mafi.Localization;
using Mafi.Unity.Ui;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library;

namespace ExampleMod;

[GlobalDependency(RegistrationMode.AsEverything)]
public class TrainsUIWindow : PanelWithHeader
{
    public TrainsUIWindow(UiContext uiContext)
    {
        this.Title("Hello world!".AsLoc());
    }
}