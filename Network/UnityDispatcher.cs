using KCPCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

/// <summary>
/// Dispatches thread events to the main thread.
/// </summary>
/// <remarks>
/// Any calls to a dispatcher will ALWAYS delay action by one frame.
/// </remarks>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1050:Declare types in namespaces", Justification = "For easier distribution.")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "To seal the one above")]
public sealed class UnityDispatcher : MonoBehaviour
{
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                              Static Properties
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public static Action<Exception> ExceptionLogger
    {
        get => m_ExceptionLogger;
        set => m_ExceptionLogger = value ?? DefaultLogger;
    }





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                              Static Properties
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    private static Action<Exception> m_ExceptionLogger = DefaultLogger;
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

    /// <summary>
    /// Delays action by one frame.
    /// </summary>
    /// <param name="action"></param>
    public static void Dispatch(Action action)
    {
        lock (queue)
        {
            //Transporter.Logger(new StackTrace().ToStringSafe());
            queue.Enqueue(action);
        }
    }

    private static void DefaultLogger(Exception exception)
    {
        Console.WriteLine($"Error in UnityDispatcher: {exception.Message}\n{exception.StackTrace}");
    }





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                              Implementations
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public void Update()
    {
        if (queue.Count == 0) return;

        Action[] callbacks;
        lock (queue)
        {
            if (queue.Count == 0) return;
            callbacks = [.. queue];
            queue.Clear();
        }

        for (int i = 0; i < callbacks.Length; i++)
        {
            try
            {
                callbacks[i]?.Invoke();
            }
            catch (Exception ex)
            {
                ExceptionLogger.Invoke(ex);
            }
        }
    }
}