using UnityEngine;
using System;

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    /// <summary>
    /// インスタンス
    /// </summary>
    private static T m_instance;

    /// <summary>
    /// インタンスを取得
    /// </summary>
    public static T Instance
    {
        get
        {
            if (m_instance.IsNull())
            {
                Type t = typeof(T);

                m_instance = (T)FindObjectOfType(t);

                if (m_instance.IsNull())
                {
                    string message = ExtensionString.ToString(new string[] { t.Name , " をアタッチしているGameObjectはありません" });
                    Debug.LogError(message);
                }
            }

            return m_instance;
        }
    }

    protected virtual void Awake()
    {
        // 他のゲームオブジェクトにアタッチされているか調べる
        // アタッチされている場合は破棄する。
        CheckInstance();
    }

    protected bool CheckInstance()
    {
        if (m_instance.IsNull())
        {
            m_instance = this as T;
            return true;
        }
        else if (Instance == this)
        {
            return true;
        }

        Destroy(this.gameObject);
        return false;
    }

    private void OnDestroy()
    {
        m_instance = null;
    }
}