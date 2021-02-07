using ControllerSystem;
using ManagerSystem;
using UnityEngine;
using static UnityEngine.Mathf;

namespace CharacterSystem
{
    /// <summary>
    /// 単純な横移動のみ
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class MovingEnemyController : BaseEnemyController, IFixedUpdatable
    {
        #region 変数宣言

        /// <summary>
        /// 強制ゲームオーバー
        /// </summary>
        [SerializeField] bool m_forceGameOver = false;

        /// <summary>
        /// データ
        /// </summary>
        [SerializeField] MovingEnemyData m_data = default;

        /// <summary>
        /// 移動座標
        /// </summary>
        [SerializeField] Vector2 m_addPostion = new Vector2(5f, 0);

        /// <summary>
        /// 最初の座標
        /// </summary>
        private Vector2 m_startPosition;

        /// <summary>
        /// 経過時間
        /// </summary>
        private float m_elapsedTime;

        #endregion

        #region メソッド

        /// <summary>
        /// 初期設定
        /// </summary>
        protected override void Initialize()
        {
            m_startPosition = this.transform.localPosition;

            m_addPostion += m_startPosition;
        }

        public void FixedUpdateMe()
        {
            MoveMotion();
        }

        /// <summary>
        /// 移動モーション
        /// </summary>
        private void MoveMotion()
        {
            // 経過時間
            m_elapsedTime += Time.deltaTime * m_data.GetMoveSpeed;

            // 移動距離
            float length = 1f;

            // 目標座標
            var target = Vector2.Lerp(m_startPosition, m_addPostion, PingPong(m_elapsedTime, length));

            this.transform.SetLocalPosition(target);
        }

        protected override void CollideWithPlayer(BasePlayerController playerController)
        {
            base.CollideWithPlayer(playerController);

            if (m_forceGameOver) { playerController.DeathMotion(); }

            DeathMotion(playerController);
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