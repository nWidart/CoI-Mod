using Mafi;
using Mafi.Localization;
using Mafi.Unity.InputControl;
using Mafi.Unity.Ui;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library;

namespace ExampleMod;

[GlobalDependency(RegistrationMode.AsEverything)]
public class TrainsUIWindow : Window
{
    public TrainsUIWindow() : base(new LocStrFormatted("Trains Management"))
    {
        this.WindowSize(25.Percent(), 700.px());
        this.MakeMovable();
        this.EnablePinning();
        this.Title(new LocStrFormatted("Trains Management"));
    }

    [GlobalDependency(RegistrationMode.AsEverything)]
    public class Controller : WindowController<TrainsUIWindow>
    {
        public Controller(ControllerContext controllerContext) : base(controllerContext)
        {
            controllerContext.UiRoot.AddDependency(this);
        } 
        
        public void Open() => this.ActivateSelf();
    }
}