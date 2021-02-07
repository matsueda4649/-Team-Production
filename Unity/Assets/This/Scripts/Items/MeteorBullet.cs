using UnityEngine;

namespace ItemSystem 
{
    public class MeteorBullet : NormalBullet
    {
        #region 変数宣言

        private Rigidbody2D m_rb;

        #endregion

        #region メソッド

        public override void Initialize()
        {
            base.Initialize();
            this.SetComponent(ref m_rb);
        }

        protected override void MovementBullet()
        {
            base.MovementBullet();
        }

        #endregion
    }
}