#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using static NetworkPipes;

/// <summary>
/// Standardized pipe class for clients.
/// </summary>
/// <![CDATA[v0.0.1]]>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1050:Declare types in namespaces", Justification = "For easier distribution.")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "To seal the one above")]
public abstract class ClientPipe
{
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                              Static Properties
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public static ClientPipe? Instance { get => m_Instance; private set => m_Instance = value; }

    /// <summary>
    /// Default Logger to be used for built-in reports.
    /// </summary>
    /// <remarks>
    /// Multithread-compatible. Called via dispatcher.
    /// </remarks>
    public static Action<object> Logger
    {
        get => m_Logger;
        set
        {
            lock (m_Logger)
            {
                m_Logger = value ?? Console.WriteLine;
            }
        }
    }


    /// <summary>
    /// Default Logger to be used for error visibility.
    /// </summary>
    /// <remarks>
    /// Multithread-compatible. Called via dispatcher.
    /// </remarks>
    public static Action<object> ExceptionLogger
    {
        get => m_ExceptionLogger;
        set
        {
            lock (m_ExceptionLogger)
            {
                m_ExceptionLogger = value ?? Console.WriteLine;
            }
        }
    }





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                                   Events
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    /// <summary>
    /// Callback for one-way message - <see cref="MessageType.Normal"/>.
    /// </summary>
    /// <param name="content">Content of the message. string.Empty when no content.</param>
    public delegate void MessageCallbackEvent(string content);

    /// <summary>
    /// Callback for two-way message - <see cref="MessageType.Direct"/>.
    /// </summary>
    /// <param name="response">Response ID that can be used to send data back to the server.</param>
    /// <param name="content">Content of the message. string.Empty when no content.</param>
    public delegate void MessageDirectEvent(ResponseID response, string content);

    /// <summary>
    /// Callback from previously sent direct message - <see cref="MessageType.Response"/>.
    /// </summary>
    /// <param name="content">Content of the message. string.Empty when no content.</param>
    public delegate void MessageResponseEvent(string content);





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                              Public Properties
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    /// <summary>
    /// Pipe name from one of the <see cref="NetworkPipes"/> sub-classes (for example: <see cref="ETG.PipeName"/>)
    /// </summary>
    public abstract string PipeName { get; }





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                              Protected Fields
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    protected static readonly Dictionary<string, MessageCallbackEvent?> callbacks = [];
    protected static readonly Dictionary<string, MessageDirectEvent?> directs = [];
    protected static readonly Dictionary<ResponseID, MessageResponseEvent?> responses = [];





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                               Private Fields
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    private static Action<object> m_Logger = Console.WriteLine;
    private static Action<object> m_ExceptionLogger = Console.WriteLine;
    private static ClientPipe? m_Instance;
    private static Worker? worker;





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                                Constructors
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public ClientPipe()
    {
        if (m_Instance == null)
        {
            m_Instance = this;
            IsClient = true;
            Initialize();
            return;
        }
        else
        {
            throw new InvalidOperationException(
                $"{LogPrefix} You should not instantiate multiple '{nameof(ClientPipe)}'s within one session.");
        }
    }





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                               Public Methods
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    /// <summary>
    /// Initializes <see cref="ClientPipe"/> as a class of a custom type <typeparamref name="T"/>.
    /// </summary>
    public static void Initialize<T>() where T : ClientPipe, new() => new T();

    /// <summary>
    /// Sends an message to a server.
    /// </summary>
    /// <remarks>
    /// Don't forget to format your message using <see cref="Messages"/> methods!
    /// </remarks>
    public static void Send(in Message message)
    {
        worker?.Send(message);
    }

    /// <summary>
    /// Sends a direct message to the server, and expects a response.
    /// </summary>
    /// <param name="message">Message to be sent.</param>
    /// <param name="response">Response hander of yours.</param>
    public static void Direct(in Message message, MessageResponseEvent response)
    {
        if (response == null) return;
        if (worker != null)
        {
            ResponseID id = message.ResponseID;
            if (id == ResponseID.Invalid)
            {
                return;
            }

            if (responses.TryGetValue(id, out var oldHandler))
            {
                Console.WriteLine($"Warning! Another process is already waiting for the same response ID! ID: {id}");
                responses[id] = oldHandler + response;
            }
            else
            {
                responses[id] = response;
            }

            worker.Send(message);
        }
    }

