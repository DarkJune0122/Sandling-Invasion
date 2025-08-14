#nullable enable
using System;

/// <summary>
/// Provides/holds ID for specific direct <see cref="Message"/>s.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1050:Declare types in namespaces", Justification = "For easier distribution.")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "To seal the one above")]
public readonly struct ResponseID
{
    // TODO: What if client/server tries to respond to the same ID twice, thrice?
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                                 Constants
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    /// <summary>
    /// Length (in characters) of a section in message, that contains ResponseID.
    /// </summary>
    public const int ResponseLength = 4;

    /// <summary>
    /// ID for testing purposes only. This ID is avoided in regular messages.
    /// </summary>
    public static readonly ResponseID TestID = new(1);

    /// <summary>
    /// <see cref="ResponseID"/> that is considered invalid.
    /// </summary>
    /// <remarks>
    /// Same as <see cref="default"/>(<see cref="ResponseID"/>).
    /// </remarks>
    public static readonly ResponseID Invalid = default;





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                               Public Fields
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public readonly ushort code = default;





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                                Constructors
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    private ResponseID(ushort code)
    {
        this.code = code;
        Console.WriteLine($":: ResponseID created: {Format(this)} ({code})");
    }





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                               Public Methods
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    /// <summary>
    /// Responds to the server to indicate that message was received.
    /// </summary>
    public void ClientRespond()
    {
        if (NetworkPipes.IsClient) NetworkPipes.Messages.Response(code);
        else throw new InvalidOperationException("Cannot use simplified respond method on server side.");
    }

    /// <summary>
    /// Responds to the server with specific content attached to the message.
    /// </summary>
    /// <param name="content">Content to be attached to the message.</param>
    public void ClientRespond(string? content)
    {
        if (NetworkPipes.IsClient) NetworkPipes.Messages.Response(code, content);
        else throw new InvalidOperationException("Cannot use simplified respond method on server side.");
    }





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                              Implementations
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public override string ToString() => code.ToString();
    public override int GetHashCode() => code.GetHashCode();
    public override bool Equals(object? obj)
    {
        return obj is ResponseID other && Equals(code, other.code);
    }





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                                 Operators
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public static bool operator ==(ResponseID left, ResponseID right) => left.code == right.code;
    public static bool operator !=(ResponseID left, ResponseID right) => left.code != right.code;
    public static implicit operator ushort(ResponseID response) => response.code;
    public static implicit operator ResponseID(ushort code) => new(code);





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                               Private Fields
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    private static readonly char[] toHex = [
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
        'A', 'B', 'C', 'D', 'E', 'F',
    ];

    private static readonly int[] toInt = [
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, // First 47 are not HEX - ignored.

        // Values from '0' to '9'
        0, 1, 2, 3, 4, 5, 6, 7, 8, 9,

        0, 0, 0, 0, 0, 0, 0, // Next 7 also not HEX - ignored.

        // Values from 'A' to 'F'.
        10, 11, 12, 13, 14, 15,

        // Going outside of the bounds will cause an error - intended.
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, // Next 27 not HEX either - ignored.

        // Values from 'a' to 'f'.
        // We mad them as well, just in case.
        10, 11, 12, 13, 14, 15,
    ];





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                               Static Methods
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public static string Format(ResponseID id)
    {
        int code = id.code & 0xffff;
        int value1 = code & 0x000f;
        int value2 = code & 0x00f0;
        int value3 = code & 0x0f00;
        int value4 = code & 0xf000;
        value2 >>= 4;
        value3 >>= 8;
        value4 >>= 12;

        return new string((char[])[
            toHex[value1],
            toHex[value2],
            toHex[value3],
            toHex[value4],
        ]);
    }

    /// <summary>
    /// Formats part of the string as <see cref="ResponseID"/>.
    /// </summary>
    /// <param name="at">Index from which start.</param>
    public static ResponseID Format(string str, int at)
    {
        char hex1 = str[at];
        char hex2 = str[at + 1];
        char hex3 = str[at + 2];
        char hex4 = str[at + 3];
        int value1 = toInt[hex1];
        int value2 = toInt[hex2];
        int value3 = toInt[hex3];
        int value4 = toInt[hex4];
        value2 <<= 4;
        value3 <<= 8;
        value4 <<= 12;
        return new ResponseID((ushort)(value1 | value2 | value3 | value4));
    }

    public static ResponseID Format(char hex1, char hex2, char hex3, char hex4)
    {
        int value1 = toInt[hex1];
        int value2 = toInt[hex2];
        int value3 = toInt[hex3];
        int value4 = toInt[hex4];
        value2 <<= 4;
        value3 <<= 8;
        value4 <<= 12;
        return new ResponseID((ushort)(value1 | value2 | value3 | value4));
    }

    // Same as the one above, but with integers.
    public static ResponseID Format(int hex1, int hex2, int hex3, int hex4)
    {
        int value1 = toInt[hex1];
        int value2 = toInt[hex2];
        int value3 = toInt[hex3];
        int value4 = toInt[hex4];
        value2 <<= 4;
        value3 <<= 8;
        value4 <<= 12;
        return new ResponseID((ushort)(value1 | value2 | value3 | value4));
    }





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                                ID Building
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    /// <summary>
    /// Builder, that is responsible for ID building.
    /// </summary>
    /// <remarks>
    /// Was made instantiable, to allow having multiple of them on the server for each pipe.
    /// </remarks>
    public sealed class ResponseIDBuilder
    {
        /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
        /// .
        /// .                                               Private Fields
        /// .
        /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
        private ushort iterator = 0;





        /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
        /// .
        /// .                                               Public Methods
        /// .
        /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
        public ResponseID GetNext()
        {
            iterator++;
            // Avoids '0' index.
            // And also avoids '1' index, for testing messages.
            if (iterator < 2) iterator = 2;
            return iterator; // SHOULD overflow.
        }
    }
}
