#nullable enable
/// <summary>
/// All message types that system supports.
/// </summary>
/// <remarks>
/// You can convert this value to <see cref="char"/> and back to for message identification.
/// </remarks>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1050:Declare types in namespaces", Justification = "For easier distribution.")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "To seal the one above")]
public enum MessageType : byte
{
    /// <summary>
    /// Usually never returned - only if bug occurred.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// Indicates that message is a response to previous direct message.
    /// </summary>
    Response = (byte)'R',

    /// <summary>
    /// Indicates one-way message, that does not expect any response.
    /// </summary>
    Normal = (byte)'N',

    /// <summary>
    /// Indicates two-way message, that expects a response.
    /// </summary>
    Request = (byte)'D',
}