    /// <summary>
    /// Listens for value updates for a specific key.
    /// </summary>
    /// <remarks>
    /// Update processor will never receive 'null' - empty string at max.
    /// </remarks>
    /// <param name="key">Key/Parameter value of which can be updated.</param>
    /// <param name="action">Value update processor to be added.</param>
    public static void Listen(string key, MessageCallbackEvent action)
    {
        List(key, callbacks, action);
    }

    /// <summary>
    /// Removes listened for a specific key.
    /// </summary>
    /// <param name="key">Key/Parameter value of which can be updated.</param>
    /// <param name="action">Value update processor to be removed.</param>
    public static void Unlisten(string key, MessageCallbackEvent action)
    {
        Unlist(key, callbacks, action);
    }

    /// <summary>
    /// Listens for a specific direct message with content.
    /// </summary>
    /// <remarks>
    /// Update processor will never receive 'null' - empty string at max.
    /// </remarks>
    /// <param name="key">Key/Parameter value of which can be updated.</param>
    /// <param name="action">Message responder processor to be added.</param>
    public static void Listen(string key, MessageDirectEvent action)
    {
        List(key, directs, action);
    }

    /// <summary>
    /// Removes listened for a specific key.
    /// </summary>
    /// <param name="key">Key/Parameter value of which can be updated.</param>
    /// <param name="action">Message responder to be removed.</param>
    public static void Unlisten(string key, MessageDirectEvent action)
    {
        Unlist(key, directs, action);
    }

    private static void List<T>(string key, Dictionary<string, T?> dictionary, T? action) where T : Delegate
    {
        if (action == null) return;
        if (dictionary.TryGetValue(key, out T? callback) && callback != null)
        {
            dictionary[key] = (T)Delegate.Combine(callback, action);
        }
        else dictionary[key] = action;
    }

    private static void Unlist<T>(string key, Dictionary<string, T?> dictionary, T? action) where T : Delegate
    {
        if (action == null) return;
        if (dictionary.TryGetValue(key, out T? callback) && callback != null)
        {
            callback = Delegate.Remove(callback, action) as T;
            if (callback == null)
            {
                dictionary.Remove(key);
            }
            else dictionary[key] = callback;
        }
    }





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                             Protected Methods
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    protected virtual void Initialize()
    {
        Listen(Messages.Test, (_) => Logger($"{LogPrefix} Test message received!"));
        worker = StartWorker();
    }

    /// <summary>
    /// Constructs and starts a <see cref="Worker"/>, reading inputs from KCP.
    /// </summary>
    protected virtual Worker StartWorker() => new();

    /// <summary>
    /// Called before processing any of the messages, sent by KCP.
    /// </summary>
    protected virtual void PreProcess()
    {
        // Nothing yet, but there might be something later.
    }

    /// <summary>
    /// Called after all messages sent by KCP was read.
    /// </summary>
    protected virtual void PostProcess()
    {
        // Nothing yet, but there might be something later.
    }

    protected virtual void ProcessMessage(string name, string content)
    {
        if (callbacks.TryGetValue(name, out MessageCallbackEvent? callback) && callback != null)
        {
            callback.Invoke(content);
        }
    }

    /// <summary>
    /// Handles <see cref="MessageType.ParameterDirect"/> message.
    /// </summary>
    /// <remarks>
    /// If you want to respond to the message later - you can do that, but DONT forget to respond to the sender eventually!
    /// </remarks>
    /// <param name="response">Response code for client to respond-to.</param>
    protected virtual void HandleDirectMessage(ResponseID response, string name, string content)
    {
        if (directs.TryGetValue(name, out MessageDirectEvent? callback) && callback != null)
        {
            callback.Invoke(response, content);
        }
    }

