using ManagerSystem;
using UnityEngine;

namespace WindowSystem
{
    /// <summary>
    /// オプションウインドウ
    /// </summary>
    public class OptionWindow : MonoBehaviour
    {
        #region メソッド

        /// <summary>
        /// ボタンクリック時オプション画面を表示
        /// </summary>
        public void OnClickOptionButton()
        {
            AudioManager.Instance.PlaySe(AudioSe.Decision);
            OptionManager.Instance.OpenTopWindow();
        }

        #endregion
    }
}