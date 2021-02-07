using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static DefTime;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using DG.Tweening;

namespace ManagerSystem
{
    /// <summary>
    /// シーン遷移の管理
    /// </summary>
    public class GameSceneManager : SingletonMonoBehaviour<GameSceneManager>
    {
        #region 変数宣言

        /// <summary>
        /// 読み込んだシーン
        /// </summary>
        private BaseScene m_loadedScene;

        /// <summary>
        /// ロード用のスライダー
        /// </summary>
        [SerializeField] Slider m_loadingSlider = default;

        #endregion

        #region メソッド

        /// <summary>
        /// 初期化
        /// </summary>
        /// <returns></returns>
        public async UniTask Initialize()
        {
            await UniTask.Delay(1);
        }

        /// <summary>
        /// 読み込んだシーンの設定
        /// </summary>
        /// <param name="loadedScene">読み込んだシーン</param>
        public void SetLoadedScene(BaseScene loadedScene)
        {
            m_loadedScene = loadedScene;
        }

        /// <summary>
        /// シーンの遷移
        /// </summary>
        /// <param name="loadSceneName"> 読み込むシーン名 </param>
        public async UniTask LoadScene(string loadSceneName)
        {
            await FadeOut();

            // 読み込んでいるシーンを破棄する
            if (!m_loadedScene.IsNull())
            {
                await m_loadedScene.TerminateScene();
                await SceneManager.UnloadSceneAsync(m_loadedScene.GetSceneName());
                m_loadedScene = null;
            }

            m_loadingSlider.SetActive(true);

            //// シーンの読み込み
            var async = SceneManager.LoadSceneAsync(loadSceneName, LoadSceneMode.Additive);

            //　コルーチンを開始
            await LoadData(m_loadingSlider, async);

            // シーン読み込みまで待機
            await UniTask.WaitUntil(()=> {
                var scene = SceneManager.GetSceneByName(loadSceneName);
                return scene.isLoaded;
            });

            // 読み込んだシーンの設定を待つ
            await UniTask.WaitUntil(()=> {
                return !m_loadedScene.IsNull();
            });

            m_loadingSlider.SetActive(!async.isDone);

            // 読み込んだシーンの初期化
            await m_loadedScene.Initialize();

            await FadeIn();

            // ロードしたシーンを開始する
            await m_loadedScene.StartScene();

            System.GC.Collect();
            await Resources.UnloadUnusedAssets();
        }

        /// <summary>
        /// データのロード
        /// </summary>
        /// <param name="loadingSlider">ロード用のスライダー</param>
        /// <param name="async"></param>
        /// <returns></returns>
        private static async UniTask LoadData(Slider loadingSlider, AsyncOperation async)
        {
            //　読み込みが終わるまで進捗状況をスライダーの値に反映させる
            while (!async.isDone)
            {
                var progressVal = Mathf.Clamp01(async.progress / 0.9f);
                loadingSlider.value = progressVal;
                await UniTask.Yield(PlayerLoopTiming.Update);
            }
        }
        /// <summary>
        /// フェードアウト
        /// </summary>
        private static async UniTask FadeOut()
        {
            // フェードアウト開始
            bool isFadeOut = false;

            UIManager.Instance.FadeOut(FADE_OUT_TIME, () => {
                isFadeOut = true;
            });

            for (; !isFadeOut;) { await UniTask.Yield(PlayerLoopTiming.Update); }

            // 待機
            await UniTask.Delay(DELAY_TIME);
        }

        /// <summary>
        /// フェードイン
        /// </summary>
        private static async UniTask FadeIn()
        {
            // フェードイン開始
            bool isFadeIn = false;

            UIManager.Instance.FadeIn(FADE_IN_TIME, () => {
                isFadeIn = true;
            });

            for (; !isFadeIn;) { await UniTask.Yield(PlayerLoopTiming.Update); }
        }

        #endregion
    }
}