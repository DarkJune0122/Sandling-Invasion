using System;
using System.Collections.Generic;
using UnityEngine;

namespace SandlingInvasion.Network;

public sealed class UnityDispatcher : MonoBehaviour
{
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                              Static Properties
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    private static readonly Queue<Action> queue = new();
    private static UnityDispatcher instance = null;





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                               Static Methods
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public static void Initialize()
    {
        if (!instance)
        {
            GameObject obj = new("Unity Dispatcher");
            DontDestroyOnLoad(obj);
            instance = obj.AddComponent<UnityDispatcher>();
        }
    }

    public static void Dispatch(Action action)
    {
        lock (queue)
        {
            queue.Enqueue(action);
        }
    }





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                              Implementations
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public void Update()
    {
        if (queue.Count == 0) return;
        lock (queue)
        {
            do
            {
                queue.Dequeue()?.Invoke();
            }
            while (queue.Count > 0);
        }
    }
}
