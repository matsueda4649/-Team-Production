using ControllerSystem;
using ManagerSystem;
using UnityEngine;
using static UnityEngine.Mathf;
using static ExtensionComponent;
using ItemSystem;

namespace CharacterSystem
{
    /// <summary>
    /// ハッシュ　自身の一部を飛ばして攻撃
    /// </summary>
    [RequireComponent(typeof(BoxCollider2D))]
    public class Hash : BaseEnemyController, IFixedUpdatable
    {
        #region 変数宣言

        /// <summary>
        /// データ
        /// </summary>
        [SerializeField] AttackEnemyData m_data = default;

        /// <summary>
        /// Spriteの配列
        /// </summary>
        [SerializeField] Sprite[] m_spriteArray = new Sprite[4]; 

        /// <summary>
        /// Listのインデックス
        /// </summary>
        private int m_index = 0;

        /// <summary>
        /// Spriteの数
        /// </summary>
        private int m_spriteCount;

        /// <summary>
        /// スタン状態のフラグ
        /// </summary>
        private bool m_onStan = false;

        /// <summary>
        /// 弾の管理
        /// </summary>
        private BulletManagement m_bullets;

        /// <summary>
        /// プレイヤー
        /// </summary>
        private BasePlayerController m_playerController;

        /// <summary>
        /// 経過時間
        /// </summary>
        private float m_elapsedTime = 0f;

        #endregion

        #region メソッド

        protected override void Initialize()
        {
            this.SetComponent(ref m_bullets);
            m_bullets.Initialize();

            FindTagComponent(ref m_playerController, "PlayerController");
            m_spriteCount = m_spriteArray.Length;
        }

        public void FixedUpdateMe()
        {
            // 時間計測
            m_elapsedTime += Time.deltaTime;

            // 行動できるなら
            if(!m_onStan) { AttackMotion(); }
            // スタン状態なら
            else { StanMotion(); }
        }

        /// <summary>
        /// 攻撃モーション
        /// </summary>
        private void AttackMotion()
        {
            // 攻撃可能時間になったら
            if (m_elapsedTime > m_data.GetAttackTime)
            {
                // 自身の座標
                var start = this.transform.localPosition;
                var target = m_playerController.transform.localPosition;

                EnemyAttack.InitializeBullet(m_bullets.GetBullet(), start, target, false);

                AudioManager.Instance.PlayVoice(AudioVoice.Hash);

                // Spriteを変更する
                SetActiveSprite(addIndex: 1);
            }
        }

        /// <summary>
        /// スタン状態
        /// </summary>
        private void StanMotion()
        {
            // 初期状態に戻ったら
            if (m_index == 0)
            {
                // スタン時間を超えたら
                if (m_elapsedTime > m_data.GetStanTime)
                {
                    // スタン状態を回復させる
                    m_onStan = false;
                    m_elapsedTime = 0f;
                }
            }
            else if (m_elapsedTime > m_data.GetRecoveryTime)
            {
                // 回復時間を超えたら
                SetActiveSprite(addIndex: -1);
            }
        }

        /// <summary>
        /// Spriteの表示 : 非表示 を管理
        /// </summary>
        /// <param name="addIndex">インデックスに加える値</param>
        private void SetActiveSprite(int addIndex)
        {
            // オブジェクトがあれば
            if (m_spriteCount > 0 )
            {
                // インデックスを + value (0 <= index < m_spriteCount)
                m_index = (m_index + addIndex) % m_spriteCount;

                // Spriteを設定する
                GetSpriteRender.sprite = null;
                GetSpriteRender.sprite = m_spriteArray[m_index];

                // 最後のオブジェクトまで行ったらいったん待機
                if (m_index == (m_spriteCount - 1))
                {
                    m_onStan = true;
                }

                // 経過時間をリセット
                m_elapsedTime = 0f;
            }
        }

        protected override void CollideWithPlayer(BasePlayerController playerController)
        {
            base.CollideWithPlayer(playerController);

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