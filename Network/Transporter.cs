#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using static Pipes;

/// <summary>
/// Standard data <see cref="Transporter"/> for networking. Both for client and server.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1050:Declare types in namespaces", Justification = "For easier distribution.")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "To seal the one above")]
public abstract class Transporter : IDisposable
{
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                              Public Properties
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    /// <summary>
    /// Whether transporter can and is sending any messages.
    /// </summary>
    public virtual bool Active
    {
        get => m_Active;
        set => m_Active = value;
    }

    /// <summary>
    /// Online or offline status of the transporter.
    /// </summary>
    /// <remarks>
    /// Changes to <c>false</c> after a while without any connection to the remote host.
    /// </remarks>
    public bool Status
    {
        get => m_Status;
        private set
        {
            if (m_Status == value) return;
            if (value)
            {
                OnConnected();
            }
            else
            {
                OnDisconnected();
            }
        }
    }

    /// <summary>
    /// Delay that has to happen between messages to be sensed as connection timeout.
    /// </summary>
    /// <remarks>
    /// Once <see cref="Transporter"/> timed out - <see cref="Status"/> will be set to either <c>false</c> or <c>true</c>.
    /// </remarks>
    public int TimeoutDelayMs
    {
        get => m_TimeoutDelayMs;
        set => m_TimeoutDelayMs = Math.Max(value, 1000);
    }

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
    /// <param name="content">Content of the message. <see cref="string.Empty"/> when no content.</param>
    public delegate void MessageHandler(string content);

    /// <summary>
    /// Callback for two-way message - <see cref="MessageType.Request"/>.
    /// </summary>
    /// <param name="response">Response ID that can be used to send data back to the server.</param>
    /// <param name="content">Content of the message. <see cref="string.Empty"/> when no content.</param>
    public delegate void RequestHandler(ResponseID response, string content);

    /// <summary>
    /// Callback from previously sent direct message - <see cref="MessageType.Response"/>.
    /// </summary>
    /// <param name="content">Content of the message. <see cref="string.Empty"/> when no content.</param>
    public delegate void ResponseHandler(string content);





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                              Protected Fields
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    protected readonly Dictionary<string, MessageHandler?> callbacks = [];
    protected readonly Dictionary<string, RequestHandler?> requests = [];
    protected readonly Dictionary<ResponseID, ResponseHandler?> responses = [];
    protected readonly ResponseID.ResponseIDBuilder ids = new();
    protected readonly Message.MessageBuilder builder = new();

    /// <summary>
    /// All the pending messages to be sent to the remote host.
    /// </summary>
    protected readonly List<Message> messages = [];
    protected readonly Action<Action> dispatcher;
    protected readonly Thread worker;

    protected bool m_Active = true;
    protected bool m_Status = false;

    /// <summary>
    /// <see cref="Environment.TickCount64"/> from the last time connection was made.
    /// </summary>
    protected int lastConnectionTime = int.MinValue;





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                               Private Fields
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    private static Action<object> m_Logger = Console.WriteLine;
    private static Action<object> m_ExceptionLogger = Console.WriteLine;
    private int m_TimeoutDelayMs = 1000;
    /// <summary>
    /// Whether underlying thread should be canceled.
    /// Once set to <c>true</c> - should never be set to <c>false</c> again.
    /// </summary>
    private bool canceled = false;





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                                Constructors
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    /// <summary>
    /// Creates main-thread <see cref="Transporter"/> without dispatcher.
    /// </summary>
    /// <remarks>
    /// Use <see cref="Transporter(Action{Action})"/> to create background-thread transporter.
    /// </remarks>
    public Transporter()
    {
        dispatcher = (a) => a?.Invoke();
        worker = new Thread(ReadStream)
        {
            Name = "Transporter Stream Reader",
        };

        Listen(Message.Test, (_) => Logger($"{LogPrefix} Test message received!"));
        Handle(Message.Test, (response, _) =>
        {
            Logger($"{LogPrefix} Test request received!");
            Respond(response);
        });
    }

    /// <summary>
    /// Creates background-thread <see cref="Transporter"/> with dispatcher.
    /// </summary>
    /// <remarks>
    /// Use <see cref="Transporter()"/> to create main-thread transporter.
    /// </remarks>
    /// <param name="dispatcher">Dispatched callback, to receive messages.</param>
    public Transporter(Action<Action> dispatcher)
    {
        this.dispatcher = dispatcher;
        worker = new Thread(ReadStream)
        {
            Name = "Transporter Stream Reader",
            IsBackground = true,
        };
    }

