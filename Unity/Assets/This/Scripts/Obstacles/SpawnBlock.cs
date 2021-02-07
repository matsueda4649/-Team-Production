using ControllerSystem;
using DG.Tweening;
using UnityEngine;
using static ExtensionDOTween;

namespace ItemSystem 
{
    /// <summary>
    /// オブジェクトを生成するブロック
    /// </summary>
    public class SpawnBlock : SpecialBlock
    {
        #region 変数宣言

        /// <summary>
        /// スポーンするオブジェクトの親オブジェクト
        /// </summary>
        [SerializeField] GameObject m_spawnObjectParent = default;

        /// <summary>
        /// スポーンさせるオブジェクトの配列
        /// </summary>
        private GameObject[] m_spawnObjectArray;

        /// <summary>
        /// 配列の個数
        /// </summary>
        private int m_length;

        #endregion

        #region メソッド

        protected override void Initialize()
        {
            GetSpawnChildrenObject();
        }

        /// <summary>
        /// スポーンオブジェクトの子を取得
        /// </summary>
        private void GetSpawnChildrenObject()
        {
            if (!m_spawnObjectParent.IsNull())
            {
                m_length = m_spawnObjectParent.transform.childCount;
                m_spawnObjectArray = new GameObject[m_length];
                for(int i = 0; i < m_length; ++i)
                {
                    var child = m_spawnObjectParent.transform.GetChild(i);
                    m_spawnObjectArray[i] = child.gameObject;
                    m_spawnObjectArray[i].Deactovation();
                }
            }
        }

        protected override void TriggerWithPlayer(BasePlayerController playerController)
        {
            base.TriggerWithPlayer(playerController);

            SpawnObject();
        }

        /// <summary>
        /// オブジェクトのスポーン
        /// </summary>
        private void SpawnObject()
        {
            if (m_length > 0)
            {
                for(int i = 0, length = m_spawnObjectArray.Length; i < length; ++i)
                {
                    m_spawnObjectArray[i].Activation();
                }
            }
        }

        #endregion
    }
}