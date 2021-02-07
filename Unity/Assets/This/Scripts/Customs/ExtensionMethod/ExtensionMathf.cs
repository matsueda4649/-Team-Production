using System;
using System.Collections.Generic;
using ItemSystem;
using UnityEngine;
using static UnityEngine.Mathf;

/// <summary>
/// Mahtfの拡張メソッド
/// </summary>
public static class ExtensionMathf
{
    #region 変数宣言

    #endregion

    #region メソッド

    /// <summary>
    /// 取得した配列から最小値を取得
    /// </summary>
    /// <typeparam name="T">型</typeparam>
    /// <param name="nums">配列</param>
    /// <returns>最小値</returns>
    public static T Min<T>(params T[] nums) where T : IComparable
    {
        if (nums.Length == 0) { return default; }

        T min = nums[0];
        for (int i = 1, length = nums.Length; i < length; ++i)
        {
            min = min.CompareTo(nums[i]) < 0 ? min : nums[i];
        }

        return min;
    }

    /// <summary>
    /// ランダム数を返す
    /// </summary>
    /// <param name="min">最小値</param>
    /// <param name="max">最大値</param>
    /// <returns>ランダム値</returns>
    public static float RandomFloat(float min = 0f, float max = 10f)
    {
        return UnityEngine.Random.Range(min, max);
    }

    /// <summary>
    /// ランダム数を返す (最大値も含む)
    /// </summary>
    /// <param name="min">最小値</param>
    /// <param name="max">最大値</param>
    /// <returns>ランダム値</returns>
    public static int RandomInt(int min = 0, int max = 10)
    {
        return UnityEngine.Random.Range(min, max + 1);
    }

    /// <summary>
    /// 確率判定
    /// </summary>
    /// <param name="percent">確率 (0~100)</param>
    /// <returns>確率判定</returns>
    public static bool Probability(float percent)
    {
        if(percent >= 0f && percent <= 100f)
        {
            float value = UnityEngine.Random.value * 100.0f;

            if (value >= percent) { return true; }
        }

        return false;
    }

    /// <summary>
    /// 符号を変える
    /// </summary>
    /// <param name="value">変えたい値</param>
    /// <returns>正 => 負 || 負 => 正 </returns>
    public static float ReverseSign(ref float value)
    {
        return value *= -1f;
    }

    /// <summary>
    /// 符号を変える
    /// </summary>
    /// <param name="value">変えたい値</param>
    /// <returns>正 => 負 || 負 => 正 </returns>
    public static int ReverseSign(ref int value)
    {
        return value *= -1;
    }

    /// <summary>
    /// 除算
    /// </summary>
    /// <param name="dividend"></param>
    /// <param name="divisor"></param>
    /// <returns></returns>
    public static float Divide(this float dividend, float divisor = 2f)
    {
        return dividend / divisor;
    }

    /// <summary>
    /// 二点の角度の計算
    /// </summary>
    /// <param name="start">開始地点</param>
    /// <param name="end">目標地点</param>
    /// <returns>二点の角度</returns>
    public static float CalculateRadian(Vector2 start, Vector2 end)
    {
        Vector2 dt = end - start;
        float rad = Atan2(dt.x, dt.y);
        float degree = rad * Rad2Deg;

        if(degree < 0f)         { degree += 360f; }
        else if(degree > 180f)  { degree -= 360f; }
        return degree;
    }

    /// <summary>
    /// 抗力の計算
    /// </summary>
    /// <param name="data">データ</param>
    /// <param name="velocity">移動ベクトル</param>
    /// <param name="scale">スケール</param>
    public static float CalculateDrag(SetDragObstacleData data, Vector3 velocity, Vector3 scale)
    {
        // 速度
        float speed = Max(new float[2] { (velocity.x), (velocity.y) });
        // 自身の面積
        float area = scale.x * scale.y;
        // 抗力
        float drag = data.GetDragCoe * 0.5f * data.GetDensity * Pow(speed, 2) * area;

        return CeilToInt(drag);
    }

    /// <summary>
    /// 射出速度の計算
    /// </summary>
    /// <param name="start">開始地点</param>
    /// <param name="target">目的地点</param>
    /// <param name="angle">射出角度</param>
    /// <returns>射出速度</returns>
    public static Vector3 CalculateThrowVelocity(Vector3 start, Vector3 target, float angle)
    {
        // 開始座標
        Vector2 startPos = new Vector2(start.x, start.z);
        // ターゲット座標
        Vector2 targetPos = new Vector2(target.x, target.z);

        // 水平方向の距離
        float x = (startPos - targetPos).magnitude;
        // 垂直方向の距離
        float y = start.y - target.y;

        // ラジアン
        float rad = angle * Deg2Rad;

        // 分母
        float denom = 2 * Pow(Cos(rad), 2) * (y - x * Tan(rad));
        // 分子
        float numer = Physics2D.gravity.y * Pow(x, 2);

        // 初速度
        float speed = Sqrt(Abs(numer / denom));

        Vector3 velocity = new Vector3(target.x - start.x, x * Tan(rad), target.z - start.z);

        return velocity.normalized * speed;
    }

    /// <summary>
    /// 衝突後の速度の計算
    /// </summary>
    /// <param name="before">衝突前の速度</param>
    /// <param name="elasticity">反発係数 <para> 前提 0以上 1以下</para> </param>
    /// <param name="equidistant">等間隔で跳ねるかどうか</param>
    /// <returns></returns>
    public static float CalculateBounceVelocity(float before, float elasticity, bool equidistant = false)
    {
        // 反発係数を0以上1以下に制限する
        elasticity = Clamp01(elasticity);
        // 衝突後の速度
        return equidistant ? -before : -(elasticity * before);
    }

    /// <summary>
    /// 落下速度の計算
    /// </summary>
    /// <param name="self">
    /// 落下させる座標
    /// <para>前提 高さの最小値は0以上</para>
    /// <para>前提 高さ 0基準 </para>
    /// <para>前提 重力はマイナス</para>
    /// </param>
    public static float CalculateFallingSpeed(Vector2 self)
    {
        return Sqrt(2 * Abs(Physics2D.gravity.y * self.y));
    }

    public class RomdomIndexList
    {
        #region 変数宣言

        /// <summary>
        /// インデックスのリスト
        /// </summary>
        private List<int> m_indexList = new List<int>(10);

        #endregion

        #region メソッド

        /// <summary>
        /// Indexリストの初期化
        /// </summary>
        /// <param name="start">開始</param>
        /// <param name="end">終了</param>
        public void InitializeIndexList(int start, int end)
        {
            for (int i = start; i <= end; ++i)
            {
                m_indexList.Add(i);
            }

            m_indexList.TrimExcess();
        }

        /// <summary>
        /// ランダムにIndexを取得する
        /// </summary>
        /// <returns></returns>
        public int GetRomdomIndex()
        {
            var index = UnityEngine.Random.Range(0, m_indexList.Count);

            var ramdom = m_indexList[index];

            m_indexList.RemoveAt(index);

            return ramdom;
        }

        #endregion
    }

    #endregion
}