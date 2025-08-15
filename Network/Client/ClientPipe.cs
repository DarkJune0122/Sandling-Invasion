#nullable enable
using System;
using System.IO.Pipes;

/// <summary>
/// Standardized pipe class for clients.
/// </summary>
/// <![CDATA[v0.0.1]]>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1050:Declare types in namespaces", Justification = "For easier distribution.")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "To seal the one above")]
public abstract class ClientPipe : Transporter
{
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                              Static Properties
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public static ClientPipe? Instance { get; private set; }





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                              Public Properties
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    /// <summary>
    /// Pipe name from one of the <see cref="Pipes"/> sub-classes (for example: <see cref="ETG.PipeName"/>)
    /// </summary>
    public abstract string PipeName { get; }





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                                Constructors
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public ClientPipe() : base()
    {
        if (Instance == null)
        {
            Instance = this;
            Initialize();
            return;
        }
        else
        {
            throw new InvalidOperationException(
                $"{Pipes.LogPrefix} You should not instantiate multiple '{nameof(ClientPipe)}'s within one session.");
        }
    }





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                               Static Methods
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    /// <summary>
    /// Initializes <see cref="ClientPipe"/> as a class of a custom type <typeparamref name="T"/>.
    /// </summary>
    public static void Initialize<T>() where T : ClientPipe, new() => new T();





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                             Protected Methods
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    protected virtual void Initialize()
    {
        // Nothing yet, but there might be something later. Always call this base method, please.
    }

    protected override void OnConnected()
    {
        base.OnConnected();
        Send(Message.UpdateSettings);
    }

    /// <summary>
    /// Called when either server notifies about disconnection.
    /// </summary>
    protected override void OnDisconnected()
    {
        base.OnDisconnected();
    }

    protected override PipeStream? Connect(ref bool canceled)
    {
        using var stream = new NamedPipeClientStream(
            Pipes.ServerName, PipeName,
            PipeDirection.InOut);

        stream.Connect();
        return stream;
    }
}
