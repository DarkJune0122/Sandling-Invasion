//using System.Collections;
//using UnityEngine;

//namespace SandlingInvasion.Patches;

//public static class DogItemFindPatches
//{
//    public const float RegularChanceToFindItemOnRoomClear = 0.05f;
//    public const float InvasionChanceToFindItemOnRoomClear = 0.01f;





//    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
//    /// .
//    /// .                                                  Patches
//    /// .
//    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
//    [HarmonyLib.HarmonyPatch(typeof(DogItemFindBehavior), nameof(DogItemFindBehavior.Start))]
//    public static bool PrefixStart(DogItemFindBehavior instance)
//    {


//        return true;
//    }

//    [HarmonyLib.HarmonyPatch(typeof(DogItemFindBehavior), nameof(DogItemFindBehavior.HandleRoomCleared))]
//    public static bool PrefixHandleRoomCleared(DogItemFindBehavior instance, PlayerController controller)
//    {
//        if (Random.value < (Pipes.ETG.InvasionMode ? InvasionChanceToFindItemOnRoomClear : RegularChanceToFindItemOnRoomClear))
//        {
//            instance.m_findTimer = 4.5f;
//            if (!string.IsNullOrEmpty(instance.ItemFindAnimName))
//            {
//                instance.m_aiAnimator.PlayUntilFinished(instance.ItemFindAnimName);
//            }

//            var item = instance.ItemFindLootTable.SelectByWeight();
//            GameManager.Instance.Dungeon.StartCoroutine(DelayedSpawnItem(item, instance.m_aiActor.CenterPosition));
//        }
//        instance.ItemFindLootTable.SelectByWeight();

//        return false; // skips vanilla method.
//    }

//    private static IEnumerator DelayedSpawnItem(GameObject item, Vector2 spawnPoint)
//    {
//        yield return new WaitForSeconds(0.5f);
//        LootEngine.SpawnItem(item, spawnPoint, Vector2.up, 1f);
//    }

//    private static void SpawnItem()
//    {

//    }
//}
