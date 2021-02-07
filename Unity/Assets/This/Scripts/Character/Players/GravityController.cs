using UnityEngine;

namespace ControllerSystem
{
    public static class GravityController
    {
        #region 変数宣言

        /// <summary>
        /// 万有引力の定数
        /// </summary>
        public static float G = 6.6743f;

        /// <summary>
        /// 惑星のおおよその密度
        /// </summary>
        public static float DENSITY = 5.52f;

        #endregion

        #region メソッド

        /// <summary>
        /// 一番近い惑星に向かって重力をかける
        /// </summary>
        /// <param name="player">Playerのゲームオブジェクト</param>
        /// <param name="planetArray">惑星のゲームオブジェクトの配列</param>
        /// <param name="mass">PlayerのMass</param>
        public static Vector3 AddGravity(GameObject player, GameObject[] planetArray, float mass)
        {
            // 初期値設定
            var playerPos = player.transform.position;
            var direction = planetArray[0].transform.localPosition - playerPos;
            var distance = direction.magnitude * direction.magnitude;

            // 惑星の質量の計算
            float gravity = GetPlanetMass(planetArray[0]) * mass / distance;

            // 一番近い惑星との距離を取得する
            for (int i = 1; i < planetArray.Length; ++i)
            {
                var nextDir = planetArray[i].transform.localPosition - playerPos;
                var nextDis = nextDir.magnitude * nextDir.magnitude;

                // 新しい値に変更
                if (distance > nextDis)
                {
                    direction = nextDir;
                    distance = nextDis;
                    gravity = GetPlanetMass(planetArray[i]) * mass / distance;
                }
            }

            direction.Normalize();

            var rot = Quaternion.FromToRotation(-player.transform.up, direction);
            player.transform.localRotation *= rot;

            return G * direction * gravity;
        }

        /// <summary>
        /// 惑星の質量の取得
        /// </summary>
        /// <param name="planet">惑星</param>
        /// <returns></returns>
        private static float GetPlanetMass(GameObject planet)
        {
            var planetScale = planet.transform.localScale;
            var d = planetScale.x * planetScale.y * planetScale.z;
            //return (Mathf.PI / 6) * d * DENSITY / 1000;

            return planet.GetComponent<Rigidbody2D>().mass;
        }

        #endregion
    }
}