using Mafi;
using Mafi.Base;
using Mafi.Core;
using Mafi.Core.Mods;

namespace ExampleMod;

public sealed class ExampleMod : DataOnlyMod {
	public override string Name => "Example mod";

	public override int Version => 1;
	
	public ExampleMod(CoreMod coreMod, BaseMod baseMod) {
		// You can use Log class for logging. These will be written to the log file
		// and can be also displayed in the in-game console with command `also_log_to_console`.
		Log.Info("ExampleMod: constructed");
	}


	public override void RegisterPrototypes(ProtoRegistrator registrator) {
		Log.Info("ExampleMod: registering prototypes");
		// Register all prototypes here.
	}

}