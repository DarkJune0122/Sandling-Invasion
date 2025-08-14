public static partial class NetworkPipes
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





        /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
        /// .
        /// .                                                  Features
        /// .
        /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
        public static readonly PipeName PipeName = "KonoobiSandlingsUnited_EnterTheGungeon";

        /// <summary>
        /// Setting key for "Invasion" mode. Mode allows Sandlings to enter the game via Twitch chat directly.
        /// </summary>
        public const string SettingInvasionMode = "setting:invasion";





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
            ClientPipe.Listen(SettingInvasionMode, (v) => InvasionMode = Messages.Boolean(v));
        }
    }
}
