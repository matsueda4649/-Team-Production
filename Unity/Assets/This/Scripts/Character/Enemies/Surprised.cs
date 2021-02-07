using ControllerSystem;
using ManagerSystem;
using UnityEngine;
using static ExtensionMathf;
using static ExtensionComponent;

namespace CharacterSystem
{
    /// <summary>
    /// ビックリマークのコントローラー
    /// </summary>
    public class Surprised : BaseEnemyController, IFixedUpdatable
    {
        /// <summary>
        /// プレイヤーに対するアクション
        /// </summary>
        private enum ActionType
        {
            /// <summary>
            /// 何もなし
            /// </summary>
            None = 0,
            /// <summary>
            /// 強制ゲームオーバー
            /// </summary>
            CompulsionDead,
            /// <summary>
            /// 上方向に飛ばす
            /// </summary>
            AddUpForce,
            /// <summary>
            /// 重力変化
            /// </summary>
            ChangeGravity,
        }

        #region 変数宣言

        /// <summary>
        /// データ
        /// </summary>
        [SerializeField] MovingEnemyData m_data = default;

        /// <summary>
        /// アクションタイプ
        /// </summary>
        [SerializeField] ActionType m_actinType = default;

        /// <summary>
        /// 設定する色 
        /// 敵の種類を区別させる
        /// </summary>
        private readonly Color[] m_color = new Color[] {Color.white, Color.red, Color.yellow, Color.blue};

        /// <summary>
        /// プレイヤー
        /// </summary>
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

        private Rigidbody2D m_rb = null;

        #endregion

        #region メソッド

        protected override void Initialize()
        {
            // プレイヤーを取得
            FindTagComponent(ref m_playerController, "PlayerController");

            this.SetComponent(ref m_rb);

            // アクションタイプを決める
            GetSpriteRender.color = m_color[(int)m_actinType];
        }

        public void FixedUpdateMe()
        {
            // 画面内にいたら
            if (GetSpriteRender.isVisible){ MoveMotion(); }
        }

        /// <summary>
        /// 移動モーション
        /// </summary>
        private void MoveMotion()
        {
            var velocity = m_data.GetMoveSpeed * GetDirection;
            m_rb.ConstantAddForceX(velocity * Time.deltaTime);
        }

        protected override void CollideWithPlayer(BasePlayerController playerController)
        {
            base.CollideWithPlayer(playerController);

            switch (m_actinType)
            {
                case ActionType.CompulsionDead:
                    m_playerController.DeathMotion();
                    break;

                case ActionType.AddUpForce:
                    AddUpForceToPlayer();
                    break; 

                case ActionType.ChangeGravity:
                    ChangePlayerGravityScale();
                    break;
            }

            DeathMotion(playerController);
        }

        /// <summary>
        /// Playerに上方向の力を加える
        /// </summary>
        private void AddUpForceToPlayer()
        {
            var velocity = Vector2.up * RandomInt(min: 5);
            m_playerController.GetRb2D.AddForce(velocity, ForceMode2D.Impulse);
        }

        /// <summary>
        /// PlayerのGravityScaleを変更する
        /// </summary>
        private void ChangePlayerGravityScale()
        {
            // 変更後のgravityScale
            var gravityScale = RandomInt(min: -1, max: 5);
            m_playerController.GetRb2D.gravityScale = gravityScale;
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