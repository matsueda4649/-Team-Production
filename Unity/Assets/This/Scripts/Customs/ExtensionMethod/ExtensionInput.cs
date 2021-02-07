#if UNITY_EDITOR
#define IS_EDITOR // Editorかどうか
#endif
using UnityEngine;

/// <summary>
/// Inputの拡張
/// </summary>
public static class ExtensionInput
{
    #region 変数宣言

    /// <summary>
    /// 座標
    /// </summary>
    private static Vector3 InputPosition = Vector3.zero;

    #endregion

    #region メソッド

    /// <summary>
    /// Input情報を取得
    /// </summary>
    /// <returns>入力がない場合 None</returns>
    private static InputType GetInputType()
    {
#if IS_EDITOR
        if (Input.GetMouseButtonDown(0))    { return InputType.Began; }
        else if (Input.GetMouseButton(0))   { return InputType.Moved; }
        else if (Input.GetMouseButtonUp(0)) { return InputType.Ended; }
#else
        if (Input.touchCount > 0) { return (InputType)((int)Input.GetTouch(0).phase); }
#endif
        return InputType.None;
    }

    /// <summary>
    /// 座標を取得
    /// </summary>
    /// <returns></returns>
    public static Vector3 GetInputPosition()
    {
#if IS_EDITOR
        var type = GetInputType();
        if (type != InputType.None) { return Input.mousePosition; }
#else
        if(Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            return InputPosition = touch.position;
        }
#endif
        return Vector3.zero;
    }

    /// <summary>
    /// ワールド座標を取得
    /// </summary>
    /// <param name="camera">カメラ</param>
    /// <param name="z">Z座標</param>
    /// <returns>入力されていない場合 (0, 0, 0)</returns>
    public static Vector3 GetInputWorldPosition(Camera camera, float z)
    {
        var position = new Vector3(GetInputPosition().x, GetInputPosition().y, z);
        return camera.ScreenToWorldPoint(position);
    }

    #endregion
}
