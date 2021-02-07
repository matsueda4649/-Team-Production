using System.Collections.Generic;
using ControllerSystem;
using ManagerSystem;
using UnityEngine;

namespace CharacterSystem
{
    /// <summary>
    /// 回転しながら移動する
    /// </summary>
    [RequireComponent(typeof( Rigidbody2D ), typeof( CircleCollider2D ))]
    public class RollingEnemy : BaseEnemyController, IFixedUpdatable
    {
        #region 変更宣言

        /// <summary>
        /// データ
        /// </summary>
        [SerializeField] MovingEnemyData m_data = default;

        /// <summary>
        /// Playerの強制ゲームオーバー判定
        /// </summary>
        [SerializeField] bool m_forcedDeath = false;

        /// <summary>
        /// 移動方向
        /// </summary>
        [SerializeField] Direction m_direction = Direction.Left;

        /// <summary>
        /// フラグのリスト
        /// </summary>
        private IList<FlagController> m_flagList = new List<FlagController>(5);

        private RectTransform m_rect = null;

        private Rigidbody2D m_rb = null;

        #endregion

        #region メソッド

        protected override void Initialize()
        {
            this.SetComponent( ref m_rect );
            this.SetComponent( ref m_rb );

            GetSpriteRender.color = m_forcedDeath ? Color.red : GetSpriteRender.color;
            
            /*
             * AddForceで移動する場合は不必要
             */
            m_rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        }

        public void FixedUpdateMe()
        {
            if (GetSpriteRender.isVisible || m_data.GetAllowAction)
            {
                RollOver();
                MoveMotion();
            }
        }

        /// <summary>
        /// 転がる
        /// </summary>
        private void RollOver()
        {
            var angle = GetRotationAngle(m_direction, GetSpriteRender.transform);
            GetSpriteRender.transform.SetLocalRotation(angle, Vector3.forward);
        }

        /// <summary>
        /// 回転角度の取得
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="child"></param>
        /// <returns></returns>
        private static float GetRotationAngle(Direction direction, Transform child)
        {
            var angle = child.localEulerAngles.z - (int)direction;
            return angle * Mathf.Ceil(Time.deltaTime);
        }

        /// <summary>
        /// 移動モーション
        /// </summary>
        private void MoveMotion()
        {
            var translation = GetTranslation(m_direction, m_data.GetMoveSpeed);

            /*
             * 移動方法
             */

            this.transform.Translate(translation, Space.World);
            //m_rb.ConstantAddForceX(translation);
        }

        /// <summary>
        /// 移動ベクトルを取得
        /// </summary>
        /// <param name="direction">移動方向</param>
        /// <param name="moveSpeed">移動速度</param>
        /// <returns>移動ベクトル</returns>
        private static Vector3 GetTranslation(Direction direction, float moveSpeed)
        {
            var translation = Vector3.right * (int)direction * moveSpeed;
            return translation * Time.deltaTime;
        }

        protected override void CollideWithFlag(FlagController flagController)
        {
            if ( flagController.GetData.OnWall ) 
            { 
                ReverseDirection( ref m_direction ); 
            }

            if(flagController.GetData.OnGround)
            {
                if (m_flagList.IsEmpty()) { m_flagList.Add(flagController); return; }
                if (!m_flagList.Contains(flagController))
                {
                    m_flagList.Add(flagController);
                    if (GetSpriteRender.isVisible)
                    {
                        AudioManager.Instance.PlayVoice(AudioVoice.Stab);
                    }
                }
            }
        }

        /// <summary>
        /// 移動方向を反転する
        /// </summary>
        private static void ReverseDirection(ref Direction direction)
        {
            int dir = (int)direction;
            direction = (Direction)ExtensionMathf.ReverseSign(ref dir);
        }

        protected override void CollideWithPlayer(BasePlayerController playerController)
        {
            base.CollideWithPlayer(playerController);

            if ( m_forcedDeath ) { playerController.DeathMotion(); }
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