using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

/// <summary>
/// DOTweenの拡張メソッド
/// </summary>
public static class ExtensionDOTween
{
    #region 変数宣言

    /// <summary>
    /// 黒色
    /// </summary>
    public static readonly Color BLACK = Color.black;

    /// <summary>
    /// 透明
    /// </summary>
    public static readonly Color TRANSPARENT = new Color(0f, 0f, 0f, 0f);

    #endregion

    /// <summary>
    /// Kill  ( List )
    /// </summary>
    /// <param name="tweenList"></param>
    public static void Kill(IList<Tween> tweenList)
    {
        if (!DOTween.instance.IsNull())
        {
            for(int i = 0, count = tweenList.Count; i < count; ++i)
            {
                if(tweenList[i] != null){ tweenList[i].Kill(); }
            }
        }
    }

    /// <summary>
    /// Kill
    /// </summary>
    /// <param name="tweenList"></param>
    public static void Kill(Tween tween)
    {
        if (!DOTween.instance.IsNull())
        {
            if (tween != null) { tween.Kill(); }
        }
    }

    /// <summary>
    /// X座標を動かす
    /// </summary>
    /// <param name="rect">対象</param>
    /// <param name="endValue">目標座標</param>
    /// <param name="duration">時間</param>
    /// <param name="loopNum">ループ回数</param>
    public static Tween DOLocalMoveX(Transform rect, float endValue, float duration, Action callback = null)
    {
        return rect.DOLocalMoveX(endValue, duration)
                   .SetEase(Ease.Linear)
                   .OnComplete(()=> { callback?.Invoke(); });
    }

    /// <summary>
    /// 360度回転
    /// </summary>
    /// <param name="rect">対象</param>
    /// <param name="endValue">目標座標</param>
    /// <param name="duration">時間</param>
    public static Tween RotateFull(Transform rect, Vector3 endValue, float duration)
    {
        return rect.DOLocalRotate(endValue, duration, RotateMode.FastBeyond360)
                   .SetEase(Ease.Linear);;
    }

    /// <summary>
    /// 移動しながらジャンプさせる
    /// </summary>
    /// <param name="rect">対象</param>
    /// <param name="endValue">目標座標</param>
    /// <param name="jumpPower">ジャンプ力</param>
    /// <param name="numJumps">ジャンプ回数</param>
    /// <param name="duration">時間</param>
    public static Tween DOLocalJump(Transform rect, RectTransform endValue,float jumpPower, int numJumps, float duration)
    {
        return rect.DOLocalJump(endValue.localPosition, jumpPower, numJumps, duration)
                   .SetEase(Ease.Linear);
    }

    /// <summary>
    /// 垂直方向にはねさせる
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="endValue"></param>
    /// <param name="duration"></param>
    public static Tween Bounce(Transform rect, RectTransform endValue, float duration)
    {
        return rect.DOLocalMoveY(endValue.localPosition.y, duration)
                   .SetEase(Ease.OutBounce);
    }

    /// <summary>
    /// フェードイン
    /// </summary>
    /// <param name="canvs">キャンバス</param>
    /// <param name="image">フェード画像</param>
    /// <param name="duration">フェード時間</param>
    /// <param name="callback">コールバック</param>
    /// <returns></returns>
    public static Tween FadeIn(Canvas canvs, Image image, float duration, Action callback)
    {
        canvs.enabled = true;

        return image.DOColor(TRANSPARENT, duration).OnComplete(() => {

            callback?.Invoke();

            // キャンバスを非表示にする
            canvs.enabled = false;
        });
    }

    /// <summary>
    /// フェードアウト
    /// </summary>
    /// <param name="image">画像</param>
    /// <param name="duration">時間</param>
    /// <param name="callback">コールバック</param>
    public static Tween FadeOut(Canvas canvs, Image image, float duration, Action callback)
    {
        //キャンバスを表示
        canvs.enabled = true;

        // フェードイン
        return image.DOColor(BLACK, duration).OnComplete(() => { callback?.Invoke(); });
    }

