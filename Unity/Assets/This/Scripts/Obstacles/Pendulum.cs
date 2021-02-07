using System.Collections;
using System.Collections.Generic;
using ManagerSystem;
using UnityEngine;

namespace ItemSystem 
{
    /// <summary>
    /// 振り子
    /// </summary>
    public class Pendulum : MonoBehaviour, IUpdatable
    {
        #region 変数宣言

        [SerializeField] LineRenderer m_line = default;

        [SerializeField] Transform m_fulcrum = default;

        [SerializeField] Transform m_mg = default;

        private Vector3 m_parent;

        private Vector3 m_fulcrumPos;

        private Vector3 m_mgPos;

        #endregion

        #region メソッド

        private void Start()
        {
            m_parent = this.transform.localPosition;
            m_fulcrumPos = m_fulcrum.localPosition + m_parent;
        }

        public void UpdateMe()
        {
            m_line.SetPosition(0, m_mg.localPosition + m_parent);
            m_line.SetPosition(1, m_fulcrumPos);
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