using System.Collections.Generic;
using UnityEngine;
using static ExtensionMathf;

namespace ItemSystem
{
    public class PrefabManager : MonoBehaviour
    {
        #region 変数宣言

        /// <summary>
        /// 生成するPrefab
        /// </summary>
        [SerializeField] GameObject[] m_prefabs = default;

        /// <summary>
        /// 生成したPrefabのリスト
        /// </summary>
        private List<GameObject> m_prefabList = new List<GameObject>(DefsBullet.NORMAL_MAX_COUNT);

        /// <summary>
        /// 生成するPrefabの種類
        /// </summary>
        private int m_length;

        #endregion

        #region メソッド

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            m_length = m_prefabs.Length;

            for(int i = 0; i < DefsBullet.NORMAL_MAX_COUNT; ++i)
            {
                var index = RandomInt(max: m_length - 1);
                var newPrefab = CreateNewPrefab(m_prefabs[index], this.transform.parent);
                m_prefabList.Add(newPrefab);
            }
        }

        /// <summary>
        /// Prefabを取得する
        /// </summary>
        /// <returns></returns>
        public GameObject GetPrefab()
        {
            for(int i = 0, count = m_prefabList.Count; i < count; ++i)
            {
                if (!m_prefabList[i].activeSelf)
                {
                    return m_prefabList[i];
                }
            }

            int index = RandomInt(max: m_length - 1);
            var newPrefab = CreateNewPrefab(m_prefabList[index], this.transform.parent);
            m_prefabList.Add(newPrefab);
            return newPrefab;
        }

        /// <summary>
        /// 新しいPrefabを生成する
        /// </summary>
        /// <param name="prefab">生成するPrefab</param>
        /// <param name="parent">親オブジェクトの座標</param>
        /// <returns></returns>
        private static GameObject CreateNewPrefab(GameObject prefab, Transform parent)
        {
            var newPrefab = Instantiate(prefab, parent);
            newPrefab.Deactovation();
            return newPrefab;
        }

        #endregion
    }
}