using UnityEngine;

namespace SandlingInvasion.Patches;

public static class CompanionControllerPatch
{
    [HarmonyLib.HarmonyPatch(typeof(CompanionController), nameof(CompanionController.Initialize))]
    public static void Postfix(CompanionController instance, PlayerController owner)
    {
        owner = instance.m_owner;
        if (owner != null)
        {
            Plugin.Log($"Companion ('{instance.aiActor.ActorName}') spawned for player: {owner.name}");

            // Prevents petting, if requested.
            Pipes.ETG.InvasionModeChanged += (flag) => instance.CanBePet = Pipes.ETG.WhetherPettingAllowed;
            Pipes.ETG.PettingAllowedChanged += (flag) => instance.CanBePet = Pipes.ETG.WhetherPettingAllowed;
            instance.CanBePet = Pipes.ETG.WhetherPettingAllowed;
        }

        // Also replaces regular item find behaviour with custom one, if it was added:
        //DogItemFindBehavior behavior = instance.GetComponent<DogItemFindBehavior>();
        //if (behavior != null)
        //{
        //    Plugin.Log($"Behavior found!");
        //}
        //else Plugin.Log($"Behavior was NOT found.");
    }
}
