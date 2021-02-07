using UnityEngine;
using static UnityEngine.Mathf;

namespace ControllerSystem
{
    /// <summary>
    /// 敵のコントローラー
    /// </summary>
    public abstract class BaseEnemyController : MonoBehaviour
    {
        #region 変数宣言

        /// <summary>
        /// キャラクターのSpriteRender
        /// </summary>
        [SerializeField] SpriteRenderer m_spriteRender = default;

        /// <summary>
        /// SpriteRenderを取得
        /// </summary>
        public SpriteRenderer GetSpriteRender { get => m_spriteRender; }

        #endregion

        #region メソッド

        private void Awake()
        {
            Initialize();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        protected abstract void Initialize();

        /// <summary>
        /// 死亡モーション
        /// </summary>
        /// <param name="playerController">プレイヤー</param>
        protected virtual void DeathMotion(BasePlayerController playerController)
        {
            // プレイヤーの高さ
            var playerHeight = Round(playerController.transform.localPosition.y);

            // 自身の高さ
            var selfHeight = Round(this.transform.localPosition.y);

            // 上から踏まれたら
            if (playerHeight >= selfHeight){ this.gameObject.Deactovation(); }
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            var playerController = collision.gameObject.GetComponent<BasePlayerController>();
            if (!playerController.IsNull()) { CollideWithPlayer(playerController); }

            var flagController = collision.gameObject.GetComponent<FlagController>();
            if (!flagController.IsNull()) { CollideWithFlag(flagController); }
        }

        /// <summary>
        /// プレイヤーとの衝突処理
        /// </summary>
        /// <param name="playerController">プレイヤー</param>
        protected virtual void CollideWithPlayer(BasePlayerController playerController) { }

        /// <summary>
        /// フラグとの衝突処理
        /// </summary>
        /// <param name="flagController"> フラグ </param>
        protected virtual void CollideWithFlag(FlagController flagController)
        {
            if (flagController.GetData.OnDead) { this.gameObject.Deactovation(); }
        }

        protected virtual void OnTriggerEnter2D(Collider2D collider)
        {
            var playerController = collider.gameObject.GetComponent<BasePlayerController>();
            if (!playerController.IsNull()) { TriggerWithPlayer(playerController); }

            var flagController = collider.gameObject.GetComponent<FlagController>();
            if (!flagController.IsNull()) { TriggerWithFlag(flagController); }
        }

        /// <summary>
        /// プレイヤーとの衝突処理
        /// </summary>
        /// <param name="playerController"></param>
        protected virtual void TriggerWithPlayer(BasePlayerController playerController) { }

        /// <summary>
        /// フラグとの衝突処理
        /// </summary>
        /// <param name="flagController"> フラグ </param>
        protected virtual void TriggerWithFlag(FlagController flagController)
        {
            if (flagController.GetData.OnDead) { this.gameObject.Deactovation(); }
        }

        #endregion
    }
}