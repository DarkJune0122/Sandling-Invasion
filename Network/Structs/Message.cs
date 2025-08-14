#nullable enable
using System;

/// <summary>
/// Obtained using methods from <see cref="Pipes.Messages"/>.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1050:Declare types in namespaces", Justification = "For easier distribution.")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "To seal the one above")]
public readonly struct Message
{
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                                 Constants
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    /// <summary>
    /// Message with zero content.
    /// </summary>
    public static readonly Message Invalid = default;

    /// <summary>
    /// Default value for 'false' boolean parameter.
    /// </summary>
    public const string False = "false";

    /// <summary>
    /// Default value for 'true' boolean parameter.
    /// </summary>
    public const string True = "true";

    /// <summary>
    /// Marks direct message as accepted by any party.
    /// </summary>
    public const string Accepted = True;
    /// <summary>
    /// Name of an test message.
    /// </summary>
    public const string Test = "test";

    /// <summary>
    /// Sends general pipe configuration data to the client.
    /// </summary>
    public const string UpdateSettings = "update";

    /// <summary>
    /// Sent to the client when user tries to redeem a command.
    /// </summary>
    public const string Command = "command";

    /// <summary>
    /// Separator between parameter name and its value.
    /// </summary>
    /// <remarks>
    /// Separator might be changed later, as it is too common.
    /// </remarks>
    public const string ContentSeparator = "|||";





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                              Public Properties
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    /// <summary>
    /// Type of this <see cref="Message"/>. (See also: <seealso cref="MessageType"/>)
    /// </summary>
    public MessageType Type => content.Length == 0 ? MessageType.Unknown : (MessageType)content[0];

    /// <summary>
    /// Whether this message has <see cref="ResponseID"/>.
    /// </summary>
    /// <remarks>
    /// NOT cached! Evaluated upon each call.
    /// </remarks>
    public bool IsDirect => Type switch { MessageType.Request => true, MessageType.Response => true, _ => false };

    /// <summary>
    /// <see cref="ResponseID"/> of this message, or <see cref="ResponseID.Invalid"/> when there is no ResponseID.
    /// </summary>
    /// <remarks>
    /// NOT cached! Evaluated upon each call.
    /// </remarks>
    public ResponseID ResponseID => content.Length < ResponseID.ResponseLength + 1 ? ResponseID.Invalid
        : IsDirect ? ResponseID.Format(content, at: 1) : ResponseID.Invalid;





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                               Public Fields
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public readonly string content;





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                                Constructors
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    private Message(string message) => content = message;





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                              Implementations
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public override string ToString() => content;
    public override int GetHashCode() => content?.GetHashCode() ?? 0;
    public override bool Equals(object? obj)
    {
        return obj is Message other && string.Equals(content, other.content);
    }





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                                 Operators
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public static bool operator ==(Message left, Message right) => left.content == right.content;
    public static bool operator !=(Message left, Message right) => left.content != right.content;
    public static implicit operator string(Message message) => message.content;





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
    /// .                                                 Packing
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public static string Pack(string str1, string str2)
    {
        return $"{str1}{ContentSeparator}{str2}";
    }
    [Obsolete("You won't be able to unpack it - not supported yet.", true)]
    public static string Pack(string str1, string str2, string str3, string str4)
    {
        return $"{str1}{ContentSeparator}{str2}{ContentSeparator}{str3}{ContentSeparator}{str4}";
    }

    [Obsolete("You won't be able to unpack it - not supported yet.", true)]
    public static string Pack(string str1, string str2, string str3)
    {
        return $"{str1}{ContentSeparator}{str2}{ContentSeparator}{str3}";
    }





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                                 Unpacking
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public static void Unpack(string content, out string str1, out string str2)
    {
        int separator = content.IndexOf(ContentSeparator);
        if (separator == -1)
        {
            str1 = content;
            str2 = string.Empty;
            return;
        }
        str1 = content.Substring(0, separator);
        str2 = content.Substring(separator + ContentSeparator.Length);
    }





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                                Construction
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Made to make it unintuitive to create those messages - to make proper ways for creating them more obvious.")]
    public readonly struct MessageBuilder
    {
        // TODO: Add methods that use streams directly, to optimize memory usage.
        /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
        /// .
        /// .                                              Public Properties
        /// .
        /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
        public Message GetTest() => Get(MessageType.Normal, Test);
        public Message GetTestDirect() => Get(MessageType.Request, ResponseID.TestID, Test);
        public Message GetTestResponse() => Get(MessageType.Response, ResponseID.TestID, "-test content-");

        /// <summary>
        /// Creates <see cref="Message"/> from already formed message.
        /// </summary>
        /// <param name="message">Message, assumingly, with <see cref="MessageType"/> already inserted.</param>
        public Message Raw(string message) => new(message);

        /// <summary>
        /// Creates <see cref="Message"/> by combining <paramref name="type"/> and <paramref name="message"/> together in a right way.
        /// </summary>
        /// <param name="message">Message, assumingly, without <see cref="MessageType"/> inserted.</param>
        public Message Get(MessageType type, string message) => new($"{(char)type}{message}");

        /// <summary>
        /// Creates <see cref="Message"/> with <paramref name="message"/> as an attachment.
        /// </summary>
        /// <param name="message">Message, assumingly, without <see cref="MessageType"/> and <paramref name="id"/> inserted.</param>
        public Message Get(MessageType type, ResponseID id, string message) => new($"{(char)type}{ResponseID.Format(id)}{message}");

        /// <summary>
        /// Creates response <see cref="Message"/> with empty body to a given response <paramref name="id"/>.
        /// </summary>
        public Message Get(MessageType type, ResponseID id) => new($"{(char)type}{ResponseID.Format(id)}");
    }
}