    ~Transporter()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        Dispose(true);
    }

    private void Dispose(bool disposing)
    {
        canceled = true;
        if (!disposing)
        {
            worker.Join(1000); // Wait for the last connection to finish.
        }
    }





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                            Consistent Listening
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    /// <summary>
    /// Listens for a specific <see cref="MessageType.Normal"/> message.
    /// </summary>
    /// <remarks>
    /// Update processor will never receive 'null' - empty string at max.
    /// </remarks>
    /// <param name="message">Key/Parameter value of which can be updated.</param>
    /// <param name="handler">Value update processor to be added.</param>
    public void Listen(string message, MessageHandler handler)
    {
        List(message, callbacks, handler);
    }

    /// <summary>
    /// Removes listened for a specific <see cref="MessageType.Normal"/> message.
    /// </summary>
    /// <param name="message">Key/Parameter value of which can be updated.</param>
    /// <param name="handler">Value update processor to be removed.</param>
    public void Unlisten(string message, MessageHandler handler)
    {
        Unlist(message, callbacks, handler);
    }

    /// <summary>
    /// Listens for a specific <see cref="MessageType.Request"/> message.
    /// </summary>
    /// <remarks>
    /// Update processor will never receive 'null' - empty string at max.
    /// </remarks>
    /// <param name="message">Key/Parameter value of which can be updated.</param>
    /// <param name="handler">Value update processor to be added.</param>
    public void Handle(string message, RequestHandler handler)
    {
        List(message, requests, handler);
    }

    /// <summary>
    /// Removes listened for a specific <see cref="MessageType.Request"/> message.
    /// </summary>
    /// <param name="message">Key/Parameter value of which can be updated.</param>
    /// <param name="handler">Value update processor to be removed.</param>
    public void Miss(string message, RequestHandler handler)
    {
        Unlist(message, requests, handler);
    }





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                               Public Methods
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public void Test()
    {
        if (Status == false) return;
        Message message = builder.Get(MessageType.Normal, Message.Test);
        SendInternal(message);
    }

    public void TestRequest(ResponseHandler handler)
    {
        if (Status == false) return;
        Message message = builder.Get(MessageType.Request, ResponseID.TestID, Message.Test);
        RegisterRequestInternal(ResponseID.TestID, handler);
        SendInternal(message);
    }



    public void Send(string name)
    {
        if (Status == false) return;
        Message message = builder.Get(MessageType.Normal, name);
        SendInternal(message);
    }

    public void Send(string name, string? content)
    {
        if (Status == false) return;
        content ??= string.Empty;
        Message message = builder.Get(MessageType.Normal, Message.Pack(name, content));
        SendInternal(message);
    }



    public void Request(string name, ResponseHandler handler)
    {
        if (Status == false) return;
        ResponseID id = ids.GetNext();
        Message message = builder.Get(MessageType.Request, id, name);
        RegisterRequestInternal(id, handler);
        SendInternal(message);
    }

    public void Request(string name, string? content, ResponseHandler handler)
    {
        if (Status == false) return;
        content ??= string.Empty;
        ResponseID id = ids.GetNext();
        Message message = builder.Get(MessageType.Request, id, Message.Pack(name, content));
        RegisterRequestInternal(id, handler);
        SendInternal(message);
    }



    public void Respond(ResponseID response)
    {
        if (Status == false) return;
        Message message = builder.Get(MessageType.Response, response);
        SendInternal(message);
    }

    public void Respond(ResponseID response, string? content)
    {
        if (Status == false) return;
        content ??= string.Empty;
        Message message = builder.Get(MessageType.Response, response, content);
        SendInternal(message);
    }




    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                                  Internal
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    private void SendInternal(in Message message)
    {
        lock (messages)
        {
            messages.Add(message);
        }
    }

    private void RegisterRequestInternal(ResponseID id, ResponseHandler handler)
    {
        if (responses.TryGetValue(id, out var oldHandler))
        {
            Console.WriteLine($"Warning! Another process is already waiting for the same response ID! ID: {id}");
            responses[id] = oldHandler + handler;
        }
        else
        {
            responses[id] = handler;
        }
    }





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                                 Callbacks
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    protected virtual void OnConnected()
    {
        Logger($"{LogPrefix} Connected to the remote host.");
    }

    protected virtual void OnDisconnected()
    {
        Logger($"{LogPrefix} Disconnected from the remote host.");
        responses.Clear();
    }





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                              Handling Methods
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    /// <summary>
    /// Handles and distributes regular messages - <see cref="MessageType.Normal"/>.
    /// </summary>
    /// <param name="name">Name of the message.</param>
    /// <param name="content">Its content. <see cref="string.Empty"/> if no content.</param>
    protected virtual void HandleMessage(string name, string content)
    {
        if (callbacks.TryGetValue(name, out MessageHandler? callback) && callback != null)
        {
            callback.Invoke(content);
        }
    }

    /// <summary>
    /// Handles <see cref="MessageType.Request"/> message.
    /// </summary>
    /// <remarks>
    /// If you want to respond to the message later - you can do that, but DONT forget to respond to the sender eventually!
    /// </remarks>
    /// <param name="response">Response code for client to respond-to.</param>
    protected virtual void HandleRequest(ResponseID response, string name, string content)
    {
        if (requests.TryGetValue(name, out RequestHandler? callback) && callback != null)
        {
            callback.Invoke(response, content);
        }
    }

    protected virtual void HandleResponse(ResponseID response, string content)
    {
        if (responses.TryGetValue(response, out ResponseHandler? callback) && callback != null)
        {
            responses.Remove(response);
            callback.Invoke(content);
        }
    }




    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                                 Utilities
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    protected static void List<T>(string key, Dictionary<string, T?> dictionary, T? action) where T : Delegate
    {
        if (action == null) return;
        if (dictionary.TryGetValue(key, out T? callback) && callback != null)
        {
            dictionary[key] = (T)Delegate.Combine(callback, action);
        }
        else dictionary[key] = action;
    }

    protected static void Unlist<T>(string key, Dictionary<string, T?> dictionary, T? action) where T : Delegate
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
    /// .                                           Stream Reading/Writing
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    /// <summary>
    /// Connects to the remote host.
    /// </summary>
    /// <remarks>
    /// This code might execute on a background thread.
    /// And please, do NOT use 'using' in this method - <see cref="Transporter"/> will close <see cref="PipeStream"/> for you!
    /// </remarks>
    /// <param name="canceled">Whether connection task was canceled or not. A bit hacky method.</param>
    /// <returns>
    /// <see cref="PipeStream"/> for reading and writing purposes. <c>NULL</c> if no need to connect at the moment.
    /// </returns>
    protected abstract PipeStream? Connect(ref bool canceled);
    protected void ReadStream()
    {
        while (!canceled)
        {
            if (!m_Active)
            {
                // Prevent CPU burn, LOL
                Thread.Sleep(HeartbeatMs * 2);
                continue;
            }

            using var stream = Connect(ref canceled);
            if (stream == null || stream.IsConnected)
            {
                Thread.Sleep(HeartbeatMs * 2);
                UpdateStatus();
                continue;
            }
            else SetStatusActive();

            // Do we need to lock list here?
            if (m_Active && messages.Count > 0)
            {
                while (stream.IsConnected && !canceled)
                {
                    string[] toSend;
                    lock (messages)
                    {
                        toSend = [.. messages];
                        messages.Clear();
                    }

                    foreach (string message in toSend)
                    {
                        stream.Write(Encoding.GetBytes(message + "\n"), 0, message.Length);
                    }

                    stream.Flush();
                }
            }

            using var reader = new StreamReader(stream, Encoding);
            int result = reader.Peek(); // Ensures that the stream has anything to read.
            if (result != -1)
            {
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
                                dispatcher(() => HandleMessage(name, content));
                                break;

                            case MessageType.Request:
                                response = ResponseID.Format(
                                    reader.Read(),
                                    reader.Read(),
                                    reader.Read(),
                                    reader.Read()
                                );
                                ReadContent(reader, out name, out content);
                                dispatcher(() => HandleRequest(response, name, content));
                                break;

                            case MessageType.Response:
                                response = ResponseID.Format(
                                    reader.Read(),
                                    reader.Read(),
                                    reader.Read(),
                                    reader.Read()
                                );
                                ReadAll(reader, out content);
                                dispatcher(() => HandleResponse(response, content));
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
            }

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
        int index = line.IndexOf(Message.ContentSeparator);
        if (index != -1)
        {
#pragma warning disable IDE0057 // Use range operator - avoided for easier distribution.
            name = line.Substring(0, index);
            int offset = index + Message.ContentSeparator.Length;
            content = line.Substring(offset, line.Length - offset);
#pragma warning restore IDE0057 // Use range operator
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

    protected void UpdateStatus()
    {
        int tick = Environment.TickCount;
        Status = tick - lastConnectionTime <= TimeoutDelayMs;
    }

    protected void SetStatusActive()
    {
        lastConnectionTime = Environment.TickCount;
        UpdateStatus();
    }
}
