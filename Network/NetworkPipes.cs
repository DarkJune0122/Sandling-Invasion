#nullable enable
using System;
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
public static partial class NetworkPipes
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
    public static bool IsClient { get; set; } = false;





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
    /// Standard delay between sending messaged.
    /// </summary>
    /// <remarks>
    /// NEVER set below 1! or even 5!!! You don't need '200-1000 per Applications' updates per second!
    /// </remarks>
    public const int HeartbeatMs = 350;





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





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                            Message Construction
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public static class ReaderKeys
    {
        /// <summary>
        /// Separator between parameter name and its value.
        /// </summary>
        /// <remarks>
        /// Separator might be changed later, as it is too common.
        /// </remarks>
        public const string ContentSeparator = "|||";
    }

    /// <summary>
    /// Common methods for constructing messages.
    /// </summary>
    public static class Messages
    {
        /// <summary>
        /// Name of an test message.
        /// </summary>
        public const string Test = "test";

        /// <summary>
        /// Sends general pipe configuration data to the client.
        /// </summary>
        public const string UpdateSettings = "update";

        /// <summary>
        /// Default value for 'true' boolean parameter.
        /// </summary>
        public const string True = "true";

        /// <summary>
        /// Default value for 'false' boolean parameter.
        /// </summary>
        public const string False = "false";





        /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
        /// .
        /// .                                               Private Fields
        /// .
        /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
        /// <remarks>
        /// Not exposed, to make it easier to use pre-defined method in <see cref="Messages"/>.
        /// </remarks>
        private static readonly Message.MessageBuilder builder = new();

        /// <summary>
        /// Client-side ID builder.
        /// </summary>
        private static readonly ResponseID.ResponseIDBuilder clientIDs = new();





        /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
        /// .
        /// .                                                  Parsers
        /// .
        /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
        /// <summary>
        /// Retrieves <see cref="bool"/> from given <paramref name="value"/>.
        /// </summary>
        /// <returns>'false' by default. 'true' if value equals to <see cref="True"/>.</returns>
        public static bool Boolean(string value) => value == True;

        /// <summary>
        /// Retrieves <see cref="string"/> from given <see cref="bool"/> <paramref name="value"/>.
        /// </summary>
        /// <returns><see cref="True"/> if 'true', <see cref="False"/> if 'false.</returns>
        public static string Boolean(bool value) => value ? True : False;





        /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
        /// .
        /// .                                               Static Methods
        /// .
        /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
        /// <summary>
        /// Constructs test message for <see cref="MessageType.Test"/>.
        /// </summary>
        public static Message TestMessage() => builder.GetTest();

        /// <summary>
        /// Constructs test message for <see cref="MessageType.Test"/>.
        /// </summary>
        public static Message TestDirect() => builder.GetTestDirect();

        /// <summary>
        /// Constructs test message for <see cref="MessageType.Test"/>.
        /// </summary>
        public static Message TestResponse() => builder.GetTestResponse();

        /// <summary>
        /// Constructs <see cref="Message"/> with some <paramref name="message"/>, if provided.
        /// </summary>
        /// <returns><see cref="Message"/>, with <see cref="MessageType.Normal"/> as identifier.</returns>
        public static Message Normal(string parameter, string? message = null)
        {
            if (parameter == null || parameter.Length == 0) throw new ArgumentException("Cannot create message for an empty parameter.");
            return builder.Get(
                type: MessageType.Normal,
                message: $"{parameter}{ReaderKeys.ContentSeparator}{message ?? string.Empty}");
        }

        /// <inheritdoc cref="Direct(string, ResponseID.ResponseIDBuilder)"/>
        public static Message Direct(string parameter, string? content = null)
        {
            return Direct(clientIDs, parameter, content);
        }

        /// <summary>
        /// Constructs <see cref="Message"/> with some <paramref name="message"/>, if provided.
        /// </summary>
        /// <param name="ids">Custom <see cref="ResponseID"/> provider, for server-side.</param>
        /// <returns><see cref="Message"/>, with <see cref="MessageType.Direct"/> as identifier.</returns>
        public static Message Direct(ResponseID.ResponseIDBuilder ids, string parameter, string? message = null)
        {
            if (parameter == null || parameter.Length == 0) throw new ArgumentException("Cannot create message for an empty parameter.");
            return builder.Get(
                type: MessageType.Direct, id: ids.GetNext(),
                message: $"{parameter}{ReaderKeys.ContentSeparator}{message ?? string.Empty}");
        }

        /// <summary>
        /// Constructs custom flag <see cref="Message"/>. Useful for events and etc.
        /// </summary>
        /// <returns><see cref="Message"/>, with <see cref="MessageType.Response"/> as identifier.</returns>
        public static Message Response(ResponseID id)
        {
            return builder.Get(id);
        }

        /// <summary>
        /// Constructs response to an <see cref="Message"/> <paramref name="id"/>.
        /// </summary>
        /// <param name="id">ID of the message, provided by remote host.</param>
        /// <param name="message">Content to be attached to the message.</param>
        public static Message Response(ResponseID id, string? message)
        {
            return builder.Get(id, message: $"{message ?? string.Empty}");
        }
    }

    /// <summary>
    /// Common methods for reading messages.
    /// </summary>
    public static class Read
    {
        /// <summary>
        /// Reads message type from '0' index within a message.
        /// </summary>
        /// <returns><see cref="global::MessageType"/>, directly casted from <see cref="char"/>.</returns>
        [Obsolete("Use message.Type instead.", true)]
        public static MessageType MessageType(Message message)
        {
            if (message.content.Length == 0) return global::MessageType.Unknown;
            return (MessageType)message.content[0];
        }
    }
}