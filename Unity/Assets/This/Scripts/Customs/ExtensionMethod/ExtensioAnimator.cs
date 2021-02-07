using UnityEngine;

/// <summary>
/// Animatorの拡張メソッド
/// </summary>
public static class ExtensionAnimator
{
    /// <summary>
    /// アニメーション再生終了判定
    /// </summary>
    /// <param name="layerIndex">インデックス</param>
    /// <returns></returns>
    public static bool IsEnd(this Animator animator, int layerIndex = 0)
    {
        //オブジェクトがアクティブではない時
        if (!animator.gameObject.activeInHierarchy) { return true; }

        var animatorInfo = animator.GetCurrentAnimatorStateInfo(layerIndex);

        return animatorInfo.normalizedTime >= 1;
    }
}
