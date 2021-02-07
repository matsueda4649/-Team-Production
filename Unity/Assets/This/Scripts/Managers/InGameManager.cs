using UnityEngine;
using WindowSystem;
using ControllerSystem;
using Cysharp.Threading.Tasks;

namespace ManagerSystem
{
    /// <summary>
    /// ゲーム全体を管理
    /// </summary>
    public class InGameManager : BaseScene, IUpdatable
    {
        #region 変数

        /// <summary>
        /// ステージデータ
        /// </summary>
        private DataStage m_dataStage;

        /// <summary>
        /// ゲームのフェーズを取得
        /// </summary>
        private Phase m_phase = Phase.None;

        /// <summary>
        /// 現在のフェーズを取得
        /// </summary>
        public Phase GetPhase { get => m_phase;}

        /// <summary>
        /// フェーズ内の進行
        /// </summary>
        private PhaseStep m_phaseStep = PhaseStep.First;

        /// <summary>
        /// フェーズ内の進行タイマー
        /// </summary>
        private float m_phaseTimer = 0f;

        /// <summary>
        /// カウトダウン開始
        /// </summary>
        private bool m_countDown = false;

        /// <summary>
        /// ゲームクリア時のメッセージ
        /// </summary>
        private readonly string[] m_clearMessages = new string[] { 
        
            "Clear!!",
            "Good!!",
            "Excellent",
            "Goooooal!!"
        };

        /// <summary>
        /// プレイヤー
        /// </summary>
        [SerializeField] BasePlayerController m_player = default;

        /// <summary>
        /// ゲームシーン
        /// </summary>
        [SerializeField] InGameScene m_inGameScene = default;

        /// <summary>
        /// ジャンプコントローラー
        /// </summary>
        [SerializeField] JumpController m_jumpController = default;

        /// <summary>
        /// 移動コントローラー
        /// </summary>
        [SerializeField] MovementController m_movementController = default;

        /// <summary>
        /// ポーズ用のウインドウ
        /// </summary>
        [SerializeField] PauseWindow m_pauseWindow = default;

        /// <summary>
        /// カメラ
        /// </summary>
        [SerializeField] InGameCamera m_camera = default;

        /// <summary>
        /// タイマー
        /// </summary>
        [SerializeField] TimeManager m_timer = default;

        /// <summary>
        /// 新記録判定
        /// </summary>
        private bool m_newRecord = false;

        #endregion

        #region メソッド

        public override string GetSceneName()
        {
            return m_dataStage.name;
        }

        /// <summary>
        /// 初期設定
        /// </summary>
        public override async UniTask Initialize()
        {
            m_inGameScene.Initialize();

            m_dataStage = m_inGameScene.GetDataStage();

            RegisterCallbacks();

            m_camera.Initialize(m_player);
            m_jumpController.Initialize(m_player);
            m_movementController.Initialize(m_player);

            // 制限時間を設定
            m_timer.Initialize(m_dataStage.limitTime);

            await base.Initialize();
        }

        public override async UniTask StartScene()
        {
            UpdatePhase(Phase.Init);

            await base.StartScene();
        }

        public void UpdateMe()
        {
            UpdatePhase();
        }

        private void LateUpdate()
        {
            m_camera.LateUpdatePhase();
            m_inGameScene.ScrollBackGround(m_camera.transform.localPosition);
        }

        /// <summary>
        /// コールバックを登録する
        /// </summary>
        private void RegisterCallbacks()
        {
            m_player.OnDead      = OnGameOver;
            m_player.OnGameClear = OnGameClear;
            m_timer.onTimeOver   = OnGameOver;
        }

        /// <summary>
        /// フェーズの進行
        /// </summary>
        public void UpdatePhase()
        {
            switch(m_phase)
            {
                // 初期
                case Phase.Init:
                    InitialPhase();
                    break;

                // プレイ中
                case Phase.Play:
                    PlayPhase();
                    break;

                // ゲームクリア
                case Phase.GameClear:
                    GameClearPhase();
                    break;

                // ゲームオーバー
                case Phase.GameOver:
                    GameOverPhase();
                    break;
            }
        }

