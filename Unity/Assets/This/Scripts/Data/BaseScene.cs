using UnityEngine;
using Cysharp.Threading.Tasks;
using ManagerSystem;
using static DefTime;

public abstract class BaseScene : MonoBehaviour
{
    #region メソッド

    protected virtual void Awake()
    {
        GameSceneManager.Instance.SetLoadedScene(this);
    }

    /// <summary>
    /// Scene名の取得
    /// </summary>
    /// <returns></returns>
    public abstract string GetSceneName();

    /// <summary>
    /// シーンの初期化
    /// </summary>
    /// <returns></returns>
    public virtual async UniTask Initialize()
    {
        await UniTask.Yield(PlayerLoopTiming.Update);
    }

    /// <summary>
    /// シーンの開始
    /// </summary>
    /// <returns></returns>
    public virtual async UniTask StartScene()
    {
        await UniTask.Yield(PlayerLoopTiming.Update);
    }

    /// <summary>
    /// シーンの終了
    /// </summary>
    /// <returns></returns>
    public virtual async UniTask TerminateScene()
    {
        await UniTask.Delay(DELAY_TIME);
    }

    #endregion
}