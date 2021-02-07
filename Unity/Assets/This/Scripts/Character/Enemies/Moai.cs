using ControllerSystem;
using ItemSystem;
using ManagerSystem;
using UnityEngine;
using static ExtensionComponent;

namespace CharacterSystem 
{
    public class Moai : BaseEnemyController, IFixedUpdatable
    {
        #region 変数宣言

        /// <summary>
        /// レーザー
        /// </summary>
        [SerializeField] LaserBeam[] m_laserArry = new LaserBeam[2];

        /// <summary>
        /// プレイヤ-に標準を合わせている
        /// </summary>
        private bool m_lockOnPlayer = false;

        /// <summary>
        /// m_startPosArray配列の個数
        /// </summary>
        private int m_length;

        private BasePlayerController m_playerController;

        #endregion

        #region メソッド

        protected override void Initialize()
        {
            FindTagComponent(ref m_playerController, "PlayerController");

            InitializeLaser();
        }

        public void FixedUpdateMe()
        {
            FireLasers();
        }

        /// <summary>
        /// レーザーを初期化する
        /// </summary>
        private void InitializeLaser()
        {
            // レーザーの開始地点を設定
            m_length = m_laserArry.Length;
            for (int i = 0; i < m_length; ++i)
            {
                m_laserArry[i].Initialize(m_playerController);
            }
        }

        /// <summary>
        /// レーザーを放つ
        /// </summary>
        private void FireLasers()
        {
            if (!m_lockOnPlayer) { return; }

            for (int i = 0; i < m_length; ++i)
            {
                m_laserArry[i].FireLasers();
            }
        }

        /// <summary>
        /// プレイヤーにロックオンする
        /// </summary>
        private void LockOnPlayer(bool lockOn)
        {
            for (int i = 0; i < m_length; ++i)
            {
                m_laserArry[i].SetActiveLaser(m_lockOnPlayer = lockOn);
            }

            AudioManager.Instance.PlayVoice(AudioVoice.Beam);
        }

        protected override void TriggerWithPlayer(BasePlayerController playerController)
        {
            base.TriggerWithPlayer(playerController);
            m_playerController = playerController;
            LockOnPlayer(true);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!m_playerController.IsNull())
            {
                LockOnPlayer(false);
            }
        }

        /// <summary>
        /// 画像をタッチしたとき
        public void PointerDown()
        {
            for (int i = 0; i < m_length; ++i)
            {
                m_laserArry[i].Deactovation();
            }

            Destroy(this);
        }

        public void OnEnable()
        {
            FixedUpdateManager.AddUpdatable(this);
        }

        public void OnDisable()
        {
            FixedUpdateManager.RemoveUpdatable(this);
        }

        #endregion
    }
}