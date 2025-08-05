using Alexandria.ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SandlingInvasion;

public class Sandling : PassiveItem
{
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                               Static Fields
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public const string ItemName = "Sandling";
    public const string ShortDescription = "A <i>friendly</i> sandling that follows you around.";
    public const string LongDescription = "A <i>friendly</i> sandling that follows you around. ------------------";





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                               Static Methods
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public static void Register()
    {
        // Creates the item.
        const string ItemSpritePath = $"{nameof(SandlingInvasion)}/Resources/sandling-item.png";
        GameObject obj = new(ItemName);
        var item = obj.AddComponent<Sandling>();

        ItemBuilder.AddSpriteToObject(ItemName, ItemSpritePath, obj);

        // Registers the items.
        ItemBuilder.SetupItem(item, ShortDescription, LongDescription, Plugin.API);
        item.encounterTrackable.prerequisites = [];

        // Testing:
        ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Health, 1, StatModifier.ModifyMethod.ADDITIVE);
        ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Coolness, 1);
        item.quality = ItemQuality.C;

        // Replaces a built-in Dog item in character's inventory.
        //AIActor actor = EnemyDatabase.GetOrLoadByGuid("c80f57241dcf4fc3a4c33998141f4394");
        //Plugin.Log(actor == null ? "Actor not found." : $"Actor {actor.ActorName} found.");
    }





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                               Public Methods
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public override void Pickup(PlayerController player)
    {
        base.Pickup(player);
        Plugin.Log($"Player picked up {DisplayName}");
    }

    public override void DisableEffect(PlayerController player)
    {
        // TODO: Add full-screen no-respect warning.
        Plugin.Log($"Player dropped or got rid of {DisplayName}! NO RESPECT!");
    }
}