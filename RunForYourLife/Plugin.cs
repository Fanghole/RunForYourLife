using BepInEx;
using HarmonyLib;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using BepInEx.Logging;
using BepInEx.Configuration;
using UnityEngine;
using Patch;

[assembly: CompilationRelaxations(8)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: Debuggable(DebuggableAttribute.DebuggingModes.Default | DebuggableAttribute.DebuggingModes.DisableOptimizations | DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints | DebuggableAttribute.DebuggingModes.EnableEditAndContinue)]
namespace RunForYourLife
{

    internal class RFYLConfig
    {
        public static ConfigEntry<float> baseStamina;

        public static ConfigEntry<int> damageMagnitude;

        public static ConfigEntry<float> damageInterval;

        public static ConfigEntry<bool> isFatal;

        public static void Setup()
        {
            baseStamina = Plugin.Instance.Config.Bind<float>("Stamina", "BaseStamina", 1f, "How much stamina the player has before taking damage.");
            damageMagnitude = Plugin.Instance.Config.Bind<int>("Damage", "DamageMagnitude", 10, "The amount of damage taken per tick of damage.");
            damageInterval = Plugin.Instance.Config.Bind<float>("Damage", "DamageFrequency", 0.1f, "How much stamina taken to cause one tick of damage.");
            isFatal = Plugin.Instance.Config.Bind<bool>("Damage", "IsFatal", true, "Whether you can die from oversprinting. Otherwise it'll stop when hitting critical health.");
        }
    }

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