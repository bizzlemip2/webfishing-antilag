
using GDWeave;
using GDWeave.Modding;
namespace GDWeave.AntiLagWebFishing;


public class Mod : IMod
{
    public Mod(IModInterface modInterface) {
        modInterface.Logger.Information("Anti-Lag Started!");
        modInterface.RegisterScriptMod(new PlayerPatcher(modInterface));
        //modInterface.RegisterScriptMod(new ActorPatcher(modInterface));

    }
    public void Dispose()
    {
        // Cleanup anything you do here
    }
}
