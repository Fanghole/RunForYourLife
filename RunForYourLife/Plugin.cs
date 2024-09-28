using BepInEx;
using HarmonyLib;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using BepInEx.Logging;
using UnityEngine;

[assembly: CompilationRelaxations(8)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: Debuggable(DebuggableAttribute.DebuggingModes.Default | DebuggableAttribute.DebuggingModes.DisableOptimizations | DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints | DebuggableAttribute.DebuggingModes.EnableEditAndContinue)]
namespace RunForYourLife
{

    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private const string PLUGIN_GUID = "RunForYourLife";

        private const string PLUGIN_NAME = "Run For Your Life";

        private const string PLUGIN_VERSION = "0.1";

        private readonly Harmony harmony = new Harmony("RunForYourLife");

        internal static ManualLogSource mls;
        public static BaseUnityPlugin Instance { get; private set; }

        private void Awake()
        {

            if ((Object)(object)Instance == (Object)null)
            {
                Instance = (BaseUnityPlugin)(object)this;
            }

            mls = BepInEx.Logging.Logger.CreateLogSource(PLUGIN_GUID);

            mls.LogInfo("Loaded Run For Your Life");

            RFYLConfig.Setup();

            harmony.PatchAll(typeof(PlayerControllerBPatch));
        }
    }
}