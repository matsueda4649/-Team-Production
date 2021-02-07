using System;
using UnityEngine;

namespace ControllerSystem
{
    /// <summary>
    /// プレイヤーのコントローラー
    /// </summary>
    [RequireComponent(typeof(Animator), typeof(Rigidbody2D))]
    public abstract class BasePlayerController : MonoBehaviour
    {
        #region 変数宣言

        [SerializeField] PlayerData m_data = default;

        [SerializeField] Animator m_animator = default;

        private Rigidbody2D m_rb = null;

        private SpriteRenderer m_renderer = null;

        /// <summary>
        /// Rigidbodyを取得
        /// </summary>
        public Rigidbody2D GetRb2D { get => m_rb; }

        /// <summary>
        /// キャラクターデータを取得
        /// </summary>
        public PlayerData GetData { get => m_data; }

        /// <summary>
        /// アニメーション
        /// </summary>
        public Animator GetAnimator { get => m_animator; }

        /// <summary>
        /// SpriteRendererを取得
        /// </summary>
        public SpriteRenderer GetRenderer { get => m_renderer; }

        /// <summary>
        /// 移動方向
        /// </summary>
        public float InputDirection { get; set; }

        /// <summary>
        /// ジャンプ許可
        /// </summary>
        public bool AllowJump { get; set; }

        /// <summary>
        /// 死亡時
        /// </summary>
        public Action OnDead { get; set; }

        /// <summary>
        /// ゲームクリアコールバック
        /// </summary>
        public Action OnGameClear { get; set; }

        /// <summary>
        /// 初期摩擦
        /// </summary>
        public float InitFriction{ get; private set; }

        /// <summary>
        /// 初期バウンド
        /// </summary>
        public float InitBounciness { get; private set; }

        #endregion

        #region メソッド

        protected virtual void Awake()
        {
            Initialize();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        protected virtual void Initialize()
        {
            this.SetComponent(ref m_animator);
            this.SetComponent(ref m_rb);
            this.SetComponent(ref m_renderer);

            // 変更用を生成し初期値を設定
            m_rb.sharedMaterial = Instantiate(m_data.GetPhysicsMaterial2D);
            m_rb.SetBodyType(RigidbodyType2D.Static);
            InitFriction = m_rb.sharedMaterial.friction;
            InitBounciness = m_rb.sharedMaterial.bounciness;
        }

        /// <summary>
        /// 行動を許可する
        /// </summary>
        public void AllowAction()
        {
            m_rb.SetBodyType(RigidbodyType2D.Dynamic);
        }

        /// <summary>
        /// 死亡判定
        /// </summary>
        public virtual void DeathMotion()
        {
            // 死亡
            OnDead?.Invoke();
            this.gameObject.Deactovation();
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            var enemyController = collision.gameObject.GetComponent<BaseEnemyController>();
            if(!enemyController.IsNull()) { CollideWithEnemy(enemyController); }

            var flagController  = collision.gameObject.GetComponent<FlagController>();
            if (!flagController.IsNull()) { CollideWithFlag(flagController); }
        }

        /// <summary>
        /// 敵との衝突処理
        /// </summary>
        /// <param name="enemyController">敵</param>
        protected abstract void CollideWithEnemy(BaseEnemyController enemyController);

        /// <summary>
        /// フラグとの衝突処理
        /// </summary>
        /// <param name="flagController"> フラグ </param>
        protected virtual void CollideWithFlag(FlagController flagController) 
        {
            if(flagController.GetData.OnDead) { DeathMotion(); }
        }

        protected virtual void OnTriggerEnter2D(Collider2D collider)
        {
            var enemyController = collider.gameObject.GetComponent<BaseEnemyController>();
            if (!enemyController.IsNull()) { TriggerWithEnemy(enemyController); }

            var flagController = collider.gameObject.GetComponent<FlagController>();
            if (!flagController.IsNull()) { TriggerWithFlag(flagController); }
        }

        /// <summary>
        /// 敵との衝突処理
        /// </summary>
        /// <param name="enemyController">敵</param>
        protected abstract void TriggerWithEnemy(BaseEnemyController enemyController);

        /// <summary>
        /// フラグとの衝突処理
        /// </summary>
        /// <param name="flagController"> フラグ </param>
        protected virtual void TriggerWithFlag(FlagController flagController) 
        {
            if (flagController.GetData.OnDead) { DeathMotion(); }
        }

        #endregion
    }
}