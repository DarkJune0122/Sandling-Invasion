#nullable enable
using System;

/// <summary>
/// Obtained using methods from <see cref="NetworkPipes.Messages"/>.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1050:Declare types in namespaces", Justification = "For easier distribution.")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "To seal the one above")]
public readonly struct Message
{
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                              Static Properties
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public static readonly Message Invalid = default;





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
    public bool HasResponseID => Type switch { MessageType.Direct => true, MessageType.Response => true, _ => false };

    /// <summary>
    /// <see cref="ResponseID"/> of this message, or <see cref="ResponseID.Invalid"/> when there is no ResponseID.
    /// </summary>
    /// <remarks>
    /// NOT cached! Evaluated upon each call.
    /// </remarks>
    public ResponseID ResponseID => content.Length < ResponseID.ResponseLength + 1 ? ResponseID.Invalid
        : HasResponseID ? ResponseID.Format(content, at: 1) : ResponseID.Invalid;





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
        return obj is Message other && string.Equals(content, other.content, StringComparison.Ordinal);
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
        public Message GetTest() => Get(MessageType.Normal, NetworkPipes.Messages.Test);
        public Message GetTestDirect() => Get(MessageType.Direct, ResponseID.TestID, NetworkPipes.Messages.Test);
        public Message GetTestResponse() => Get(ResponseID.TestID, "-test content-");

        /// <summary>
        /// Creates <see cref="Message"/> from already formed message.
        /// </summary>
        /// <param name="message">Message, assumingly, with <see cref="MessageType"/> already inserted.</param>
        public Message Get(string message) => new(message);

        /// <summary>
        /// Creates <see cref="Message"/> by combining <paramref name="type"/> and <paramref name="message"/> together in a right way.
        /// </summary>
        /// <param name="message">Message, assumingly, without <see cref="MessageType"/> inserted.</param>
        public Message Get(MessageType type, string message) => new($"{(char)type}{message}");

        /// <summary>
        /// Creates direct <see cref="Message"/> from given <paramref name="message"/>.
        /// </summary>
        /// <param name="message">Message, assumingly, without <see cref="MessageType"/> and <paramref name="id"/> inserted.</param>
        public Message Get(MessageType type, ResponseID id, string message) => new($"{(char)type}{ResponseID.Format(id)}{message}");

        /// <summary>
        /// Creates response <see cref="Message"/> to a given <see cref="ResponseID"/>.
        /// </summary>
        /// <param name="message">Message, assumingly, without <see cref="MessageType"/> and <paramref name="id"/> inserted.</param>
        public Message Get(ResponseID id) => new($"{(char)MessageType.Response}{ResponseID.Format(id)}");

        /// <summary>
        /// Creates response <see cref="Message"/> to a given <see cref="ResponseID"/> with <paramref name="message"/> as content.
        /// </summary>
        /// <param name="message">Content of the response message.</param>
        public Message Get(ResponseID id, string message) => new($"{(char)MessageType.Response}{ResponseID.Format(id)}{message}");
    }
}
