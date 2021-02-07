using ControllerSystem;
using ManagerSystem;
using UnityEngine;
using static UnityEngine.Mathf;

namespace CharacterSystem
{
    /// <summary>
    /// Playerを使ったコントローラー
    /// </summary>
    public class PlayerController : BasePlayerController, IFixedUpdatable
    {
        #region 変数宣言

        /// <summary>
        /// 当たり判定の管理
        /// </summary>
        [SerializeField] ContactFilter2D m_filter2d = default;

        /// <summary>
        /// ジャンプした回数
        /// </summary>
        private int m_jumpCount = 0;

        /// <summary>
        /// 強制的にジャンプ時の方向を制御
        /// </summary>
        private bool m_compulsion = false;

        /// <summary>
        /// 自動で動かすかどうか
        /// </summary>
        private bool m_autoMoving = false;

        /// <summary>
        /// 移動方向
        /// </summary>
        private float m_direction;

        private ParticleSetting m_particleSetting;

        #endregion

        #region メソッド

        protected override void Initialize()
        {
            base.Initialize();
            this.SetComponent(ref m_particleSetting);
            m_particleSetting.Initialize();
        }

        public void FixedUpdateMe()
        {
            MoveMotion();
            JumpMotion();

            var isPlaying = GetRb2D.OnGround() && GetRb2D.IsMove();
            m_particleSetting.SetParticle(isPlaying);
        }

        /// <summary>
        /// 移動モーション
        /// </summary>
        private void MoveMotion()
        {
            // 移動方向の設定
            float direction = /* m_compulsion ? m_direction : */ InputDirection;

            PlayMovamentAnimation(direction);

            // 移動停止処理
            if (m_autoMoving && InputDirection == 0) { return; }

            // 移動ベクトルの設定
            Vector2 moveVelocity = GetRb2D.velocity;

            moveVelocity.x = GetData.GetMoveSpeed * direction * Time.deltaTime;

            // 移動ベクトル
            Vector2 force = GetData.GetMoveForceMultiplier * (moveVelocity - GetRb2D.velocity);
            GetRb2D.AddForce(force, ForceMode2D.Force);
        }

        /// <summary>
        /// ジャンプモーション
        /// </summary>
        private void JumpMotion()
        {
            // ジャンプを許可 && 現在のジャンプ回数が最大値未満
            if (AllowJump && (m_jumpCount < GetData.GetMaxJumpCount))
            {
                AddJumpVelocity();
            }

            // ジャンプを許可させる
            AllowJump = false;
        }

        /// <summary>
        /// 移動時のアニメーションを再生
        /// </summary>
        /// <param name="direction">移動方向</param>
        private void PlayMovamentAnimation(float direction)
        {
            // キャラクターの向きの設定
            if (direction > 0 && this.transform.localEulerAngles.y != 0f) 
            {
                this.transform.SetLocalRotation(0f, Vector3.up);
            }
            else if (direction < 0 && this.transform.localEulerAngles.y != 180f)
            {
                this.transform.SetLocalRotation(180f, Vector3.up);
            }


            //if (direction != 0) { GetRenderer.flipX = direction < 0f; }

            // アニメーション再生の許可
            bool allowPlay = direction != 0;

            GetAnimator.SetBool("Move", allowPlay);
        }

        /// <summary>
        /// ジャンプ時のベクトルの設定
        /// </summary>
        private void AddJumpVelocity()
        {
            // 右か左を判断する
            if (!m_compulsion)
            {
                m_direction = InputDirection;
                m_compulsion = true;
                m_autoMoving = false;
            }

            // ジャンプ回数をカウント
            m_jumpCount++;

            // 速度を初期化
            GetRb2D.velocity = Vector2.zero;

            AudioManager.Instance.PlayVoice(AudioVoice.Jump);

            // ジャンプさせる
            GetRb2D.AddForce(Vector2.up * GetData.GetJumpForce, ForceMode2D.Impulse);
        }

        protected override void CollideWithEnemy(BaseEnemyController enemyController)
        {
            // 成功したら
            if(StepOnSuccessful(enemyController.transform))
            {
                AddJumpVelocity();
            }
            // 失敗したら
            else
            {
                DeathMotion();
            }
        }

        /// <summary>
        /// 敵を踏めたかどうか判定
        /// </summary>
        /// <param name="enemyPos"> 敵の座標 </param>
        /// <returns> 成功判定 </returns>
        private bool StepOnSuccessful(Transform enemyPos)
        {
            // プレイヤーの高さ
            float playerHeight = Round(this.transform.localPosition.y);
            // 敵の高さ
            float enemyHeight  = Round(enemyPos.localPosition.y);

            // 上から踏む && 自身が落下しているいたら成功
            return playerHeight > enemyHeight; // && GetRb2D.IsDown();
        }

        protected override void TriggerWithEnemy(BaseEnemyController enemyController)
        {
            DeathMotion();
        }

        protected override void CollideWithFlag(FlagController flagController)
        {
            base.CollideWithFlag(flagController);

            // 地面判定
            if (flagController.GetData.OnGround) { OnGround(flagController); }

            // 死亡判定
            if (flagController.GetData.OnDead)   { DeathMotion(); }

            // クリア判定
            if (flagController.GetData.OnClear)  { OnGameClear?.Invoke(); }
        }

        /// <summary>
        /// 地面に着地したら
        /// </summary>
        private void OnGround(FlagController flag)
        {
            if (GetRb2D.IsTouching(m_filter2d))
            {
                // 落下しているなら  || 強制的に地面判定 
                if (GetRb2D.IsDown() || flag.GetData.OnForceGround)
                {
                    m_autoMoving = flag.GetData.OnForceGround;
                    m_compulsion = false;
                    m_jumpCount = 0;
                    // GetRb2D.sharedMaterial.friction = InitFriction;
                }
            }
        }

        protected override void TriggerWithFlag(FlagController flagController)
        {
            base.TriggerWithFlag(flagController);

            // 死亡判定
            if (flagController.GetData.OnDead)  { DeathMotion(); }

            // クリア判定
            if (flagController.GetData.OnClear) { OnGameClear?.Invoke(); }
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