    /// <summary>
    /// ボタンを強調させるアニメーション
    /// </summary>
    /// <param name="button">ボタン</param>
    /// <param name="endValue">目標の大きさ</param>
    /// <param name="duration">時間</param>
    public static Tween EmphasizeButton(Transform button, float endValue, float duration)
    {
        return button.DOScale(endValue, duration)
                     .SetRecyclable(true)
                     .SetEase(Ease.OutQuart)
                     .SetLoops(-1, LoopType.Restart);
    }

    /// <summary>
    /// 回転
    /// </summary>
    /// <param name="rect">ターゲット</param>
    /// <param name="endValue">目的座標</param>
    /// <param name="duration">時間</param>
    /// <returns></returns>
    public static Tween DOLocalRotate(Transform rect, Vector3 endValue, float duration, Action callback = null)
    {
        return rect.DOLocalRotate(endValue, duration)
                   .SetEase(Ease.Linear)
                   .OnComplete(()=> { callback?.Invoke(); });
    }

    /// <summary>
    /// サイズ変更
    /// </summary>
    /// <param name="rect">ターゲット</param>
    /// <param name="endValue">目標サイズ</param>
    /// <param name="duration">時間</param>
    /// <param name="callback">コールバック</param>
    /// <param name="ease">イージング</param>
    /// <returns></returns>
    public static Tween DOScale(Transform rect, Vector3 endValue, float duration, Action callback, Ease ease = Ease.Linear)
    {
        return rect.DOScale(endValue, duration)
                   .OnComplete(() => callback?.Invoke())
                   .SetEase(ease);
    }

    /// <summary>
    /// カメラ回転
    /// </summary>
    /// <param name="rect">ターゲット</param>
    /// <param name="endValue">目標サイズ</param>
    /// <param name="moveDuration">移動時間</param>
    /// <param name="rotaionDuration">回転時間</param>
    /// <param name="callback">アニメーション終了後のコールバック</param>
    public static Tween GoDownRotation(Transform rect, Vector3 endValue,float moveDuration, float rotaionDuration, Action callback)
    {
        var backEndValue = new Vector3(0f, 0f,  -5f);

        Tween tween = null;

        tween = rect.parent.DOLocalMove(backEndValue, moveDuration).OnComplete(()=> {

            tween = rect.DORotate(endValue, rotaionDuration)
                        .SetEase(Ease.Linear)
                        .OnComplete(() =>{
                            tween = rect.parent.DOLocalMove(Vector3.zero, moveDuration)
                                               .OnComplete(()=> { callback?.Invoke(); });
                        });
        });

        return tween;
    }

    /// <summary>
    /// 左右に揺れる
    /// </summary>
    /// <param name="x">移動するX座標</param>
    /// <returns></returns>
    public static Tween DOPunchPositionX(Transform transform, float x, Action callback)
    {
        // 移動ベクトルの範囲
        var punch = new Vector2(x, 0f);

        // 揺れる時間
        var duration = 0.7f;

        // 揺れる回数
        var vibrato = 10;

        // 弾力
        var elasticity = 1f;

        // 値を整数に変更するかどうか
        var snapping = false;

        return transform.DOPunchPosition(punch, duration, vibrato, elasticity, snapping)
            .OnComplete(()=> { callback?.Invoke(); })
            .SetEase(Ease.Linear);
    }

    /// <summary>
    /// ジャンプアニメーション
    /// </summary>
    /// <param name="y">高さ</param>
    /// <param name="duraiton">ジャンプする時間</param>
    /// <returns></returns>
    public static Tween JumpAnimation(Transform rect, float y, float duraiton)
    {
       return rect.DOLocalMoveY(y, duraiton)
            .SetRelative(true)
            .SetEase(Ease.OutQuad)
            .SetLoops(-1, LoopType.Yoyo);
    }

    /// <summary>
    /// NewRecordアニメーション
    /// </summary>
    /// <returns></returns>
    public static Tween NewRecordAnimation(Transform rect, float endValue, float duration)
    {
        return rect.DOLocalMoveY(endValue, duration)
            .SetRelative(true)
            .SetEase(Ease.OutQuad)
            .SetLoops(-1, LoopType.Yoyo);
    }
}
