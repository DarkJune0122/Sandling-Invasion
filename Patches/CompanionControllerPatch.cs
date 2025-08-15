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
            Pipes.ETG.InvasionModeChanged += (flag) => __instance.CanBePet = Pipes.ETG.WhetherPettingAllowed;
            Pipes.ETG.PettingAllowedChanged += (flag) => __instance.CanBePet = Pipes.ETG.WhetherPettingAllowed;
            __instance.CanBePet = Pipes.ETG.WhetherPettingAllowed;
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
