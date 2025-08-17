using KCPClient;
using SandlingInvasion;

/// <summary>
/// Gives functions to work with Konoob Control Panel messages in Enter the Gungeon.
/// </summary>
public sealed class ETGPipeAPI() : ClientPipe(Pipes.ETG.PipeName)
{
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                              Public Properties
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public static new ETGPipeAPI Instance { get; } = (ETGPipeAPI)ClientPipe.Instance;





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
        SetupEvents();
    }

    private void SetupEvents()
    {
        Pipes.ETG.Initialize();
        //Pipes.ETG.Blank += (username) => Plugin.Log($"{username} - {Pipes.ETG.BlankCommand}");
        //Pipes.ETG.Ammo += (username) => Plugin.Log($"{username} - {Pipes.ETG.AmmoCommand}");
        //Pipes.ETG.Health += (username) => Plugin.Log($"{username} - {Pipes.ETG.HealthCommand}");
        //Pipes.ETG.Shield += (username) => Plugin.Log($"{username} - {Pipes.ETG.ShieldCommand}");

        //DungeonHooks.OnPostDungeonGeneration += () => Send(Pipes.ETG.NewGameEvent);
    }
}