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





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                                     UI
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public static tk2dSpriteCollectionData NewCollection(string name, Texture2D[] textures, float ppt = 16f)
    {
        // Create GameObject for the collection
        var go = new GameObject(name);
        UnityEngine.Object.DontDestroyOnLoad(go);

        var collection = go.AddComponent<tk2dSpriteCollectionData>();
        collection.spriteCollectionName = name;
        collection.textures = textures;
        collection.materials = new Material[textures.Length];
        collection.materialInsts = new Material[textures.Length];
        collection.spriteDefinitions = new tk2dSpriteDefinition[textures.Length];

        Shader shader = Shader.Find("tk2d/BlendVertexColor");
        for (int i = 0; i < textures.Length; i++)
        {
            Texture2D tex = textures[i];

            // Create a material for each texture.
            Material mat = new(shader)
            {
                mainTexture = tex
            };

            collection.materials[i] = mat;
            collection.materialInsts[i] = mat;

            // Create sprite definition
            float xHalf = tex.width / (ppt * 2f);
            float yHalf = tex.height / (ppt * 2f);
            float xScale = tex.width / ppt;
            float yScale = tex.height / ppt;
            tk2dSpriteDefinition def = new()
            {
                name = tex.name,
                material = mat,
                materialInst = mat,
                position0 = new(0, 0, 0),
                position1 = new(xScale, 0, 0),
                position2 = new(xScale, yScale, 0),
                position3 = new(0, yScale, 0),
                uvs =
                [
                    new(0, 0),
                    new(1, 0),
                    new(1, 1),
                    new(0, 1)
                ],

                boundsDataCenter = new Vector3(xHalf, yHalf, 0f),
                boundsDataExtents = new Vector3(xScale, yScale, 0f),
                untrimmedBoundsDataCenter = new Vector3(xHalf, yHalf, 0f),
                untrimmedBoundsDataExtents = new Vector3(xScale, yScale, 0f),
                texelSize = new Vector2(1.0f / tex.width, 1.0f / tex.height)
            };

            collection.spriteDefinitions[i] = def;
        }

        return collection;
    }
}
