#nullable enable
/// <summary>
/// Standalone pipe name.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1050:Declare types in namespaces", Justification = "For easier distribution.")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "To seal the one above")]
public readonly struct PipeName
{
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                              Static Properties
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public static readonly PipeName Invalid = default;





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                               Public Fields
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public readonly string name;





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                                Constructors
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    private PipeName(string name) => this.name = name;
    static PipeName()
    {
        // This is to ensure that the static constructor is called before any instance is created.
        // Needed to avoid TypeInitializationExceptions.
    }





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                              Implementations
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public override string ToString() => name;
    public override int GetHashCode() => name?.GetHashCode() ?? 0;
    public override bool Equals(object? obj)
    {
        return obj is PipeName other && string.Equals(name, other.name);
    }





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                                 Operators
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public static bool operator ==(PipeName left, PipeName right) => left.name == right.name;
    public static bool operator !=(PipeName left, PipeName right) => left.name != right.name;
    public static implicit operator string(PipeName pipe) => pipe.name;
    public static implicit operator PipeName(string name) => new(name);
}
