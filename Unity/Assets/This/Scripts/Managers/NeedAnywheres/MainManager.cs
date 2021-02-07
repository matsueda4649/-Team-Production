using UnityEngine;
using Cysharp.Threading.Tasks;

namespace ManagerSystem
{
    /// <summary>
    /// マネージャーの管理
    /// </summary>
    [RequireComponent(typeof(UpdateManager), typeof(FixedUpdateManager))]
    public class MainManager : SingletonMonoBehaviour<MainManager>
    {
        #region 変数宣言

        [SerializeField] string m_firstLoadScene = "NotesScene";

        #endregion

        #region メソッド

        protected override void Awake()
        {
            Initialize().Forget();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <returns></returns>
        private async UniTask Initialize()
        {
            // DataManagerの初期化
            await DataManager.Instance.Initialize();

            // SaveDataの初期化
            await SaveManager.Instance.Initialize();

            // GameSceneの初期化
            await GameSceneManager.Instance.Initialize();

            // Audioの初期化
            await AudioManager.Instance.Initialize();

            await GameSceneManager.Instance.LoadScene(m_firstLoadScene);
        }

        #endregion
    }
}