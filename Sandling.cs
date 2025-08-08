using Alexandria.DungeonAPI;
using Alexandria.ItemAPI;
using Dungeonator;
using Gungeon;
using System;
using System.Linq;
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
        PickupObject dogItem = Game.Items.Get("gungeon:dog");
        dogItem.SetName(ItemName);
        dogItem.SetShortDescription(ShortDescription);
        dogItem.SetLongDescription(LongDescription);
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
        tk2dSpriteDefinition definition = dogItem.sprite.GetCurrentSpriteDef();
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
    private static int currentOdds = 0; // Note: 0 - is a valid odd. Odds go not from [1 - 10] but [0 - 9]


    protected static void SetupSpawn()
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
                        int sandlingAmount = 0;
                        foreach (var player in GameManager.Instance.AllPlayers)
                        {
                            sandlingAmount += player.passiveItems.Count(item => item.itemName == sandling.itemName);
                        }

                        //Plugin.Log(GameManager.Instance.AllPlayers, (item) => item.ActorName);
                        //int reduction = GameManager.Instance.AllPlayers.Count(player => player.ActorName == "guide");
                        //Plugin.Log("Reduction: " + reduction);

                        int spawnOdds = GetSpawnOdds(stage: sandlingAmount);

                        // Keep in mind - int range is exclusive! Also 0/9 odd is essentially 1/10.
                        if (currentOdds >= UnityEngine.Random.Range(0, spawnOdds) && TryGetLandingLocation(room, out Vector2 position))
                        {
                            Plugin.Log($"Spawning item (sorry if it is inside the wall :D)");
                            LootEngine.SpawnItem(sandling.gameObject, position, Vector2.up, 0f);

                            // Odd reset.
                            currentOdds = 0;
                        }
                        else currentOdds++; // Gives higher odds next roll.
                    };
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

    protected static int GetSpawnOdds(int stage)
    {
        if (stage < 0)
        {
            return DefaultOdds[0];
        }
        else if (stage < DefaultOdds.Length)
        {
            return DefaultOdds[stage];
        }
        else if (DefaultOdds.Length >= 2)
        {
            int oddBase = DefaultOdds[DefaultOdds.Length - 1];
            int oddDelta = (oddBase - DefaultOdds[DefaultOdds.Length - 2]);
            return oddBase + oddDelta * (stage - DefaultOdds.Length + 1);
        }
        else
        {
            return DefaultOdds[DefaultOdds.Length - 1] * (stage - DefaultOdds.Length + 1);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="room"></param>
    /// <param name="position"></param>
    /// <returns>Whether position was found.</returns>
    protected static bool TryGetLandingLocation(RoomHandler room, out Vector2 position)
    {
        position = room.area.Center;
        return true; // Temporary solution, because otherwise everything is VERY buggy.

        //if (room.Cells == null || room.Cells.Count == 0)
        //{
        //    position = default;
        //    return false;
        //}

        //try
        //{
        //    DungeonData dungeon = GameManager.Instance.Dungeon.data;

        //    // Defines min-max positions of the room.
        //    int xMin = int.MaxValue, yMin = int.MaxValue, xMax = int.MinValue, yMax = int.MinValue;
        //    foreach (var cell in room.Cells)
        //    {
        //        xMin = Mathf.Min(xMin, cell.x);
        //        xMax = Mathf.Min(xMax, cell.x);
        //        yMin = Mathf.Min(yMin, cell.y);
        //        yMax = Mathf.Min(yMax, cell.y);
        //    }

        //    float xCenter = (float)xMax - xMin;
        //    float yCenter = (float)yMax - yMin;

        //    // Looks for a valid cell.
        //    float lastDistance = float.PositiveInfinity;
        //    IntVector2 centerCell = default;
        //    foreach (var cell in room.Cells)
        //    {
        //        float xDelta = cell.x - xCenter;
        //        float yDelta = cell.y - yCenter;
        //        float sqrDistance = xDelta * xDelta + yDelta * yDelta;
        //        Plugin.Log(sqrDistance);
        //        if (sqrDistance >= lastDistance)
        //        {
        //            continue;
        //        }

        //        CellData data = dungeon[cell.x, cell.y];
        //        Plugin.Log($"Data: " + data);
        //        Plugin.Log($"Data.type: " + data.type);
        //        if (data != null && data.type == CellType.FLOOR) // Accepts pit-only - any flags will be removed.
        //        {
        //            // Cell is valid. Go for spawning.
        //            lastDistance = sqrDistance;
        //            centerCell = cell;
        //        }
        //    }

        //    Plugin.Log($"Lowest distance recorded: {lastDistance}");
        //    if (lastDistance > 1_000_000f) // Likely bugged out.
        //    {
        //        position = room.area.Center;
        //        return true;
        //    }
        //    else
        //    {
        //        position = centerCell.ToVector2() + new Vector2(0.5f, 0.5f);
        //        return true;
        //    }
        //}
        //catch (Exception e)
        //{
        //    Plugin.Warning($"Cannot locate a room center without issues!");
        //    Plugin.Warning(e.Message);
        //    Plugin.Warning(e.StackTrace);
        //}

        //position = default;
        //return false;
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