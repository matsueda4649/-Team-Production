using System.Text;

public static class ExtensionString
{
    /// <summary>
    /// 文字列を追加し、取得する
    /// </summary>
    /// <param name="strArray">追加する文字列</param>
    /// <returns></returns>
    public static string ToString(string[] strArray)
    {
        StringBuilder stringBuilder = new StringBuilder(32);
        for(int i = 0, length = strArray.Length; i < length; ++i)
        {
            stringBuilder.Append(strArray[i]);
        }

        return stringBuilder.ToString();
    }
}
