using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ManagerSystem 
{
    /// <summary>
    /// 注意事項の管理
    /// </summary>
    public class NotesSceneManager : BaseScene
    {
        public static readonly string SceneName = "NotesScene";

        #region メソッド

        public override string GetSceneName()
        {
            return SceneName;
        }

        public override async UniTask Initialize()
        {
            await SelectLoadScene();
        }

        /// <summary>
        /// 読み込むシーンの選択
        /// </summary>
        private static async UniTask SelectLoadScene()
        {
            //var saveData = SaveManager.Instance.GetSaveData;
            //if (!saveData.onNewGame)
            {
                await GameSceneManager.Instance.LoadScene("TitleScene");
            }
        }

        public void OnClickButton()
        {
            GameSceneManager.Instance.LoadScene("TitleScene").Forget();
        }
        #endregion
    }
}