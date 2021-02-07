using System;
using UnityEngine;
using static ExtensionComponent;

namespace ManagerSystem
{
    /// <summary>
    /// FixedUpdateを管理
    /// </summary>
    public class FixedUpdateManager : MonoBehaviour
    {
        /// <summary>
        /// 配列の初期サイズ
        /// </summary>
        private const int INITIAL_SIZE = 20;

        #region 変数宣言

        /// <summary>
        /// 配列の大きさを縮小するかどうか
        /// </summary>
        [SerializeField] bool m_resizeMin = false;

        /// <summary>
        /// FiexedUpdateをするオブジェクトの配列
        /// </summary>
        private IFixedUpdatable[] m_updatableArray = new IFixedUpdatable[INITIAL_SIZE];

        /// <summary>
        /// 配列の数
        /// </summary>
        private int m_length = 0;

        private static FixedUpdateManager m_manager = null;

        /// <summary>
        /// 配列のサイズを変更するかどうか
        /// </summary>
        public static bool ReduceArraySizeNeed
        {
            get => m_manager.m_resizeMin;
            set => m_manager.m_resizeMin = value;
        }

        #endregion

        #region メソッド

        private void Awake()
        {
            if (!m_manager)
            {
                FindTypeComponent(ref m_manager);
                if (!m_manager)
                {
                    MainManager main = null;
                    this.SetComponent(ref main);
                    m_manager = main.SafeGetComponent<FixedUpdateManager>();
                }
            }

            if (m_manager && m_manager != this) { Destroy(this); }
        }

        private void FixedUpdate()
        {
            for (int i = 0; i < m_length; ++i)
            {
                if (m_updatableArray[i] == null) { continue; }
                m_updatableArray[i].FixedUpdateMe();
            }
        }

        /// <summary>
        /// 配列に追加
        /// </summary>
        public static void AddUpdatable(IFixedUpdatable updatable)
        {
            if (updatable == null) { return; }
            m_manager.Add(updatable);
        }

        /// <summary>
        /// 配列から除外する
        /// </summary>
        public static void RemoveUpdatable(IFixedUpdatable updatable)
        {
            if (updatable == null) { return; }
            m_manager.Remove(updatable);
        }

        /// <summary>
        /// 配列を整理する
        /// </summary>
        public static void RefreshUpdatableArray()
        {
            m_manager.Refresh();
        }

        private void Add(IFixedUpdatable updatable)
        {
            if (m_updatableArray.Length == m_length)
            {
                // 配列のサイズが大きくなったら変更する
                Array.Resize(ref m_updatableArray, checked(m_length * 2));
            }

            m_updatableArray[m_length++] = updatable;
        }

        private void Remove(IFixedUpdatable updatable)
        {
            for (int i = 0, length = m_updatableArray.Length; i < length; ++i)
            {
                if (m_updatableArray[i] == updatable)
                {
                    m_updatableArray[i] = null;
                    Refresh();
                    return;
                }
            }
        }

        private void Refresh()
        {
            var j = m_length - 1;

            for (int i = 0, length = m_updatableArray.Length; i < length; i++)
            {
                if (m_updatableArray[i] == null)
                {
                    while (i < j)
                    {
                        var updatable = m_updatableArray[j];
                        if (updatable != null)
                        {
                            m_updatableArray[i] = updatable;
                            m_updatableArray[j] = null;
                            j--;
                            goto NEXTLOOP;
                        }
                        j--;
                    }

                    m_length = i;
                    break;
                }

            NEXTLOOP:
                continue;
            }

            // 配列のサイズを縮小する
            if (m_resizeMin && m_length < m_updatableArray.Length / 2)
            {
                Array.Resize(ref m_updatableArray, m_updatableArray.Length / 2);
            }
        }

        #endregion
    }
}