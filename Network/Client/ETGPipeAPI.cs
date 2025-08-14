using SandlingInvasion;
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
public sealed class ETGPipeAPI : ClientPipe
{
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                              Public Properties
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public static new ETGPipeAPI Instance { get; } = (ETGPipeAPI)ClientPipe.Instance;
    public override string PipeName => NetworkPipes.ETG.PipeName;





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                              Implementations
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    protected override void Initialize()
    {
        base.Initialize();
        UnityDispatcher.Initialize();
        UnityDispatcher.ExceptionLogger = Plugin.Log;
        Logger = Plugin.Log;
        ExceptionLogger = Plugin.Log;
    }
}