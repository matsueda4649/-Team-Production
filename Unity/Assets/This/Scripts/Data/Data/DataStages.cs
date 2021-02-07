using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR        
using UnityEditor;
#endif

/// <summary>
/// ステージデータの管理
/// </summary>
[System.Serializable]
public class DataStages : Data
{
    #region 変数宣言

    [SerializeField] List<DataStage> m_dataStageList = new List<DataStage>(48);
    public List<DataStage> GetList
    {
        get { return m_dataStageList; }
    }

    private Dictionary<int, DataStage> m_dictionary = new Dictionary<int, DataStage>(48);
    public Dictionary<int, DataStage> GetDictionary
    {
        get { return m_dictionary; }
    }

    #endregion

#if UNITY_EDITOR
    #region Editor 処理

    [MenuItem("Data/Stage")]
    public static void CreateDataStage()
    {
        var stageAsset = CreateInstance<DataStages>();

        // データを作成
        AssetDatabase.CreateAsset(stageAsset, "Assets/Resources/Data/NewStageData.asset");

        // 初期化
        AssetDatabase.Refresh();
    }

    [CustomEditor(typeof(DataStages))]
    public class QuestOnInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            DataStages dataStages = (DataStages)this.target;

            // データを保存
            SaveData(dataStages, this.target);

            // レイアウト
            DisplayLabel();

            // データを管理   
            ManageData(dataStages);

            // データを追加
            AddData(dataStages);
        }

        /// <summary>
        /// ラベルの表示
        /// </summary>
        private static void DisplayLabel()
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField(" □ ", GUILayout.Width(60));        // 空白

                EditorGUILayout.LabelField("Id", GUILayout.Width(40));          // ID

                EditorGUILayout.LabelField("Name", GUILayout.Width(100));       // 名前

                EditorGUILayout.LabelField("Next", GUILayout.Width(40));        // 次のクエスト

                EditorGUILayout.LabelField("Icon", GUILayout.Width(80));        // アイコン

                EditorGUILayout.LabelField("BGM", GUILayout.Width(80));         // BGM

                EditorGUILayout.LabelField("LimitTime", GUILayout.Width(70));   // 制限時間
            }
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// テキストの表示
        /// </summary>
        /// <param name="dataStage"> データ </param>
        private static void DisplayData(DataStage dataStage)
        {

            dataStage.id     = EditorGUILayout.IntField(dataStage.id, GUILayout.Width(40));
          
            dataStage.name   = EditorGUILayout.TextField(dataStage.name, GUILayout.Width(100));

            dataStage.nextId = EditorGUILayout.IntField(dataStage.nextId, GUILayout.Width(40));

            dataStage.icon   = (Sprite)EditorGUILayout.ObjectField(dataStage.icon, typeof(Sprite), true, GUILayout.Width(80));

            dataStage.audio  = (AudioBgm)EditorGUILayout.EnumPopup(dataStage.audio, GUILayout.Width(80));

            float limitTime  = Mathf.Clamp(dataStage.limitTime, 10f, 999f);
            dataStage.limitTime = EditorGUILayout.FloatField(limitTime, GUILayout.Width(70));
        }

        /// <summary>
        /// データの保存
        /// </summary>
        /// <param name="dataStages"> データ </param>
        private static void SaveData(DataStages dataStages, Object target)
        {
            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("□   保存   □", GUILayout.Width(70)))
                {
                    // listを昇順に変更
                    dataStages.GetList.Sort((a, b) => a.id - b.id);

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
        /// <param name="dataStages"> データ </param>
        private static void ManageData(DataStages dataStages)
        {
            for (int i = 0, count = dataStages.GetList.Count; i < count; ++i)
            {
                var dataQuest = dataStages.GetList[i];

                EditorGUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("□ 削除 □", GUILayout.Width(60)))
                    {
                        dataStages.GetList.Remove(dataQuest);
                        break;
                    }
                    DisplayData(dataQuest);
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        /// <summary>
        /// データの追加
        /// </summary>
        /// <param name="dataStages"> データ </param>
        private static void AddData(DataStages dataStages)
        {
            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("□ 追加 □", GUILayout.Width(60)))
                {
                    dataStages.GetList.Add(new DataStage());
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
        m_dataStageList.TrimExcess();

        for (int i = 0, count = m_dataStageList.Count; i < count; ++i)
        {
            // キーが違ったら
            if (!m_dictionary.ContainsKey(m_dataStageList[i].id))
            {
                // ディクショナリーに追加
                m_dictionary.Add(m_dataStageList[i].id, m_dataStageList[i]);
            }
        }
    }

    #endregion
}