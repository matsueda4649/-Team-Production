using ControllerSystem;
using ManagerSystem;
using UnityEngine;
using static RaycastSystem;

namespace CharacterSystem
{
    /// <summary>
    /// シャープ マリオでいうドッスンの逆のようなもの
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
    public class DroppedEnemyController : BaseEnemyController, IFixedUpdatable
    {
        #region 変数宣言

        /// <summary>
        /// キャラクターデータ
        /// </summary>
        [SerializeField] DroppedEnemyData m_data = default;

        /// <summary>
        /// 初期位置
        /// </summary>
        private Vector3 m_initPosition;

        /// <summary>
        /// 落下フラグ
        /// </summary>
        private bool m_allowDrop = false;

        /// <summary>
        /// 経過時間
        /// </summary>
        private float m_elapsedTime = 0f;

        /// <summary>
        /// 上方向に重力がかかるかどうか
        /// </summary>
        private bool m_upwardGravity= false;

        private Rigidbody2D m_rb = null;

        #endregion

        #region メソッド

        protected override void Initialize()
        {
            this.SetComponent(ref m_rb);

            // 初期位置を取得
            m_initPosition = this.transform.localPosition;

            // レイキャストが上方向なら TRUE
            m_upwardGravity = m_data.GetDirection.y > 0 ? true : false;
        }

        public void FixedUpdateMe()
        {
            PrepareToDrop();

            // 落下したら
            if (m_allowDrop)
            {
                // 時間を計測
                m_elapsedTime += Time.deltaTime;

                // インターバルを超えたら
                if (m_elapsedTime > m_data.GetInterval)
                {
                    ReturnToInitialPosition();
                }
            }
        }

        /// <summary>
        /// 落下の準備をする
        /// </summary>
        private void PrepareToDrop()
        {
            // 落下中なら
            if (m_allowDrop) { return; }

            // Rayの開始地点
            Vector2 origin = this.transform.localPosition;

            // Rayのヒット判定
            bool hit = RayHit2D(origin, m_data.GetDirection, m_data.GetDistance, m_data.GetColliderMask);

            // ヒットしたら
            if (hit)
            {
                // 重力の影響を受けさせる
                ChangeRigidbodyType(m_rb, RigidbodyType2D.Dynamic, m_data.GetGravity);
            }
        }

        /// <summary>
        /// RigidBody2DのBodyの変更
        /// </summary>
        /// <param name="bodyType">ボディタイプ</param>
        /// <param name="gravityScale">重力</param>
        private static void ChangeRigidbodyType(Rigidbody2D rb, RigidbodyType2D bodyType, float gravityScale)
        {
            // BodyTypeの変更
            if (rb.bodyType != bodyType)
            {
                rb.SetGravityScale(gravityScale);
                rb.bodyType = bodyType;
            }
        }

        /// <summary>
        /// 初期位置に戻る
        /// </summary>
        private void ReturnToInitialPosition()
        {
            float t = Time.deltaTime * m_data.GetInterval;
            var start = this.transform.localPosition;
            var end = Vector2.Lerp(start, m_initPosition, t);
            this.transform.SetLocalPosition(end);

            // 初期位置に戻ったら
            if (Mathf.Ceil(start.y)  >= m_initPosition.y && !m_upwardGravity || 
                Mathf.Floor(start.y) <= m_initPosition.y &&  m_upwardGravity)
            {
                this.transform.SetLocalPosition(m_initPosition);
                m_allowDrop = false;
                m_elapsedTime = 0f;
            }
        }

        protected override void CollideWithFlag(FlagController flagController)
        {
            base.CollideWithFlag(flagController);

            if (flagController.GetData.OnGround && !m_allowDrop)
            {
                AudioManager.Instance.PlayVoice(AudioVoice.Sharp);

                // 落下を許可する
                m_allowDrop = true;

                ChangeRigidbodyType(m_rb, RigidbodyType2D.Static, m_data.GetGravity);

                m_elapsedTime = 0f;
            }
        }

        protected override void CollideWithPlayer(BasePlayerController playerController)
        {
            base.CollideWithPlayer(playerController);
            playerController.DeathMotion();
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
        }

        #endregion
    }
}