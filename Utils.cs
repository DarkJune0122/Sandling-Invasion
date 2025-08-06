using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SandlingInvasion;

public static class Utils
{
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                               Static Methods
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public static T Find<T>(ICollection<T> collection, Predicate<T> predicate)
    {
        foreach (T item in collection)
        {
            if (predicate(item)) return item;
        }

        return default;
    }

    public static bool TryFind<T>(ICollection<T> collection, Predicate<T> predicate, out T result)
    {
        foreach (T item in collection)
        {
            if (predicate(item))
            {
                result = item;
                return true;
            }
        }

        result = default;
        return false;
    }
}
