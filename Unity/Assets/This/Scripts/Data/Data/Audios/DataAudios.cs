using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR        
using UnityEditor;
#endif

[System.Serializable]
public class DataAudios : Data
{
    #region 変数宣言

    [SerializeField] List<DataAudio> m_dataAudioList = new List<DataAudio>(20);
    public List<DataAudio> GetList
    {
        get { return m_dataAudioList; }
    }

    private Dictionary<int, DataAudio> m_dictionary = new Dictionary<int, DataAudio>(20);
    public Dictionary<int, DataAudio> GetDictionary
    {
        get { return m_dictionary; }
    }

    #endregion

#if UNITY_EDITOR

    #region Editor 処理

    [MenuItem("Data/Audio")]
    public static void CreateAudioData()
    {
        var QusetAsset = CreateInstance<DataAudios>();

        // データを作成
        AssetDatabase.CreateAsset(QusetAsset, "Assets/This/Resources/Data/NewAudioData.asset");

        // 初期化
        AssetDatabase.Refresh();
    }

    [CustomEditor(typeof(DataAudios))]
    public class QuestOnInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            DataAudios audios = (DataAudios)this.target;

            // データを保存
            SaveData(audios, this.target);

            // レイアウト
            Display_Label();

            // データを管理   
            ManageData(audios);

            // データを追加
            AddData(audios);
        }

        /// <summary>
        /// ラベルの表示
        /// </summary>
        private void Display_Label()
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField(" □ ", GUILayout.Width(60));      // 空白

                EditorGUILayout.LabelField("Id", GUILayout.Width(40));        // ID

                EditorGUILayout.LabelField("Name", GUILayout.Width(100));     // 名前

                EditorGUILayout.LabelField("Loop", GUILayout.Width(60));      // ループ

                EditorGUILayout.LabelField("EXplanation", GUILayout.Width(120));  // 説明
            }
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// テキストの表示
        /// </summary>
        /// <param name="dataAudio"> データ </param>
        private static void DisplayData(DataAudio dataAudio)
        {

            dataAudio.id = EditorGUILayout.IntField(dataAudio.id, GUILayout.Width(40));

            dataAudio.name = EditorGUILayout.TextField(dataAudio.name, GUILayout.Width(100));

            dataAudio.onLoop = EditorGUILayout.Toggle(dataAudio.onLoop, GUILayout.Width(40));

            dataAudio.explanation = EditorGUILayout.TextField(dataAudio.explanation, GUILayout.Width(120));
        }

        /// <summary>
        /// データの保存
        /// </summary>
        /// <param name="dataAudios"> データ </param>
        private static void SaveData(DataAudios dataAudios, Object target)
        {
            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("□   保存   □", GUILayout.Width(70)))
                {
                    // listを昇順に変更
                    dataAudios.GetList.Sort((a, b) => a.id - b.id);

                    // 保存
                    EditorUtility.SetDirty(target);

                    AssetDatabase.SaveAssets();
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// データの取得
        /// </summary>
        /// <param name="dataAudios"> データ </param>
        private static void ManageData(DataAudios dataAudios)
        {
            for (int i = 0, count = dataAudios.GetList.Count; i < count; ++i)
            {
                var dataAudio = dataAudios.GetList[i];

                EditorGUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("□ 削除 □", GUILayout.Width(60)))
                    {
                        dataAudios.GetList.Remove(dataAudio);
                        break;
                    }
                    DisplayData(dataAudio);
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        /// <summary>
        /// データの追加
        /// </summary>
        /// <param name="dataAudios"> データ </param>
        private static void AddData(DataAudios dataAudios)
        {
            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("□ 追加 □", GUILayout.Width(60)))
                {
                    dataAudios.GetList.Add(new DataAudio());
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    #endregion

#endif

    #region メソッド

    /// <summary>
    /// 初期化
    /// </summary>
    public override void Initialize()
    {
        m_dataAudioList.TrimExcess();

        for (int i = 0, count = m_dataAudioList.Count; i < count; ++i)
        {
            // キーが違ったら
            if (!m_dictionary.ContainsKey(m_dataAudioList[i].id))
            {
                // ディクショナリーに追加
                m_dictionary.Add(m_dataAudioList[i].id, m_dataAudioList[i]);
            }
        }
    }

    #endregion
}