using Cysharp.Threading.Tasks;
using ManagerSystem;
using TMPro;
using UnityEngine;

namespace UISystem 
{
    /// <summary>
    /// ノードの共通データ
    /// </summary>
    public abstract class BaseNode : MonoBehaviour
    {
        #region 変数宣言

        [SerializeField] TextMeshProUGUI m_title = default;

        /// <summary>
        /// タイトル
        /// </summary>
        public TextMeshProUGUI GetTitle { get => m_title; }

        #endregion

        #region メソッド

        /// <summary>
        /// ノードを選択する
        /// </summary>
        public virtual void OnClickNode()
        {
            // SEを再生
            AudioManager.Instance.PlaySe(AudioSe.SelectStage);
        }

        #endregion
    }
}