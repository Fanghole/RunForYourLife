using BepInEx;
using HarmonyLib;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using BepInEx.Logging;
using UnityEngine;
using GameNetcodeStuff;
using RunForYourLife;
using Coroner;

namespace Patch
{
    [HarmonyPatch(typeof(PlayerControllerB))]
    internal class PlayerControllerBPatch
    {
        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        private static void overStamina(ref float ___sprintMeter, ref bool ___isSprinting, ref bool ___isExhausted)
        {
            PlayerControllerB localPlayerController = GameNetworkManager.Instance.localPlayerController;

            if (___sprintMeter <= RFYLConfig.damageInterval.Value + 0.1f && ___isSprinting)
            {
                if (!localPlayerController.isPlayerDead && localPlayerController.isPlayerControlled)
                {
                    if (localPlayerController.health <= RFYLConfig.damageMagnitude.Value && RFYLConfig.isFatal.Value)
                    {
                        localPlayerController.KillPlayer(default(Vector3), spawnBody: true, (CauseOfDeath)0, 0); 
                    } 
                    else
                    {
                        localPlayerController.DamagePlayer(RFYLConfig.damageMagnitude.Value, false, true, (CauseOfDeath)0, 0, false, default(Vector3));
                        if (RFYLConfig.isFatal.Value)
                        {
                            localPlayerController.MakeCriticallyInjured(enable: false); // This prevents the limping allowing more sprint. Without this player heals to 20.
                        }
                    }
                }
                ___sprintMeter += RFYLConfig.damageInterval.Value;
                ___isExhausted = true;
            }
        }
    }
}