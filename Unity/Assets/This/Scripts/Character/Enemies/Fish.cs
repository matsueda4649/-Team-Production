using ControllerSystem;
using ManagerSystem;
using UnityEngine;
using static ExtensionComponent;
using static ExtensionMathf;
using static UnityEngine.Mathf;

namespace CharacterSystem
{
    /// <summary>
    /// 魚
    /// </summary>
    public class Fish : BaseEnemyController, IUpdatable
    {
        #region 変数宣言

        /// <summary>
        /// データ
        /// </summary>
        [SerializeField] MovingEnemyData m_data = default;

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

        private Animator m_animator;

        private BasePlayerController m_playerController;

        #endregion

        #region メソッド

        protected override void Initialize()
        {
            this.SetComponent(ref m_animator);

            // プレイヤーを取得
            FindTagComponent(ref m_playerController, "PlayerController");

            m_animator.SetBool("Swim", true);
        }

        public void UpdateMe()
        {
            if (GetSpriteRender.isVisible) { MoveMotion(); }
        }

        /// <summary>
        /// 移動モーション
        /// </summary>
        private void MoveMotion()
        {
            var start  = this.transform.localPosition;
            var target = m_playerController.transform.localPosition;
            var direction = m_data.GetMoveSpeed * GetDirection;
            transform.Translate(GetTranslation(start, target, direction), Space.World);
        }

        /// <summary>
        ///追加座標の取得
        /// </summary>
        /// <param name="start">移動開始座標</param>
        /// <param name="target">目標座標</param>
        /// <param name="direction">移動方向</param>
        /// <returns></returns>
        private static Vector2 GetTranslation(Vector2 start, Vector2 target, Vector2 direction)
        {
            // パラメータ設定
            var angle = 90f - CalculateRadian(start, target); // アングル
            return new Vector2((direction.x), Sin(angle * Deg2Rad)) * Time.deltaTime;
        }

        protected override void CollideWithPlayer(BasePlayerController playerController)
        {
            base.CollideWithPlayer(playerController);
            DeathMotion(playerController);
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