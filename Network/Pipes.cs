#nullable enable
using System.Text;

/// <summary>
/// Stores all the names to network pipes.
/// </summary>
/// <remarks>
/// Separated for easier distribution between applications.
/// </remarks>
/// <![CDATA[v0.0.1]]>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1050:Declare types in namespaces", Justification = "For easier distribution.")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "To seal the one above")]
public static partial class Pipes
{
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                              Public Properties
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    /// <summary>
    /// Standardized pipe message data encoding type.
    /// </summary>
    public static Encoding Encoding => Encoding.UTF8;





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                                 Constants
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public const string ServerName = ".";
    public const string ReadableServerName = "Konoob Control Panel";

    /// <summary>
    /// Prefix for all logging operations on all platforms.
    /// </summary>
    public const string LogPrefix = "[KPC]";

    /// <summary>
    /// Standard delay between sending messaged + <see cref="ConnectionHeartbeatMs"/>.
    /// </summary>
    /// <remarks>
    /// NEVER set below 1! or even 5!!! You don't need '200-1000 per Applications' updates per second!
    /// </remarks>
    public const int HeartbeatMs = 200;

    /// <summary>
    /// How small delays between connection checks will be.
    /// </summary>
    public const int ConnectionHeartbeatMs = 50;





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                               Static Fields
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    /// <summary>
    /// All pipe names, composed in one handy array.
    /// </summary>
    public static readonly PipeName[] All = [
        ETG.PipeName,
    ];





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                            Connection Reports
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public static string GetConnectionEstablishedMessage()
    {
        return GetConnectionEstablishedMessage(Locales.English);
    }

    /// <summary>
    /// Retrieves a welcome message for a specific locale.
    /// </summary>
    /// <remarks>
    /// Don't kill me for the pun T^T.
    /// </remarks>
    /// <param name="locale">Locale to be used. <see cref="Locales.English"/> by default.</param>
    /// <returns>Localized "Connection established" message.</returns>
    public static string GetConnectionEstablishedMessage(string locale)
    {
        return locale switch
        {
            Locales.English or _ => "Welcome! Connection with Konoobi control established sandsessfully!",
        };
    }

    public static string GetConnectionTerminatedMessage()
    {
        return GetConnectionTerminatedMessage(Locales.English);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// *Connection terminated. I'm sorry to interrupt you, Elisabeth, if you still even remember that name.*
    /// </remarks>
    /// <param name="locale">Locale to be used. <see cref="Locales.English"/> by default.</param>
    /// <returns>Localized "Connection terminated" message.</returns>
    private static string GetConnectionTerminatedMessage(string locale)
    {
        return locale switch
        {
            Locales.English or _ => "Connection terminated. I'm sorry to interrupt you, Elisabeth. Likely KonoobControl app was closed.",
        };
    }





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                                  Locale
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public static class Locales
    {
        public const string English = "en";
    }
}