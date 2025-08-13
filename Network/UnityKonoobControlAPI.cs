using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Threading;

/// <summary>
/// Gives functions to work with Konoob Control Panel messages in Enter the Gungeon.
/// </summary>
/// <![CDATA[v0.0.1]]>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1050:Declare types in namespaces", Justification = "For easier distribution.")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "To seal the one above")]
public static partial class UnityKonoobControlAPI
{
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                              Static Properties
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    /// <summary>
    /// Invoked when KonoobControl reports any Twitch event or any other message.
    /// </summary>
    public static event Action<string> OnMessageReceived;

    /// <summary>
    /// Default Logger to be used for built-in reports.
    /// </summary>
    public static Action<object> Logger
    {
        get => m_Logger; set => m_Logger = value ?? Console.WriteLine;
    }





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                               Static Fields
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    private static readonly Dictionary<string, Action> callbacks = [];
    private static Action<object> m_Logger = Console.WriteLine;
    private static bool isInitialized = false;





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                               Static Methods
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    /// <summary>
    /// Initializes <see cref="UnityKonoobControlAPI"/> and <see cref="UnityDispatcher"/>.
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public static void Initialize()
    {
        if (isInitialized) return;
        isInitialized = true;
        UnityDispatcher.Initialize();
        Worker.Initialize();
        Listen(NetworkPipes.Messages.TestMessage, () => Logger($"{Worker.LogPrefix} Test message received!"));
    }

    public static void Listen(string key, Action action)
    {
        if (callbacks.TryGetValue(key, out Action callback))
        {
            callback += action;
            callbacks[key] = callback;
        }
        else callbacks[key] = action;
    }

    public static void Unlisten(string key, Action action)
    {
        if (callbacks.TryGetValue(key, out Action callback))
        {
            callback -= action;
            if (callback == null)
            {
                callbacks.Remove(key);
            }
            else callbacks[key] = callback;
        }
    }





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                               Network Worker
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    /// <summary>
    /// Worker, that communicates with specific pipe, provided by the server (server-reading only).
    /// </summary>
    private static class Worker
    {
        /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
        /// .
        /// .                                                 Constants
        /// .
        /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
        public const string LogPrefix = "[KCP]"; // Short form of "Konoob Control Panel".





        /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
        /// .
        /// .                                               Static Fields
        /// .
        /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
        private static Thread thread;





        /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
        /// .
        /// .                                              Static Properties
        /// .
        /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
        public static void Initialize()
        {
            if (thread != null) return;
            thread = new Thread(ReadMessages)
            {
                IsBackground = true
            };

            thread.Start();
        }

        private static void ReadMessages()
        {
            while (true)
            {
                try
                {
                    using var pipe = new NamedPipeClientStream(
                        NetworkPipes.ServerName, NetworkPipes.EnterTheGungeonPipe,
                        PipeDirection.In);

                    pipe.Connect();

                    using var reader = new StreamReader(pipe, NetworkPipes.Encoding);
                    string message;
                    while ((message = reader.ReadLine()) != null)
                    {
                        UnityDispatcher.Dispatch(() =>
                        {
                            OnMessageReceived?.Invoke(message);
                            if (callbacks.TryGetValue(message, out var callback))
                                callback?.Invoke();
                        });
                    }
                }
                catch (IOException)
                {
                    Logger($"{LogPrefix} Connection with Konoob Control Panel lost.");
                }
                catch
                {
                    // Ignores "Operation failed successfully" message XD.
                }

                // Back-off before reconnect
                Thread.Sleep(NetworkPipes.HeartbeatMs);
            }
        }
    }
}