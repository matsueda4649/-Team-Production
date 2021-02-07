using ControllerSystem;
using DG.Tweening;
using ManagerSystem;
using UnityEngine;
using static ExtensionDOTween;
using static ExtensionMathf;
using static UnityEngine.Mathf;

namespace CharacterSystem
{
    /// <summary>
    /// 武器で攻撃する敵
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class SoldierEnemyController : BaseEnemyController, IFixedUpdatable
    {
        #region 変数宣言

        [SerializeField] DOtweenMovingEnemyData m_data = default;

        private Tween m_tween = null;

        /// <summary>
        /// 移動時間
        /// </summary>
        [SerializeField] float m_moveDuration = 5f;

        /// <summary>
        /// 目的地
        /// </summary>
        private float m_targetPosX;

        /// <summary>
        /// 移動方向
        /// </summary>
        private Direction m_direction;

        /// <summary>
        /// 移動許可
        /// </summary>
        private bool m_allowMove = true;

        /// <summary>
        /// 攻撃許可
        /// </summary>
        private bool m_allowAttack = false;

        private Transform m_transform;

        [SerializeField] Transform m_sprite = default;

        #endregion

        #region メソッド

        protected override void Initialize()
        {
            this.SetComponent(ref m_transform);

            m_direction = m_data.GetMoveDirection;

            // 目標座標の設定
            m_targetPosX = GetMoveLength(m_transform, m_data.GetMoveLength, m_direction);
        }

        public void FixedUpdateMe()
        {
            // カメラに映っていなかったら
            if (!GetSpriteRender.isVisible) { return; }

            // 止まっていたら
            if (Time.timeScale == 0f) { return; }

            if (m_allowMove) { MoveMotion(); }
            else if (m_allowAttack) { AttackMotion(); }
        }

        /// <summary>
        /// 移動距離を取得
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="length">移動距離</param>
        /// <param name="direction">移動方向</param>
        /// <returns></returns>
        private static float GetMoveLength(Transform target, float length, Direction direction)
        {
            var start = target.localPosition.x;
            return start + length * (int)direction;
        }

        /// <summary>
        /// 移動方向を取得する
        /// </summary>
        private static Direction GetDirection(Direction direction)
        {
            int dir = (int)direction;
            return (Direction)ReverseSign(ref dir);
        }

        /// <summary>
        /// 移動モーション
        /// </summary>
        private void MoveMotion()
        {
            m_allowMove = false;

            m_tween = DOLocalMoveX(m_transform, m_targetPosX, m_moveDuration, () =>
            {
                m_allowAttack = true;
            });
        }

        /// <summary>
        /// 攻撃モーション
        /// </summary>
        private void AttackMotion()
        {
            // 回転
            var rotateValue = new Vector3(0f, 0f, -90f);

            // 回転時間
            float rotateDuration = Abs(m_moveDuration).Divide();

            m_allowAttack = false;

            ShakeClubAnimation(rotateValue, rotateDuration);
        }

        /// <summary>
        /// こん棒を振るアニメーション
        /// </summary>
        /// <param name="rotateValue"></param>
        /// <param name="rotateDuration"></param>
        /// <param name="callback"></param>
        private void ShakeClubAnimation(Vector3 rotateValue, float rotateDuration)
        {
            // こん棒を振るアニメーション
            m_tween = DOLocalRotate(m_sprite, rotateValue, rotateDuration, () => {

                AudioManager.Instance.PlayVoice(AudioVoice.Demon);

                // こん棒を振り終わる
                m_tween = DOLocalRotate(m_sprite, Vector3.zero, rotateDuration, () => {
                    ResetParameter();
                });
            });
        }

        /// <summary>
        /// 必要なパラメータ―の初期化
        /// </summary>
        private void ResetParameter()
        {
            m_direction = GetDirection(m_direction);
            var angle = this.transform.localEulerAngles.y - 180f;
            this.transform.SetLocalRotation(angle, Vector2.up);
            m_targetPosX = GetMoveLength(m_transform, m_data.GetMoveLength, m_direction);

            m_allowMove = true;
        }

        protected override void CollideWithPlayer(BasePlayerController playerController)
        {
            base.CollideWithPlayer(playerController);

            DeathMotion(playerController);
        }

        protected override void TriggerWithPlayer(BasePlayerController playerController)
        {
            base.TriggerWithPlayer(playerController);

            playerController.DeathMotion();
        }

        public void OnEnable()
        {
            FixedUpdateManager.AddUpdatable(this);
        }

        public void OnDisable()
        {
            FixedUpdateManager.RemoveUpdatable(this);

            Kill(m_tween);
        }

        #endregion
    }
}