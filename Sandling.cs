using Alexandria.CharacterAPI;
using Alexandria.DungeonAPI;
using Alexandria.EnemyAPI;
using Alexandria.ItemAPI;
using Dungeonator;
using Gungeon;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SandlingInvasion;

public static class Sandling
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

    public const string InternalItemName = "sandling_compass_001";
    public const string InternalFullItemName = InternalItemName + ".png";





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

        //GameManager.Instance.OnNewLevelFullyLoaded += () =>
        //{
        //    Vector2 position = GameManager.Instance.PrimaryPlayer.CenterPosition;
        //    LootEngine.SpawnItem(item.gameObject, position, Vector2.up, 0f);
        //};

        // Replaces a built-in Dog item in character's inventory.
        CompanionItem item = Game.Items.Get("gungeon:dog") as CompanionItem;
        if (item != null)
        {
            item.SetName(ItemName);
            item.SetShortDescription(ShortDescription);
            item.SetLongDescription(LongDescription);
        }

        //Plugin.Log();
        //Plugin.LogFields(item.sprite.GetCurrentSpriteDef());
        //Plugin.Log(item.sprite.GetCurrentSpriteDef().uvs);
        //Plugin.Log();
        //Plugin.LogFields(dogItem.sprite.GetCurrentSpriteDef());
        //Plugin.Log(dogItem.sprite.GetCurrentSpriteDef().uvs);
        //tk2dSpriteDefinition definition = dogItem.sprite.GetCurrentSpriteDef();
        //definition.texelSize = new Vector2(0.2f, 0.2f);

        //int spriteID = SpriteBuilder.AddSpriteToCollection(ItemSpritePath, SpriteBuilder.itemCollection);
        //if (dogItem.sprite is tk2dSprite sprite)
        //{
        //    sprite.SetSprite(spriteID);
        //    sprite.SortingOrder = 0;
        //    sprite.IsPerpendicular = true;
        //}
        //else Plugin.Warning("Dog item has unexpected sprite type.");

        //Plugin.Log(Game.Enemies.Pairs);

        //Plugin.Log(dogItem.sprite.Collection.FirstValidDefinition);
        //Plugin.Log();

        // Overwriting dog's item sprite definition.
        Texture2D texture = ResourceExtractor.GetTextureFromResource(ItemSpritePath);
        tk2dSpriteDefinition definition = item.sprite.GetCurrentSpriteDef();
        ETGMod.ReplaceTexture(definition, texture);
        definition.texelSize = new Vector2(17, 17);
        float halfSize = 8.5f / 16f; // Half of 17 in game units (pixels per unit: 16)

        definition.boundsDataCenter = new Vector3(halfSize, halfSize, 0f);
        definition.boundsDataExtents = new Vector3(halfSize, halfSize, 0f);
        definition.untrimmedBoundsDataCenter = new Vector3(halfSize, halfSize, 0f);
        definition.untrimmedBoundsDataExtents = new Vector3(halfSize, halfSize, 0f);

        // Adjust positions based on 17px width/height in world units
        float worldWidth = 17f / 16f;  // 1.0625
        definition.position0 = new Vector3(0f, 0f, 0f);
        definition.position1 = new Vector3(worldWidth, 0f, 0f);
        definition.position2 = new Vector3(0f, worldWidth, 0f);
        definition.position3 = new Vector3(worldWidth, worldWidth, 0f);

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

        SetupSpawn();
    }





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                               Spawn Behavior
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    private static readonly int[] DefaultOdds = [10, 40, 100, 240, 480];
    private static readonly int[] InvasionOdds = [10, 50, 100, 200, 300];
    private static int currentOdds = 0; // Note: 0 - is a valid odd. Odds go not from [1 - 10] but [0 - 9]

    /// <summary>
    /// Last killed enemy, if any.
    /// </summary>
    private static readonly List<AIActor> trackers = [];


    private static void SetupSpawn()
    {
        PickupObject sandling = Game.Items.Get("gungeon:dog");
        
        DungeonHooks.OnPostDungeonGeneration += () =>
        {
            // Sandlings are professionals. They need no tutorial :D
            if (GameManager.Instance.InTutorial)
            {
                currentOdds = 0;
                Plugin.Log("No Sandlings in a tutorial! We need no tutorial!");
                return;
            }

            try
            {
                foreach (var room in GameManager.Instance.Dungeon.data.rooms)
                {
                    room.OnEnemiesCleared += () =>
                    {
                        // Fired before enemy kill events, surprisingly.
                        currentOdds++;
                    };

                    room.Entered += (player) =>
                    {
                        if (room.GetActiveEnemiesCount(RoomHandler.ActiveEnemyType.All) != 0)
                        {
                            foreach (var enemy in room.GetActiveEnemies(RoomHandler.ActiveEnemyType.All))
                            {
                                TrackEnemy(enemy);
                            }
                        }
                    };

                    room.OnEnemyRegistered += TrackEnemy;

                    // Simplifications:
                    void TrackEnemy(AIActor enemy)
                    {
                        if (enemy != null && enemy.healthHaver != null)
                        {
                            trackers.Add(enemy);
                            enemy.healthHaver.OnPreDeath += (position) =>
                            {
                                // Removes all 'null's, if there is any.
                                for (int i = trackers.Count - 1; i >= 0; i--)
                                {
                                    if (trackers[i] == null) trackers.RemoveAt(i);
                                }

                                if (trackers.Remove(enemy) && trackers.Count == 0)
                                {
                                    TrySpawnItem(sandling, room, around: enemy.CenterPosition);
                                }
                            };
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Plugin.Warning($"Cannot attach sandling spawn events! Sandlings spawn on this level.");
                Plugin.Warning(e.Message);
                Plugin.Warning(e.StackTrace);
            }
        };
    }

    private static void TrySpawnItem(PickupObject sandling, RoomHandler room, Vector2 around)
    {
        int sandlingAmount = 0;
        foreach (var player in GameManager.Instance.AllPlayers)
        {
            sandlingAmount += player.passiveItems.Count(item => item.itemName == sandling.itemName);
        }

        int spawnOdds = GetSpawnOdds(stage: sandlingAmount);

        // Keep in mind - '0' is included, and 'max' is excluded! Meaning, that 0/9 odd is essentially 1/10.
        if (currentOdds >= UnityEngine.Random.Range(0, spawnOdds))
        {
            var area = room.area;
            Dungeon dungeon = GameManager.Instance.Dungeon;
            IntVector2 source = new(
                Mathf.Clamp(Mathf.RoundToInt(around.x), area.basePosition.x, area.basePosition.x + area.dimensions.x),
                Mathf.Clamp(Mathf.RoundToInt(around.y), area.basePosition.y, area.basePosition.y + area.dimensions.y));
            if (Utils.TryFindClosest(source,
                (pos) => dungeon.data.CheckInBoundsAndValid(source) && dungeon.data[source].type == CellType.FLOOR,
                out IntVector2 position))
            {
                LootEngine.SpawnItem(sandling.gameObject, position.ToVector3(), Vector2.up, 0f);
                currentOdds = 0;
            }
            else
            {
                currentOdds = spawnOdds;
            }
        }
        else currentOdds++; // Gives higher odds next roll.
    }

    private static int GetSpawnOdds(int stage)
    {
        int[] odds = Pipes.ETG.InvasionMode ? InvasionOdds : DefaultOdds;
        if (stage < 0)
        {
            return odds[0];
        }
        else if (stage < odds.Length)
        {
            return odds[stage];
        }
        else if (odds.Length >= 2)
        {
            int oddBase = odds[odds.Length - 1];
            int oddDelta = (oddBase - odds[odds.Length - 2]);
            return oddBase + oddDelta * (stage - odds.Length + 1);
        }
        else
        {
            return odds[odds.Length - 1] * (stage - odds.Length + 1);
        }
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
}