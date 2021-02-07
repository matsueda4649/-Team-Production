#if UNITY_EDITOR
using UnityEditor;
using System.Reflection;
using System;

static class GameViewUtil
{
    /// <summary>
    /// 設定する画面サイズ
    /// </summary>
    private static float SCALE = 0.307f;

    [InitializeOnLoadMethod]
    static void CheckPlaymodeState()
    {
        // プレイモードが変わったときのコールバックに登録する
        EditorApplication.playModeStateChanged += mode =>
        {
            // Playモードに変わったときに処理する
            if (mode == PlayModeStateChange.EnteredPlayMode)
            {
                var asm = typeof(Editor).Assembly;
                var type = asm.GetType("UnityEditor.GameView");
                EditorWindow gameView = EditorWindow.GetWindow(type);

                // GameViewクラスのSnapZoomプライベートインスタンスメソッドを引数1で呼び出す
                var flag = BindingFlags.NonPublic | BindingFlags.Instance;
                type.GetMethod("SnapZoom", flag, null, new Type[] { typeof(float) }, null).Invoke(gameView, new object[] { SCALE });
            }
        };
    }
}
#endif