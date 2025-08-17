using System;
using System.Collections.Generic;
using UnityEngine;

namespace SandlingInvasion.Patches;

public static class CompanionControllerPatch
{
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                                  Patches
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    [HarmonyLib.HarmonyPatch(typeof(CompanionController))]
    [HarmonyLib.HarmonyPatch(nameof(CompanionController.Initialize))]
    public static void Prefix(CompanionController __instance, PlayerController player)
    {
        if (__instance.aiActor.ActorName != "Dog")
        {
            return;
        }

        var owner = __instance.m_owner;
        if (owner != null)
        {
            // Prevents petting, if requested.
            Plugin.Log("Sandling initialized.");
            ETGPipe.InvasionModeChanged += (flag) => __instance.CanBePet = ETGPipe.WhetherPettingAllowed;
            ETGPipe.PettingAllowedChanged += (flag) => __instance.CanBePet = ETGPipe.WhetherPettingAllowed;
            __instance.CanBePet = ETGPipe.WhetherPettingAllowed;
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(CompanionController))]
    [HarmonyLib.HarmonyPatch(nameof(CompanionController.OnDestroy))]
    public static void Prefix(CompanionController __instance)
    {
        if (__instance.aiActor.ActorName != "Dog")
        {
            return;
        }

        var owner = __instance.m_owner;
        if (owner != null)
        {

        }
    }


}
