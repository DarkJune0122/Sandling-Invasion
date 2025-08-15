using System;
using System.Collections.Generic;
using UnityEngine;

namespace SandlingInvasion;

public static class Utils
{
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                                Constructors
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public static void Initialize()
    {
        // Nothing. Runs a constructor below.
    }

    static Utils()
    {
        CreateOffsets();
    }





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





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                                   World
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    /// <summary>
    /// Offsets, sorted by distance.
    /// </summary>
    private static IntVector2[] offsets;
    private static void CreateOffsets()
    {
        const int Size = 7; // Should be odd: 1, 3, 5, 7, 9, etc.
        const int Side = Size / 2;

        // Offsets for a 5x5 block centered at (0,0), sorted by distance from center
        offsets = new IntVector2[Size * Size];
        int index = 0;
        for (int dy = -Side; dy <= Side; dy++)
        {
            for (int dx = -Side; dx <= Side; dx++)
            {
                offsets[index++] = new IntVector2(dx, dy);
            }
        }

        // Sort by Manhattan distance, then by Euclidean distance for tie-breaking
        Array.Sort(offsets, (a, b) =>
        {
            int distanceA = Mathf.Abs(a.x) + Mathf.Abs(a.y);
            int distanceB = Mathf.Abs(b.x) + Mathf.Abs(b.y);
            if (distanceA != distanceB)
                return distanceA.CompareTo(distanceB);

            float sqrA = a.x * a.x + a.y * a.y;
            float sqrB = b.x * b.x + b.y * b.y;
            return sqrA.CompareTo(sqrB);
        });
    }

    /// <summary>
    /// Tries to find a closest valid position within 7x7 tile radius, with <paramref name="source"/> being in the middle.
    /// </summary>
    public static bool TryFindClosest(IntVector2 source, Func<IntVector2, bool> validator, out IntVector2 cell)
    {
        if (validator == null)
        {
            cell = source;
            return false;
        }

        for (int i = 0; i < offsets.Length; i++)
        {
            IntVector2 offset = offsets[i];
            int xPos = source.x + offset.x;
            int yPos = source.y + offset.y;
            IntVector2 position = new(xPos, yPos);
            if (validator(position))
            {
                cell = position;
                return true;
            }
        }

        cell = source;
        return false;
    }
}
