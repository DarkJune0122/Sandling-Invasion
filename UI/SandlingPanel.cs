using Alexandria.ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SandlingInvasion.UI;

/// <summary>
/// Panel that shows active sandlings.
/// </summary>
public sealed class SandlingPanel
{
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                               Private Fields
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    private tk2dUISpriteAnimator sandling;





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                                Constructors
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public SandlingPanel()
    {
        GameObject ui = new("Sandling Panel");
        var anchor = GameUIRoot.Instance.transform;
        ui.transform.SetParent(anchor, false);

        GameObject animator = new("Invading Sandling");
        sandling = ui.AddComponent<tk2dUISpriteAnimator>();

        tk2dSpriteAnimationClip clip = new()
        {
            name = "sandling_ui_idle",
            fps = 12,
            wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop,
        };

        const int Frames = 4;
        const string SpritesPath = $"{nameof(SandlingInvasion)}/Resources/dog_item";
        var collection = GetCollection(SpritesPath, Frames);
        var frames = new tk2dSpriteAnimationFrame[Frames];
        for (int i = 0; i < Frames; i++)
        {
            frames[i] = new tk2dSpriteAnimationFrame { spriteCollection = collection, spriteId = i };
        }

        var animation = new tk2dSpriteAnimation()
        {
            clips = [clip]
        };

        // TODO: Finish.
    }

    private tk2dSpriteCollectionData GetCollection(string name, int frames)
    {
        Texture2D[] textures = new Texture2D[frames];
        for (int i = 0; i < frames; i++)
        {
            textures[i] = ResourceExtractor.GetTextureFromFile($"{name}_{i:000}");
        }

        return Utils.NewCollection("SandlingUIJam", textures);
    }





    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                               Public Methods
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public void Render()
    {

    }
}
