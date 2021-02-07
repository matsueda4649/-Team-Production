using ManagerSystem;
using UnityEngine;

namespace WindowSystem
{
    /// <summary>
    /// 操作説明のウインドウを管理
    /// </summary>
    public class OperationExplanationWindow : BaseDialog
    {
        #region 変数宣言

        [SerializeField] GameObject[] m_windowArray = new GameObject[2];

        #endregion

        #region メソッド

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="sortingOrder"> ソーティングレイヤーのオーダー順 </param>
        public void Initialize(int sortingOrder)
        {
            Initialize(sortingOrder, "");

            Open();
        }

        /// <summary>
        /// Closeボタンのクリック時
        /// </summary>
        public void OnClickCloseButton()
        {
            // ポップ中は受け付けない
            if (!PopFlag)
            {
                // SEを再生
                AudioManager.Instance.PlaySe(AudioSe.Close);

                Close();
            }
        }

        /// <summary>
        /// Lefttボタンのクリック時
        /// </summary>
        public void OnClickLeftButton()
        {
            AudioManager.Instance.PlaySe(AudioSe.Decision);
            m_windowArray[1].Deactovation();
            m_windowArray[0].Activation();
        }

        /// <summary>
        /// Rightボタンのクリック時
        /// </summary>
        public void OnClickRightButton()
        {
            AudioManager.Instance.PlaySe(AudioSe.Decision);
            m_windowArray[0].Deactovation();
            m_windowArray[1].Activation();
        }

        #endregion
    }
}