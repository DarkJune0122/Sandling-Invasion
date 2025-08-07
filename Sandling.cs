using Alexandria.CharacterAPI;
using Alexandria.ItemAPI;
using Alexandria.TranslationAPI;
using Gungeon;
using MonoMod.RuntimeDetour;
using System.Collections;
using System.IO;
using System.Linq;
using System.Runtime;
using UnityEngine;
using static SpawnEnemyOnDeath;

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
    public static void Register(GameManager manager)
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
            definition.texelSize = new Vector2(20, 20);
            definition.boundsDataCenter = new Vector3(10f / 16f, 10f / 16f, 0f); // Half of width/height
            definition.boundsDataExtents = new Vector3(10f / 16f, 10f / 16f, 0f); // Half of width/height
            definition.untrimmedBoundsDataCenter = new Vector3(10f / 16f, 10f / 16f, 0f);
            definition.untrimmedBoundsDataExtents = new Vector3(10f / 16f, 10f / 16f, 0f);

            definition.position0 = new Vector3(0f, 0f, 0f);
            definition.position1 = new Vector3(1.25f, 0f, 0f);
            definition.position2 = new Vector3(0f, 1.25f, 0f);
            definition.position3 = new Vector3(1.25f, 1.25f, 0f);
        }
        else Plugin.Warning("Cannot replace dog item!");

        // Overwriting dog's entity sprite preview.
        //if (dogItem is not CompanionItem companion)
        //{
        //    Plugin.Warning("What a dog going?");
        //    return;
        //}

        //AIActor dog = EnemyDatabase.GetOrLoadByGuid(companion.CompanionGuid);
        //if (dog != null && companion.OverridePlayerOrbitalItem)
        //{
        //    Texture2D texture = ResourceExtractor.GetTextureFromResource(ItemSpritePath);
        //    ETGMod.ReplaceTexture(definition, texture);

        //    Plugin.ResetCount();
        //    Plugin.Count(dog.sprite.Collection.spriteDefinitions.ttWhere((d) => d.name.Contains("dog")), (v) => v.name);
        //    Plugin.Log(dog.sprite.Collection.FirstValidDefinition);
        //    Plugin.Log("Definition sprite was replaced.");
        //}
        //else Plugin.Warning("Cannot locate a dog itself! Sniff it out!");

        SetupSpawn(manager);
    }





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                               Spawn Behavior
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    static bool spawned = false;
    protected static void SetupSpawn(GameManager manager)
    {
        PickupObject item = Game.Items.Get("gungeon:dog");
        manager.OnNewLevelFullyLoaded += () =>
        {
            GameManager manager = GameManager.Instance;
            foreach (var room in manager.Dungeon.data.rooms)
            {
                room.OnEnemiesCleared += () =>
                {
                    if (spawned) return;
                    spawned = true;

                    // Always spawns for now.
                    for (int i = 0; i < 16; i++)
                    {
                        LootEngine.SpawnItem(item.gameObject, room.area.UnitCenter, Vector2.up, 0f);
                    }
                };
            }
        };
    }

    private static void SpawnItem(Vector2 position, PickupObject item)
    {
        var player = GameManager.Instance.PrimaryPlayer;
        Vector2 spawnPosition = player.CenterPosition;
        spawnPosition.x += 3.2f;
        spawnPosition.y -= 3.2f;

        LootEngine.SpawnItem(item.gameObject, spawnPosition, Vector2.up, 0f);
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