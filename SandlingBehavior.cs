using System;
using System.Collections;
using UnityEngine;

namespace SandlingInvasion;

[Obsolete("Not in use yet", true)]
public sealed class SandlingBehavior : BehaviorBase
{
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                              Public Properties
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public float ChanceToFindItemOnRoomClear
    {
        get
        {
            if (ETGPipe.InvasionMode) return InvasionChanceToFindItemOnRoomClear;
            else return RegularChanceToFindItemOnRoomClear;
        }
    }





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                                Public Fields
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public GenericLootTable ItemFindLootTable;
    public string ItemFindAnimationName;
    public float RegularChanceToFindItemOnRoomClear = 0.05f;
    public float InvasionChanceToFindItemOnRoomClear = 0.01f;





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                               Private Fields
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    private float m_DigTimer = 0f;





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                               Public Methods
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public override void Start()
    {
        base.Start();
        if (m_aiActor.CompanionOwner != null)
        {
            m_aiActor.CompanionOwner.OnRoomClearEvent += HandleRoomCleared;
        }
    }

    public override void Destroy()
    {
        if (m_aiActor.CompanionOwner != null)
        {
            m_aiActor.CompanionOwner.OnRoomClearEvent -= HandleRoomCleared;
        }
        base.Destroy();
    }

    private IEnumerator DelayedSpawnItem(Vector2 spawnPoint)
    {
        yield return new WaitForSeconds(0.5f);
        LootEngine.SpawnItem(ItemFindLootTable.SelectByWeight(), spawnPoint, Vector2.up, 1f);
    }

    private void HandleRoomCleared(PlayerController obj)
    {
        if (UnityEngine.Random.value < ChanceToFindItemOnRoomClear)
        {
            m_DigTimer = 4.5f;
            if (!string.IsNullOrEmpty(ItemFindAnimationName))
            {
                m_aiAnimator.PlayUntilFinished(ItemFindAnimationName);
            }
            GameManager.Instance.Dungeon.StartCoroutine(DelayedSpawnItem(m_aiActor.CenterPosition));
        }
    }

    public override void Upkeep()
    {
        base.Upkeep();
    }

    public override BehaviorResult Update()
    {
        if (m_DigTimer > 0f)
        {
            DecrementTimer(ref m_DigTimer);
            m_aiActor.ClearPath();
        }
        return base.Update();
    }
}
