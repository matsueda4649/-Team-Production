using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ControllerSystem;
using ManagerSystem;

namespace CharacterSystem
{
    /// <summary>
    /// 倍になる敵
    /// </summary>
    public class DoubleEnemyController : BaseEnemyController, IUpdatable
    {
        #region 変数宣言

        [SerializeField] DoubleEnemyData m_data = default;

        /// <summary>
        /// 影分身のリスト
        /// </summary>
        private IList<DoubleEnemyController> m_childList = new List<DoubleEnemyController>(5);

        #endregion

        #region メソッド

        protected override void Initialize()
        {
            switch (m_data.GetTpe)
            {
                case DoubleEnemyData.Type.Size:
                    break;
                case DoubleEnemyData.Type.Speed:
                    break;
                case DoubleEnemyData.Type.Number:

                    for(int i = 0; i < m_data.GetMagnification; ++i)
                    {

                    }

                    break;
            }
        }

        public void UpdateMe()
        {

        }

        public void OnEnable()
        {
            UpdateManager.AddUpdatable(this);
        }

        public void OnDisable()
        {
            UpdateManager.RemoveUpdatable(this);
        }

        #endregion
    }
}