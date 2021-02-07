using UnityEngine;
using UnityEngine.UI;

public static class ExtensioSprite 
{
    /// <summary>
    /// Spriteを変更する
    /// </summary>
    /// <param name="before">変更前</param>
    /// <param name="after"> 変更後</param>
    public static void ChangeSprite(this Sprite before, Sprite after)
    {
        before = null;
        before = after;
    }

    /// <summary>
    /// Spriteを変更する
    /// </summary>
    /// <param name="before">変更前</param>
    /// <param name="after"> 変更後</param>
    public static void ChangeSprite(this Image before, Sprite after)
    {
        before.sprite = null;
        before.sprite = after;
    }

    /// <summary>
    /// Spriteを変更する
    /// </summary>
    /// <param name="before">変更前</param>
    /// <param name="after"> 変更後</param>
    public static void ChangeSprite(this SpriteRenderer before, Sprite after)
    {
        before.sprite = null;
        before.sprite = after;
    }
}
