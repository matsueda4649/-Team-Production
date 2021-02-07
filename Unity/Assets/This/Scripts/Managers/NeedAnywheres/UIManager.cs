using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using WindowSystem;

namespace ManagerSystem
{
    /// <summary>
    /// UIの管理
    /// </summary>
    public class UIManager : SingletonMonoBehaviour<UIManager>
    {
        #region 変数宣言

        /// <summary>
        /// フェード用のCanvas
        /// </summary>
        [SerializeField] Canvas m_fadeCanvas = default;

        /// <summary>
        /// Fade用のキャンバス
        /// </summary>
        public Canvas GetFadeCanvas { get => m_fadeCanvas; }

        /// <summary>
        /// フェード画像
        /// </summary>
        [SerializeField] Image m_fadeImage = default;

        /// <summary>
        /// 二択のダイヤログ
        /// </summary>
        [SerializeField] ChoicesDialogWindow m_choicesDialog = default;

        /// <summary>
        /// Yesのみのダイヤログ
        /// </summary>
        [SerializeField] ChoicesDialogWindow m_yesDialog = default;

        /// <summary>
        /// スコア表示用のダイヤログ
        /// </summary>
        [SerializeField] RecordDialog m_recordDialg = default;

        /// <summary>
        /// フェードイン
        /// </summary>
        private Tween m_fadeIn  = null;

        /// <summary>
        /// フェードアウト
        /// </summary>
        private Tween m_fadeOut = null;

        #endregion

        #region メソッド

        /// <summary>
        /// フェードイン
        /// </summary>
        public void FadeIn()
        {
            m_fadeCanvas.enabled = false;
        }

        /// <summary>
        /// フェードイン
        /// </summary>
        /// <param name="duration">時間        </param>
        /// <param name="callback">コールバック</param>
        public void FadeIn(float duration, Action callback)
        {
            m_fadeIn = ExtensionDOTween.FadeIn(m_fadeCanvas, m_fadeImage, duration, () => {
                callback?.Invoke();
            });
        }

        /// <summary>
        /// フェードアウト
        /// </summary>
        /// <param name="duration">時間        </param>
        /// <param name="callback">コールバック</param>
        public void FadeOut(float duration = 0, Action callback = null)
        {
            m_fadeOut = ExtensionDOTween.FadeOut(m_fadeCanvas, m_fadeImage, duration, () => {
                callback?.Invoke();
            });
        }

        /// <summary>
        /// 二択のダイヤログ
        /// </summary>
        /// <param name="sortingOrder"> ソーティングレイヤーのオーダー順    </param>
        /// <param name="message">      メッセージ                          </param>
        /// <param name="yesText">      Yesボタンのテキスト                 </param>
        /// <param name="noText">       Noボタン のテキスト                 </param>
        /// <param name="yesCallback">  Yesボタンのクリック時のコールバック </param>
        /// <param name="noCallback">   Noボタン のクリック時のコールバック </param>
        public ChoicesDialogWindow ShowChoiceDialog(int sortingOrder, string message, string yesText, string noText,
                                     Action yesCallback, Action noCallback)
        {
            // ダイヤログの生成
            return CreateDialog(sortingOrder, message, yesText, noText, yesCallback, noCallback, m_choicesDialog);
        }

        /// <summary>
        /// 一択のダイヤログ
        /// </summary>
        /// <param name="sortingOrder"> ソーティングレイヤーのオーダー順    </param>
        /// <param name="message">      メッセージ                          </param>
        /// <param name="yesText">      Yesボタンのテキスト                 </param>
        /// <param name="yesCallback">  Yesボタンのクリック時のコールバック </param>
        public ChoicesDialogWindow ShowYesDialog(int sortingOrder, string message, string yesText, Action yesCallback)
        {
            // ダイヤログの生成
            return CreateDialog(sortingOrder, message, yesText, "", yesCallback, null, m_yesDialog);
        }

        /// <summary>
        /// ダイヤログの生成
        /// </summary>
        /// <param name="sortingOrder"> ソーティングレイヤーのオーダー順    </param>
        /// <param name="message">      メッセージ                          </param>
        /// <param name="yesText">      Yesボタンのテキスト                 </param>
        /// <param name="noText">       Noボタン のテキスト                 </param>
        /// <param name="yesCallback">  Yesボタンのクリック時のコールバック </param>
        /// <param name="noCallback">   Noボタン のクリック時のコールバック </param>
        /// <param name="dialogObject"> 生成するダイヤログ                  </param>
        private ChoicesDialogWindow CreateDialog(int sortingOrder, string message, string yesText, string noText, Action yesCallback,
                                  Action noCallback, ChoicesDialogWindow dialogObject)
        {

            if (!dialogObject.IsNull())
            {
                var dialog = CreateBaseDialog(dialogObject, this.transform);

                // ダイヤログを初期化する
                dialog.Initialize(sortingOrder, message, yesText, noText, yesCallback, noCallback);

                return dialog;
            }

            return null;
        }

        /// <summary>
        /// Record用のダイヤログを表示
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="newRecord">新記録更新判定</param>
        /// <param name="clearTime">クリア時間</param>
        public RecordDialog ShowRecordDialg(string message, bool newRecord, float clearTime)
        {
            return CreateRecordDialg(message, newRecord, clearTime);
        }

        /// <summary>
        /// Record用のダイヤログ生成
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="newRecord">新記録更新判定</param>
        /// <param name="clearTime">クリア時間</param>
        private RecordDialog CreateRecordDialg(string message, bool newRecord, float clearTime)
        {
            if (!m_recordDialg.IsNull())
            {
                var recordDialg = CreateBaseDialog(m_recordDialg, this.transform);

                // ダイヤログを初期化する
                recordDialg.Initialize(message, newRecord, clearTime, ()=> {
                    // Closeを押したら

                    // 設定したシーンに戻る
                    GameSceneManager.Instance.LoadScene("StageSelectScene").Forget();
                });

                return recordDialg;
            }


            return null;
        }

        /// <summary>
        /// ダイヤログの生成
        /// </summary>
        /// <param name="dialog">生成するダイヤログ</param>
        /// <param name="parent">生成する場所</param>
        /// <param name="initialize">初期化</param>
        /// <returns></returns>
        public static T CreateBaseDialog<T>(T dialog, Transform parent) where T : BaseDialog
        {
            T baseDialog = null;

            if (!dialog.IsNull())
            {
                // ダイヤログを生成
                baseDialog = Instantiate(dialog, parent);
                // 座標設定
                baseDialog.transform.localPosition = Vector3.zero;
                // 大きさ設定
                baseDialog.transform.localScale = Vector3.one;
            }

            return baseDialog;
        }

        private void OnDisable()
        {
            ExtensionDOTween.Kill(m_fadeIn);
            ExtensionDOTween.Kill(m_fadeOut);
        }

        #endregion
    }
}