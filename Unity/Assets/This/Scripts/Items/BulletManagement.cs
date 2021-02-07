using System.Collections.Generic;
using UnityEngine;
using static ExtensionMathf;

namespace ItemSystem
{
    /// <summary>
    /// 弾の管理
    /// </summary>
    public class BulletManagement : MonoBehaviour
    {
        #region 変数宣言

        /// <summary>
        /// 攻撃用の弾のプレハブ
        /// </summary>
        [SerializeField] NormalBullet[] m_bulletPrefabs = default;

        /// <summary>
        /// 生成した弾のプレハブのリスト
        /// </summary>
        private List<NormalBullet> m_bulletList = new List<NormalBullet>(DefsBullet.NORMAL_MAX_COUNT);

        /// <summary>
        /// 使用するBulletの数
        /// </summary>
        private int m_length;

        #endregion

        #region メソッド

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            m_length = m_bulletPrefabs.Length;

            // 最初にある程度弾を生成しておく
            for(int i = 0; i < DefsBullet.NORMAL_MAX_COUNT; ++i)
            {
                // 新しい 弾のプレハブを生成
                int index = RandomInt(max: m_length - 1);
                NormalBullet newBullet = CreateNewBullet(m_bulletPrefabs[index], this.transform.parent);
                m_bulletList.Add(newBullet);
            }
        }

        /// <summary>
        /// 未使用の弾を返す
        /// </summary>
        /// <returns> 未使用の弾のプレハブ </returns>
        public NormalBullet GetBullet()
        {
            for (int i = 0, count = m_bulletList.Count; i < count; ++i)
            {
                // 弾が未使用(非アクティブ)だったら
                if(m_bulletList[i].gameObject.activeSelf == false)
                {
                    // 弾のプレハブを渡す
                    return m_bulletList[i];
                }
            }

            // 新しく弾のプレハブを生成する
            int index = RandomInt(max: m_length - 1);
            var newBullet = CreateNewBullet(m_bulletPrefabs[index], transform.parent);
            m_bulletList.Add(newBullet);

            return newBullet;
        }

        /// <summary>
        /// 新しいBulletを生成
        /// </summary>
        /// <param name="bullet"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        private static NormalBullet CreateNewBullet(NormalBullet bullet, Transform parent)
        {
            var newBullet = Instantiate(bullet, parent);
            newBullet.Initialize();
            newBullet.gameObject.Deactovation();
            return newBullet;
        }

        #endregion
    }
}