using ControllerSystem;
using ItemSystem;
using ManagerSystem;
using UnityEngine;
using static ExtensionComponent;

namespace CharacterSystem
{
    /// <summary>
    /// U.F.O
    /// </summary>
    public class UFO : BaseEnemyController, IFixedUpdatable
    {
        #region 変数宣言

        /// <summary>
        /// 攻撃関係のデータ
        /// </summary>
        [SerializeField] AttackEnemyData m_attackData = default;

        /// <summary>
        /// 移動関係のデータ
        /// </summary>
        [SerializeField] AcceleratingEnemyData m_movingData = default;

        /// <summary>
        /// Prefabの管理
        /// </summary>
        private PrefabManager m_prefabs;

        /// <summary>
        /// 経過時間
        /// </summary>
        private float m_elapsedTime = default;

        private BasePlayerController m_playerController;

        /// <summary>
        /// 移動方向
        /// </summary>
        private Vector2 GetDirection
        {
            get
            {
                var self = this.transform.localPosition;
                var player = m_playerController.transform.localPosition;

                if (player.x > self.x)
                {
                    return Vector2.right;
                }

                return Vector2.left;
            }
        }

        private Rigidbody2D m_rb;

        #endregion

        #region メソッド

        protected override void Initialize()
        {
            FindTagComponent(ref m_playerController, "PlayerController");
            this.SetComponent(ref m_rb);
            this.SetComponent(ref m_prefabs);

            m_prefabs.Initialize();
        }

        public void FixedUpdateMe()
        {
            m_elapsedTime += Time.deltaTime;

            AttackMotion();

            MoveMotion();
        }

        /// <summary>
        /// 移動モーション
        /// </summary>
        private void MoveMotion()
        {
            EnemyMovement.AddConstantForce(m_rb,
                                           m_movingData.GetMoveSpeed,
                                           GetDirection,
                                           m_movingData.GetMoveForceMultiplier);
        }

        /// <summary>
        /// 攻撃モーション
        /// </summary>
        private void AttackMotion()
        {
            if (m_attackData.GetAttackTime < m_elapsedTime)
            {
                InitializeEnemy(m_prefabs.GetPrefab(), this.transform);

                m_elapsedTime = 0f;
            }
        }

        /// <summary>
        /// Bulletのパラメータの設定
        /// </summary>
        /// <param name="enemyPrefab">パラメータを設定する敵</param>
        /// <param name="parent">開始座標</param>
        /// <param name="target">目標座標</param>
        private static void InitializeEnemy(GameObject enemyPrefab, Transform parent)
        {
            enemyPrefab.transform.SetLocalPosition(parent.localPosition);
            enemyPrefab.gameObject.Activation();
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