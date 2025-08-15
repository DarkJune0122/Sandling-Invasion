#nullable enable
public static partial class Pipes
{
    /// <summary>
    /// Configuration data for Enter the Gungeon.
    /// </summary>
    /// <![CDATA[v0.0.1]]>
    public static class ETG
    {
        /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
        /// .
        /// .                                              Static Properties
        /// .
        /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
        /// <summary>
        /// Whether 'Sandling Invasion' mode was activated in KCP.
        /// </summary>
        public static bool InvasionMode { get; set; } = false;

        public static event CommandReceivedEventHandler? Blank;
        public static event CommandReceivedEventHandler? Ammo;
        public static event CommandReceivedEventHandler? Health;
        public static event CommandReceivedEventHandler? Shield;
        public static event ReplacePlayerEventHandler? ReplacePlayer;






        /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
        /// .
        /// .                                                   Events
        /// .
        /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
        public delegate void CommandReceivedEventHandler(string username);
        public delegate void ReplacePlayerEventHandler(string resignedPlayer, string newPlayer);





        /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
        /// .
        /// .                                                  Features
        /// .
        /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
        public static readonly PipeName PipeName = "KonoobiSandlingsUnited_EnterTheGungeon";

        /// <summary>
        /// Setting key for "Invasion" mode. Mode allows Sandlings to enter the game via Twitch chat directly.
        /// </summary>
        public const string SettingInvasionMode = "etg:setting:invasion";

        /// <summary>
        /// Event is sent to the server when new game starts.
        /// </summary>
        public const string NewGameEvent = "etg:event:started_new_game";

        /// <summary>
        /// Requests random player name from available users from the server.
        /// </summary>
        public const string RequestRandomPlayer = "etg:event:random_player";

        /// <summary>
        /// Request from a server to approve and use random player, provided after <see cref="RequestRandomPlayer"/> event.
        /// </summary>
        /// <remarks>
        /// Expects <see cref="Message.Accepted"/> as return value.
        /// </remarks>
        public const string ApproveRandomPlayer = "etg:event:receive_player";

        /// <summary>
        /// Requests remote client to replace player with another one.
        /// </summary>
        public const string ReplacePlayerEvent = "etg:event:replace_player";





        /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
        /// .
        /// .                                                 Commands
        /// .
        /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
        public const string InvadeCommand = "invade";
        public const string UninvadeCommand = "uninvade";
        public const string ResignCommand = "resign";

        // Client-side commands.
        public const string BlankCommand = "blank";
        public const string AmmoCommand = "ammo";
        public const string HealthCommand = "health";
        public const string ShieldCommand = "shield";





        /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
        /// .
        /// .                                               Static Methods
        /// .
        /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
        /// <summary>
        /// Requests all the settings from KCP to be sent to this instance.
        /// </summary>
        public static void Initialize()
        {
            ClientPipe.Instance?.Listen(SettingInvasionMode, (v) => InvasionMode = Message.Boolean(v));
            ClientPipe.Instance?.Listen(Message.Command, (content) =>
            {
                Message.Unpack(content, out string username, out string command);
                switch (command)
                {
                    case BlankCommand: Blank?.Invoke(username); break;
                    case AmmoCommand: Ammo?.Invoke(username); break;
                    case HealthCommand: Health?.Invoke(username); break;
                    case ShieldCommand: Shield?.Invoke(username); break;
                }
            });

            ClientPipe.Instance?.Listen(ReplacePlayerEvent, (content) =>
            {
                Message.Unpack(content, out string resignedPlayer, out string newPlayer);
                ReplacePlayer?.Invoke(resignedPlayer, newPlayer);
            });
        }
    }
}
