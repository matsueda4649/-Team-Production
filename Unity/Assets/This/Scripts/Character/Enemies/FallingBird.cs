using ControllerSystem;
using UnityEngine;

namespace CharacterSystem
{
    /// <summary>
    /// 落下する鳥
    /// </summary>
    public class FallingBird : BaseBird
    {
        /// <summary>
        /// カウント時間の目安
        /// </summary>
        private readonly float[] CountTimeArry = { 4f, 3f, 1f };

        /// <summary>
        /// カウントダウンの目安カラー
        /// </summary>
        private readonly Color[] ColorArry = { Color.yellow, Color.red };

        #region 変数宣言

        /// <summary>
        /// 初期座標
        /// </summary>
        private Vector2 m_initPosition;

        /// <summary>
        /// 移動距離
        /// </summary>
        private float m_movingDistance = 0f;

        /// <summary>
        /// カウントダウンを開始する
        /// </summary>
        private bool m_startCountDown = false;

        /// <summary>
        /// ステップ
        /// </summary>
        private PhaseStep m_step = PhaseStep.First;

        /// <summary>
        /// カウント時間
        /// </summary>
        private float m_countTime;

        #endregion

        protected override void Initialize()
        {
            base.Initialize();
            m_initPosition = GetRb.position;
        }

        /// <summary>
        /// 移動モーション
        /// </summary>
        protected override void MoveMotion()
        {
            // 総合制限時間を超えたら
            if (m_step == PhaseStep.Third)
            {
                // 落下させる
                GetRb.SetBodyType(RigidbodyType2D.Dynamic);
                return;
            }

            // 移動距離の計算
            m_movingDistance += GetData.GetMoveSpeed * Time.deltaTime;

            // 移動座標
            var movingPosition = GetMovingPosition(m_initPosition, m_movingDistance);

            GetRb.MovePosition(movingPosition);
            SetVelocityOfPlayer(PlayerController, distance: movingPosition - GetRb.position);

            StartFallCountdown();
        }

        /// <summary>
        /// 落下のカウントダウンをする
        /// </summary>
        private void StartFallCountdown()
        {
            if (m_startCountDown)
            {
                m_countTime += Time.deltaTime;

                // 制限時間を超えたら
                if (m_countTime > CountTimeArry[(int)m_step - 1])
                {
                    m_countTime = 0f;
                    GetRenderer.color = ColorArry[(int)m_step - 1];
                    m_step++;
                }
            }
        }

        /// <summary>
        /// 移動座標を取得
        /// </summary>
        /// <param name="initPostion">初期座標</param>
        /// <param name="moveSpeed">移動速度</param>
        /// <returns></returns>
        private static Vector2 GetMovingPosition(Vector2 initPostion, float moveSpeed)
        {
            // 追加座標
            var addPosition = new Vector2(moveSpeed, Mathf.Sin(Time.time));

            // 移動結果の座標
            Vector2 movingPosition = initPostion + addPosition;

            return movingPosition;
        }

        protected override void CollisionWithPlayer(BasePlayerController playerController)
        {
            m_startCountDown = true;
        }

        protected override void OnTriggerExit2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);
            m_startCountDown = false;
        }
    }
}