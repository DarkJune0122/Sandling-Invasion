using Alexandria.CharacterAPI;
using Alexandria.ItemAPI;
using Alexandria.TranslationAPI;
using Gungeon;
using UnityEngine;

namespace SandlingInvasion;

public class Sandling : CompanionItem
{
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                               Static Fields
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public const string ItemName = "Sandling";
    public const string ShortDescription = "Simply Sandling";
    public const string LongDescription =
        "A curious creature, that came from a realm of endless sands.\n\n" +
        "How is he ended-up here? Where is he going? What is he looking for?\n\n" +
        "A mysterious destination that one can dare to find, remains secret until reached.";





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                              Static Properties
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    // public static Sandling Item { get; protected set; }





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                               Static Methods
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public static void Register()
    {
        // Creates the item.
        const string ItemSpritePath = $"{nameof(SandlingInvasion)}/Resources/dog_item_001.png";
        //GameObject obj = new(ItemName);
        //var item = obj.AddComponent<Sandling>();

        //ItemBuilder.AddSpriteToObject(ItemName, ItemSpritePath, obj);

        //// Registers the items.
        //ItemBuilder.SetupItem(item, ShortDescription, LongDescription, Plugin.API);
        //item.encounterTrackable.prerequisites = [];

        //// Testing:
        //ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Health, 1, StatModifier.ModifyMethod.ADDITIVE);
        //ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Coolness, 1);
        //item.quality = ItemQuality.C;

        //// Replaces a built-in Dog item in character's inventory.
        PickupObject dogItem = Game.Items.Get("gungeon:dog");
        dogItem.SetName(ItemName);
        dogItem.SetShortDescription(ShortDescription);
        dogItem.SetLongDescription(LongDescription);

        //Plugin.Log(Game.Enemies.Pairs);

        //Plugin.Log(dogItem.sprite.Collection.FirstValidDefinition);
        //Plugin.Log();

        // Overwriting dog's item sprite definition.
        if (Utils.TryFind(dogItem.sprite.Collection.spriteDefinitions, (d) => d.name == "dog_item_001", out var definition))
        {
            Texture2D texture = ResourceExtractor.GetTextureFromResource(ItemSpritePath);
            ETGMod.ReplaceTexture(definition, texture);
            Plugin.Log("Definition was replaced.");
        }
        else Plugin.Warning("Cannot replace dog item!");

        // Overwriting dog's entity sprite preview.
    }





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                               Public Methods
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    //public override void Pickup(PlayerController player)
    //{
    //    base.Pickup(player);
    //    Plugin.Log($"Player picked up {DisplayName}");
    //}

    //public override void DisableEffect(PlayerController player)
    //{
    //    // TODO: Add full-screen no-respect warning.
    //    Plugin.Log($"Player dropped or got rid of {DisplayName}! NO RESPECT!");
    //}

    //public override void Update()
    //{
    //    base.Update();
    //    Plugin.Log("Update");
    //    Plugin.Log(FindObjectsOfType<PlayerController>());
    //}
}