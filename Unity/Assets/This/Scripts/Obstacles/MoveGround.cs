using ControllerSystem;
using ManagerSystem;
using UnityEngine;

namespace ItemSystem
{
    /// <summary>
    /// 移動する床
    /// </summary>
    public class MoveGround : MonoBehaviour, IFixedUpdatable
    {
        #region Enum

        /// <summary>
        /// 座標を足すか引くか
        /// </summary>
        private enum Adjustment
        {
            /// <summary>
            /// 加法
            /// </summary>
            Addition,
            /// <summary>
            /// 減法
            /// </summary>
            Subtraction,
        } 

        /// <summary>
        /// 移動方向
        /// </summary>
        private enum Coordinate
        {
            /// <summary>
            /// 縦
            /// </summary>
            Vertical,
            /// <summary>
            /// 横
            /// </summary>
            Horizontal,
        }

        #endregion

        #region 変数宣言

        /// <summary>
        /// 自動移動可能
        /// </summary>
        [SerializeField] bool m_autoMovable = true;

        /// <summary>
        /// 移動距離
        /// </summary>
        [SerializeField] float m_length = 5f;

        /// <summary>
        /// 加速度
        /// </summary>
        [SerializeField] float m_acceleration = 4f;

        /// <summary>
        /// 移動方向
        /// </summary>
        [SerializeField] Coordinate m_coordinate = Coordinate.Vertical;

        /// <summary>
        /// 座標の加算方法
        /// </summary>
        [SerializeField] Adjustment m_adjustment = Adjustment.Addition;

        /// <summary>
        /// 初期座標
        /// </summary>
        private Vector2 m_initPosition;

        /// <summary>
        /// 経過時間
        /// </summary>
        private float m_elapsedTime = 0;

        private Rigidbody2D m_rb;

        private BasePlayerController m_playerController;

        #endregion

        private void Start()
        {
            this.SetComponent(ref m_rb);
            m_initPosition = transform.position;
        }

        public void FixedUpdateMe()
        {
            if (m_autoMovable) { MoveMotion(); }
        }

        /// <summary>
        /// 移動モーション
        /// </summary>
        private void MoveMotion()
        {
            // 変更前の座標
            var previousPosition = m_rb.position;
            // 移動座標
            var movingPosition   = GetMovingPosition();

            m_rb.MovePosition(movingPosition);
            SetVelocityOfPlayer(movingPosition - previousPosition);
        }

        /// <summary>
        /// 移動座標の取得
        /// </summary>
        /// <returns></returns>
        private Vector2 GetMovingPosition()
        {
            m_elapsedTime += Time.deltaTime;

            Vector2 position = Vector2.zero;

            switch (m_adjustment)
            {
                case Adjustment.Addition:
                    position = m_initPosition + ReturnPosition(m_coordinate, m_elapsedTime, m_acceleration, m_length);
                    break;
                case Adjustment.Subtraction:
                    position = m_initPosition - ReturnPosition(m_coordinate, m_elapsedTime,m_acceleration, m_length);
                    break;
            }

            return position;
        }

        /// <summary>
        /// PlayerのVelocityを設定
        /// </summary>
        /// <param name="distance">距離</param>
        private void SetVelocityOfPlayer(Vector2 distance)
        {
            if (!m_playerController.IsNull())
            {
                // キー入力を取得
                var input = Mathf.Abs(m_playerController.InputDirection);

                // ベクトルの計算
                var x = m_playerController.GetRb2D.velocity.x * input;
                var y = m_playerController.GetRb2D.velocity.y;

                switch (m_coordinate)
                {
                    case Coordinate.Vertical:

                        // 速度の計算
                        var speedY = distance.y / Time.deltaTime;
                        y = speedY < 0 ? y + speedY : y;

                        break;
                    case Coordinate.Horizontal:

                        // 速度の計算
                        var speedX = distance.x / Time.deltaTime;
                        x += speedX;

                        break;
                }

                m_playerController.GetRb2D.velocity = new Vector2(x, y);
            }
        }

        /// <summary>
        /// 移動座標の計算
        /// </summary>
        /// <param name="coordinate">移動方向</param>
        /// <param name="acceleration">加速度</param>
        /// <param name="length">距離</param>
        private static Vector2 ReturnPosition(Coordinate coordinate, float time, float acceleration, float length)
        {
            float x = 0f, y = 0f;
            float moveSpeed = time * acceleration;

            switch (coordinate)
            {
                case Coordinate.Vertical:
                    y = Mathf.PingPong(moveSpeed, length);
                    break;
                case Coordinate.Horizontal:
                    x = Mathf.PingPong(moveSpeed, length);
                    break;
            }

            return new Vector2(x, y);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            m_autoMovable = true;
            collision.gameObject.SetComponent(ref m_playerController);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!m_playerController.IsNull())
            {
                m_playerController.GetRb2D.velocity = Vector2.zero;
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            var playerController = collision.gameObject.GetComponent<BasePlayerController>();
            if (!playerController.IsNull()) { m_playerController = null; }
        }

        public void OnEnable()
        {
            FixedUpdateManager.AddUpdatable(this);
        }

        public void OnDisable()
        {
            FixedUpdateManager.RemoveUpdatable(this);
        }
    }
}
/*
 * TODO
 * 200/06/29
 * matsueda
 * OnCollisionStay2Dでの処理のほうがよいのでは？
 */