    protected virtual void ReceiveResponse(ResponseID response, string content)
    {
        if (responses.TryGetValue(response, out MessageResponseEvent? callback) && callback != null)
        {
            callback.Invoke(content);
            responses.Remove(response);
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
    protected class Worker
    {
        /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
        /// .
        /// .                                               Static Fields
        /// .
        /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
        protected readonly List<Message> messages = [];
        protected readonly Action<Action> dispatcher;
        protected readonly Thread thread;





        /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
        /// .
        /// .                                                Constructors
        /// .
        /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
        public Worker()
        {
            dispatcher = GetDispatcher();
            thread = GetThread();
            thread.Start();
        }

        /// <summary>
        /// Constructs thread worker for reading data from a pipe.
        /// </summary>
        /// <returns></returns>
        protected virtual Thread GetThread() => new(ReadMessages)
        {
            IsBackground = true
        };

        /// <summary>
        /// Constructs dispatcher for any <see cref="Action"/>.
        /// Dispatcher will execute actions in the exact order in which <see cref="Action"/>s were sent to him.
        /// </summary>
        /// <remarks>
        /// By default does not dispatch anything - just executes given <see cref="Action"/>.
        /// </remarks>
        /// <returns>Dispatcher method.</returns>
        protected virtual Action<Action> GetDispatcher() => (a) => a.Invoke();





        /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
        /// .
        /// .                                               Public Methods
        /// .
        /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
        public void Send(in Message message)
        {
            lock (messages)
            {
                messages.Add(message);
            }
        }





        /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
        /// .
        /// .                                                Reading Pipe
        /// .
        /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
        /// <summary>
        /// Background worker method.
        /// </summary>
        protected void ReadMessages()
        {
            StringBuilder builder = new(256);
            while (true)
            {
                try
                {
                    if (m_Instance != null && !string.IsNullOrEmpty(m_Instance.PipeName))
                    {
                        using var pipe = new NamedPipeClientStream(
                            ServerName, m_Instance.PipeName,
                            PipeDirection.In);

                        pipe.Connect();

                        // Lock might be redundant, but just in case.
                        ClientPipe client;
                        lock (m_Instance)
                        {
                            client = m_Instance;
                        }

                        using var reader = new StreamReader(pipe, NetworkPipes.Encoding);
                        int result = reader.Peek(); // Ensures that the stream has anything to read.
                        if (result != -1)
                        {
                            dispatcher(() => client?.PreProcess());

                            try
                            {
                                do
                                {
                                    MessageType type = (MessageType)reader.Read();
                                    ResponseID response = default;
                                    string name;
                                    string content;
                                    switch (type)
                                    {
                                        case MessageType.Normal:
                                            ReadContent(reader, out name, out content);
                                            dispatcher(() => client?.ProcessMessage(name, content));
                                            break;

                                        case MessageType.Direct:
                                            response = ResponseID.Format(
                                                reader.Read(),
                                                reader.Read(),
                                                reader.Read(),
                                                reader.Read()
                                            );
                                            ReadContent(reader, out name, out content);
                                            dispatcher(() => client?.HandleDirectMessage(response, name, content));
                                            break;

                                        case MessageType.Response:
                                            response = ResponseID.Format(
                                                reader.Read(),
                                                reader.Read(),
                                                reader.Read(),
                                                reader.Read()
                                            );
                                            ReadAll(reader, out content);
                                            dispatcher(() => client?.ReceiveResponse(response, content));
                                            continue;

                                        default: throw new InvalidOperationException($"{LogPrefix} Unknown message type: {type}");
                                    }
                                }
                                while (reader.Peek() != -1);
                            }
                            catch
                            {
                                // Ignores another "Operation failed successfully" message)
                                // Gosh, that's a lot of try-catch statements.
                            }

                            dispatcher(() => client?.PostProcess());
                        }
                    }
                }
                catch (IOException)
                {
                    dispatcher(() => Logger($"{LogPrefix} Connection with Konoob Control Panel was lost."));
                }
                catch
                {
                    // Ignores "Operation failed successfully" message XD.
                }

                // Back-off before reconnect
                Thread.Sleep(HeartbeatMs);
            }
        }

        protected static void ReadContent(StreamReader reader, out string name, out string content)
        {
            // TODO: Replace with less memory-consuming solution.
            string? line = reader.ReadLine();
            if (line == null)
            {
                name = string.Empty;
                content = string.Empty;
                return;
            }

            // Cuts line from "name: content" form into parts.
            int index = line.IndexOf(ReaderKeys.ContentSeparator);
            if (index != -1)
            {
                name = line.Substring(0, index);
                int total = index + ReaderKeys.ContentSeparator.Length;
                content = line.Substring(total, line.Length - total);
            }
            else
            {
                name = line;
                content = string.Empty;
            }
        }

        protected static void ReadAll(StreamReader reader, out string content)
        {
            content = reader.ReadLine() ?? string.Empty;
        }
    }
}
