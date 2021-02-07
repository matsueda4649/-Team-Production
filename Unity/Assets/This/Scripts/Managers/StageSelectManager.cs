using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using WindowSystem;
using static DefNum;

namespace ManagerSystem
{
    /// <summary>
    /// ステージ選択画面を管理
    /// </summary>
    public class StageSelectManager : BaseScene, IUpdatable
    {
        public static readonly string SceneName = "StageSelectScene";

        #region 変数宣言

        /// <summary>
        /// スクロールウインドウ
        /// </summary>
        [SerializeField] RectTransform m_scrollWindow = default;

        /// <summary>
        /// スクロールビュー
        /// </summary>
        [SerializeField] ScrollView m_scrollVeiw = default;

        /// <summary>
        /// スクロールビューのリスト
        /// </summary>
        private IList<ScrollView> m_scrollVeiwList = new List<ScrollView>();

        /// <summary>
        /// スクロールビューの個数
        /// </summary>
        private int m_scrollVeiwCount = 0;

        /// <summary>
        /// ステージデータ
        /// </summary>
        private DataStages m_dataStages;

        /// <summary>
        /// セーブデータ
        /// </summary>
        private SaveData m_saveData;

        /// <summary>
        /// 選択しているスクロールビューのインデックス
        /// </summary>
        private int m_selectIndex = 0;

        /// <summary>
        /// 選択していたスクロールビューのインデックス
        /// </summary>
        private int m_selectedIndex = 0;

        /// <summary>
        /// 左ボタン
        /// </summary>
        [SerializeField] Button m_leftButton = default;

        /// <summary>
        /// 右ボタン
        /// </summary>
        [SerializeField] Button m_rightButton = default;

        /// <summary>
        /// 難易度設定のトグル
        /// </summary>
        [SerializeField] Toggle m_difficultyToggle = default;

        /// <summary>
        /// ボタンクリックの許可
        /// </summary>
        private bool m_allowOnClickButton = false;

        #endregion

        #region メソッド

        public override string GetSceneName()
        {
            return SceneName;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public override async UniTask Initialize()
        {
            // 必要なデータを取得
            m_dataStages = DataManager.Instance.GetDataStages;
            m_saveData = SaveManager.Instance.GetSaveData;

            // データの再読み込み( 必要なら )
            await ReloadData();

            // スクロールビューの生成
            CreateScrollView();

            m_selectIndex = m_saveData.questId / STAGE_INIT_NUMBER - 1;
            m_selectIndex = Mathf.Clamp(m_selectIndex, 0, m_scrollVeiwCount - 1);

            // スクロールビューの更新
            StartCoroutine(IUpdateScrollView());

            // BGMを再生する
            AudioManager.Instance.PlayBgm(AudioBgm.StageSelect);

            m_difficultyToggle.isOn = m_saveData.easyMode;

            await UniTask.Delay(DefTime.DELAY_TIME);

            m_allowOnClickButton = true;
        }

        public void UpdateMe()
        {
            if (UIManager.Instance.GetFadeCanvas.enabled) { return; }

            if (!m_allowOnClickButton) { return; }

            DebugManager.ReturnToTitle();
        }

        /// <summary>
        /// データの再読み込み
        /// </summary>
        private async UniTask ReloadData()
        {
            // データが読み込めなければ
            if (m_dataStages.IsNull())
            {
                // DataManagerを初期化して
                await DataManager.Instance.Initialize();

                // 再取得
                m_dataStages = DataManager.Instance.GetDataStages;

                await SaveManager.Instance.Initialize();
            }
        }

        /// <summary>
        /// スクロールビューの生成
        /// </summary>
        private void CreateScrollView()
        {
            // データのリストがなければ
            if (m_dataStages.GetList.IsEmpty()){ DebugManager.Error("StageId = Null"); }

            // 新しいスクロールビューのインデックス
            var newIndex = 0;

            for (int i = 0, count = m_dataStages.GetList.Count; i < count; ++i)
            {
                // IDからインデックスを取得
                int index = m_dataStages.GetList[i].id / STAGE_INIT_NUMBER;

                // 解放済みクエストのリストにIDがあれば
                if (m_saveData.openStageIds.Contains(m_dataStages.GetList[i].id))
                {
                    // インデックスが変更されていれば
                    if(newIndex != index)
                    {
                        newIndex = index;

                        // スクロールビューを生成し初期化する
                        CreateScrollView(index, i);                    
                    }
                }
            }
        }

        /// <summary>
        /// スクロールビューの生成
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="initId">最初に生成されるノードの初期ID</param>
        private void CreateScrollView(int index, int initId)
        {
            ScrollView scrollView = Instantiate(m_scrollVeiw, m_scrollWindow);

            if (!scrollView.IsNull())
            {
                // スクロールビューのリスト追加
                m_scrollVeiwList.Add(scrollView);
                m_scrollVeiwCount++;

                // 初期化
                scrollView.Initialize(index, initId, m_dataStages.GetList, m_saveData);
                scrollView.transform.SetLocalScale(Vector3.one);
                scrollView.Deactovation();
            }
        }

        /// <summary>
        /// スクロールビューを更新するコルーチン
        /// </summary>
        /// <returns></returns>
        private IEnumerator IUpdateScrollView()
        {
            m_leftButton.interactable  = false;
            m_rightButton.interactable = false;

            // 選択前のスクロールビューを非表示にして
            m_scrollVeiwList[m_selectedIndex].Deactovation();
            // 選択しているスクロールビューを表示する
            m_scrollVeiwList[m_selectIndex].Activation();

            // いったん待機
            yield return new WaitForSeconds(0.5f);

            // ボタンの設定
            m_leftButton.interactable = m_selectIndex != 0;
            m_rightButton.interactable = (m_selectIndex + 1) != m_scrollVeiwCount;
        }

        /// <summary>
        /// 左ボタンを押したとき
        /// </summary>
        public void OnClickLeftButton()
        {
            AudioManager.Instance.PlaySe(AudioSe.Decision);
            m_selectedIndex = m_selectIndex;
            m_selectIndex = Mathf.Clamp(m_selectIndex + 1, 0, m_scrollVeiwCount);
            StartCoroutine(IUpdateScrollView());
        }

        /// <summary>
        /// 右ボタンを押したとき
        /// </summary>
        public void OnClickRightButton()
        {
            AudioManager.Instance.PlaySe(AudioSe.Decision);
            m_selectedIndex = m_selectIndex;
            m_selectIndex = Mathf.Clamp(m_selectIndex - 1, 0, m_scrollVeiwCount);
            StartCoroutine(IUpdateScrollView());
        }

        public void OnClickDifficultyToggle(Toggle toggle)
        {
            m_saveData.easyMode = toggle.isOn;
            SaveManager.Save(m_saveData);
        }

        public void OnEnable()
        {
            UpdateManager.AddUpdatable(this);
        }

        public void OnDisable()
        {
            UpdateManager.RemoveUpdatable(this);
        }

        #endregion
    }
}