using HarmonyLib;
using UnityEngine;
using GameNetcodeStuff;

namespace RunForYourLife
{
    [HarmonyPatch(typeof(PlayerControllerB))]
    internal class PlayerControllerBPatch
    {
        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        private static void OverStamina(ref float ___sprintMeter, ref bool ___isSprinting, ref bool ___isExhausted, ref bool ___isJumping)
        {
            PlayerControllerB localPlayerController = GameNetworkManager.Instance.localPlayerController;

            if (___sprintMeter <= RFYLConfig.damageInterval.Value + 0.1f && (___isSprinting || ___isJumping))
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

        [HarmonyPatch("LateUpdate")]
        [HarmonyPostfix]
        private static void MaxStaminaAndRegen(ref float ___sprintMeter, ref bool ___isSprinting, ref bool ___isWalking, ref float ___num3,
            ref float ___sprintTime, ref float ___carryWeight, ref bool ___isExhausted, ref int ___isMovementHindered)
        {
            PlayerControllerB localPlayerController = GameNetworkManager.Instance.localPlayerController;

            if (___isSprinting)
            {
                ___sprintMeter = Mathf.Clamp(___sprintMeter + Time.deltaTime / ___sprintTime * ___carryWeight * ___num3, 0f, 1f); // Take vanilla value away
                ___sprintMeter = Mathf.Clamp(___sprintMeter - Time.deltaTime / ___sprintTime * ___carryWeight * ___num3, 0f, RFYLConfig.baseStamina.Value); // Use modded value
            }
            else if (___isMovementHindered > 0)
            {
                if (___isWalking)
                {
                    ___sprintMeter = Mathf.Clamp(___sprintMeter + Time.deltaTime / ___sprintTime * ___num3 * 0.5f, 0f, 1f);
                    ___sprintMeter = Mathf.Clamp(___sprintMeter - Time.deltaTime / ___sprintTime * ___num3 * 0.5f, 0f, RFYLConfig.baseStamina.Value);
                }
            }
            else
            {
                if (!___isWalking)
                {
                    ___sprintMeter = Mathf.Clamp(___sprintMeter - Time.deltaTime / (___sprintTime + 4f) * ___num3, 0f, 1f);
                    ___sprintMeter = Mathf.Clamp(___sprintMeter + Time.deltaTime / (___sprintTime + 4f) * ___num3, 0f, RFYLConfig.baseStamina.Value);
                }
                else
                {
                    ___sprintMeter = Mathf.Clamp(___sprintMeter - Time.deltaTime / (___sprintTime + 9f) * ___num3, 0f, 1f);
                    ___sprintMeter = Mathf.Clamp(___sprintMeter + Time.deltaTime / (___sprintTime + 9f) * ___num3, 0f, RFYLConfig.baseStamina.Value);
                }
                if (___isExhausted && ___sprintMeter > 0.2f * RFYLConfig.baseStamina.Value)
                {
                    ___isExhausted = false;
                }
            }
            localPlayerController.sprintMeterUI.fillAmount = ___sprintMeter / RFYLConfig.baseStamina.Value;
        }

        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        private static void SetMaxStamina(ref float ___sprintMeter)
        {
            ___sprintMeter = RFYLConfig.baseStamina.Value;
        }

    }

}