        /// <summary>
        /// フェーズを更新する
        /// </summary>
        /// <param name="nextPhase"> 次のフェーズ </param>
        public void UpdatePhase(Phase nextPhase)
        {
            // 次のフェーズに設定
            m_phase = nextPhase;
            m_movementController.UpdatePhase(nextPhase);
            m_timer.UpdatePhase(nextPhase);

            m_phaseStep  = PhaseStep.First;
            m_phaseTimer = 0f;
        }

        /// <summary>
        /// 初期フェーズ
        /// </summary>
        private void InitialPhase()
        {
            switch (m_phaseStep)
            {
                case PhaseStep.First:
                    // BGM再生
                    AudioManager.Instance.PlayBgm(m_dataStage.audio);
                    // 進行開始
                    UpdatePhase(Phase.Play);
                    m_countDown = true;
                    m_player.AllowAction();
                    break;
            }
        }

        /// <summary>
        /// 進行フェーズ
        /// </summary>
        private void PlayPhase()
        {
            switch(m_phaseStep)
            {
                case PhaseStep.First:
                    if (m_countDown){ m_timer.UpdatePhase(); }
                    m_phaseStep = PhaseStep.Second;
                    break;

                case PhaseStep.Second:
                    m_phaseStep = PhaseStep.First;
                    break;
            }

            m_movementController.UpdatePhase();
            m_pauseWindow.PausePhase().Forget();
        }

        /// <summary>
        /// ゲームクリアフェーズ
        /// </summary>
        private void GameClearPhase()
        {
            switch (m_phaseStep)
            {
                case PhaseStep.First:
                    m_inGameScene.SetWindow();
                    m_phaseStep = PhaseStep.Second;
                    break;

                case PhaseStep.Second:
                    m_phaseTimer += Time.deltaTime;
                    if(m_phaseTimer > DefTime.WAIT_PHASE_TIME)
                    {
                        var saveData = m_inGameScene.GetSaveData;
                        if(saveData.questId > 0)
                        {
                            m_newRecord = SaveManager.Instance.SaveClear(saveData.questId, m_timer.ElapsedTime);
                        }
                        m_phaseStep = PhaseStep.Third;
                    }
                    break;

                case PhaseStep.Third:
                    var message = m_clearMessages[Random.Range(0, m_clearMessages.Length)];
                    UIManager.Instance.ShowRecordDialg(message, m_newRecord, m_timer.ElapsedTime);
                    m_phaseStep = PhaseStep.Fourth;
                    break;
            }
        }

        /// <summary>
        /// ゲームオーバーフェーズ
        /// </summary>
        private void GameOverPhase()
        {
            switch (m_phaseStep)
            {
                case PhaseStep.First:
                    m_phaseTimer += Time.deltaTime;
                    if (m_phaseTimer > DefTime.WAIT_PHASE_TIME) 
                    {
                        m_phaseStep = PhaseStep.Second;
                    }
                    break;

                case PhaseStep.Second:
                    m_inGameScene.ShowOverDialog("GameOver");
                    m_phaseStep = PhaseStep.Third;
                    break;
            }
        }

        /// <summary>
        /// ゲームクリア
        /// </summary>
        private void OnGameClear()
        {
            // ゲームクリアのフェーズに移行
            UpdatePhase(Phase.GameClear);

            // BGMを停止する
            AudioManager.Instance.StopBgm();

            AudioManager.Instance.PlaySe(AudioSe.GameClear);

            m_player.OnGameClear = null;
            m_player.OnDead = null;
        }

        /// <summary>
        /// ゲームオーバー
        /// </summary>
        private void OnGameOver()
        {
            // ゲームオーバーのフェーズに移行
            UpdatePhase(Phase.GameOver);

            // BGMを停止する
            AudioManager.Instance.StopBgm();

            AudioManager.Instance.PlaySe(AudioSe.GameOver);

            m_player.OnDead = null;
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