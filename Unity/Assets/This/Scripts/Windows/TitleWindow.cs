using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using static ExtensionDOTween;

namespace WindowSystem
{
    /// <summary>
    /// DOTweenウインドウ管理
    /// </summary>
    public class TitleWindow : MonoBehaviour
    {
        #region 変数宣言

        /// <summary>
        /// DOTweenを適用するターゲット
        /// </summary>
        [SerializeField] RectTransform[] m_targetArray = default;

        /// <summary>
        /// 目標座標
        /// </summary>
        [SerializeField] RectTransform[] m_endValueArray = default;

        /// <summary>
        /// DOTweenのリスト
        /// </summary>
        private IList<Tween> m_tweenList = new List<Tween>(3);

        #endregion

        #region メソッド

        /// <summary>
        /// タイトルウインドウを表示
        /// </summary>
        /// <param name="first">インデックス</param>
        public void ShowTitleWindow(int first = 0, int second = 1)
        {
            // タイトルウィンドウをはねさせてから
            BounceTitle(first);

            float waitTime = 1.0f;

            DOVirtual.DelayedCall(
                waitTime, // 1秒待機させる
                () =>
                {
                    if(m_targetArray[second].IsNull()) { return; }

                    // タイトルのトップをはねさせる
                    JumpTopTitleText(second);
                });
        }

        /// <summary>
        /// タイトルテキストをジャンプさせる
        /// </summary>
        /// <param name="index">インデックス</param>
        private void JumpTopTitleText(int index = 0)
        {
            // 跳ねる強さ
            float jumpPower = 50f;

            // ジャンプする回数
            int numJumps = 5;

            // アニメーションの時間
            float duration = 2.5f;

            m_tweenList.Add(DOLocalJump(m_targetArray[index], m_endValueArray[index], jumpPower, numJumps, duration));
        }

        /// <summary>
        /// タイトルウィンドウをはねさせる
        /// </summary>
        /// <param name="index">インデックス</param>
        private void BounceTitle(int index = 0)
        {
            // アニメーションの時間
            float duration = 1f;

            m_tweenList.Add(Bounce(m_targetArray[index], m_endValueArray[index], duration));
        }

        /// <summary>
        /// オブジェクトを回転させる
        /// </summary>
        /// <param name="obj">オブジェクト</param>
        public void RotateObject(RectTransform obj)
        {
            // 360°回転
            Vector3 endValue = new Vector3(0f, 0f, 360f);

            float duration = 10f;

            m_tweenList.Add(RotateFull(obj, endValue, duration));
        }

        private void OnDisable()
        {
            Kill(m_tweenList);
        }

        #endregion
    }
}