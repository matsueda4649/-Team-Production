using System.Diagnostics;
using ManagerSystem;
using UnityEngine;
using WindowSystem;

/// <summary>
/// デバッグ担当
/// </summary>
public static class DebugManager
{
    #region 変数宣言

    /// <summary>
    /// アプリ終了の確認用のダイアログ
    /// </summary>
    private static ChoicesDialogWindow m_endAppDialog = null;

    public static ChoicesDialogWindow EndAppDialog
    {
        get => m_endAppDialog;
    }

    /// <summary>
    /// タイトルに戻るときの確認用のダイアログ
    /// </summary>
    private static ChoicesDialogWindow m_returnToTitleDialg = null;

    public static ChoicesDialogWindow ReturnToTitleDialg
    {
        get => m_returnToTitleDialg;
    }

    #endregion

    #region メソッド

    /// <summary>
    /// デバッグログ表示
    /// </summary>
    /// <param name="comment"> コメント </param>
    [Conditional("UNITY_EDITOR")]
    public static void Log(object comment)
    {
        UnityEngine.Debug.Log(comment);
    }

    /// <summary>
    /// エラーログ表示
    /// </summary>
    /// <param name="comment"> コメント </param>
    [Conditional("UNITY_EDITOR")]
    public static void LogError(object comment)
    {
        UnityEngine.Debug.LogError(comment);
    }

    /// <summary>
    /// エラーログ表示
    /// </summary>
    public static void Error(object comment)
    {
        UIManager.Instance.ShowYesDialog(2000, $"ERROR : \n {comment}", "Close", () => {

#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
                UnityEngine.Application.Quit();
#elif UNITY_ANDROID
                Application.runInBackground = false;
                Application.Quit();
#endif
        });
    }

    /// <summary>
    /// アプリを終了する
    /// </summary>
    public static void EndApp()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (m_endAppDialog.IsNull())
            {
                m_endAppDialog = UIManager.Instance.ShowChoiceDialog(1005, "アプリを終了しますか", "はい", "いいえ", () => {
                    m_endAppDialog = null;
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
                    UnityEngine.Application.Quit();
#elif UNITY_ANDROID
                    Application.runInBackground = false;
                    Application.Quit();
#endif
                }, () => {
                    m_endAppDialog = null;
                });
            }
            else
            {
                m_endAppDialog.Close();
            }
        }
    }

    /// <summary>
    /// タイトルに戻る
    /// </summary>
    public static void ReturnToTitle()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (m_returnToTitleDialg.IsNull())
            {
                m_returnToTitleDialg = UIManager.Instance.ShowChoiceDialog(1005, "   タイトルへ戻ります。\nよろしいですか？", "はい", "いいえ", async () => {
                    m_returnToTitleDialg = null;
                    await GameSceneManager.Instance.LoadScene("TitleScene");
                }, () => { m_returnToTitleDialg = null; });
            }
            else
            {
                m_returnToTitleDialg.Close();
            }
        }
    }

    #endregion
}
