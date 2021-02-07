using System.IO;
using UnityEngine;
using Cysharp.Threading.Tasks;
using static DefTime;
using static DefNum;

namespace ManagerSystem
{
    /// <summary>
    /// セーブデータを管理
    /// </summary>
    public class SaveManager : SingletonMonoBehaviour<SaveManager>
    {
        #region 変数宣言

        /// <summary>
        /// セーブデータ
        /// </summary>
        private SaveData m_saveData;

        /// <summary>
        /// セーブデータ取得
        /// </summary>
        public SaveData GetSaveData
        {
            get
            {
                // データがなければ
                if (m_saveData == null) { Initialize().Forget(); }

                return m_saveData;
            } 
        }

        #endregion

        #region メソッド

        /// <summary>
        /// 初期設定
        /// </summary>
        public async UniTask Initialize()
        {
            // セーブデータの読み込み
            LoadSaveData();

            await UniTask.Delay(DELAY_TIME);
        }

        /// <summary>
        /// セーブデータの読み込み
        /// </summary>
        public void LoadSaveData()
        {
            var path = Application.persistentDataPath + "/.savedata.json";

            if (File.Exists(path))
            {
                // テキストファイルを読み込む
                var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                var streamReader = new StreamReader(fileStream);

                // 一度にすべて読み込む
                string data = streamReader.ReadToEnd();
                streamReader.Close();
                m_saveData = JsonUtility.FromJson<SaveData>(data);
            }
            else
            {
                ResetData();
            }
        }

        /// <summary>
        /// 初期設定
        /// </summary>
        public void ResetData()
        {
            m_saveData = new SaveData();

            // セーブデータを初期化
            m_saveData.Reset();

            // はじめのクエストID
            m_saveData.newStageIds.Add(STAGE_FIRST_NUMBER);
            m_saveData.openStageIds.Add(STAGE_FIRST_NUMBER);

            // 音量の設定
            m_saveData.ResetAudioSetting();

            Save(m_saveData);
        }

        /// <summary>
        /// データの保存
        /// </summary>
        public static void Save(SaveData saveData)
        {
            StreamWriter streamWriter;
            string json = JsonUtility.ToJson(saveData);

            string path = ExtensionString.ToString(new string[] { Application.persistentDataPath, "/.savedata.json" });
            streamWriter = new StreamWriter(path);
            streamWriter.Write(json);
            streamWriter.Flush();
            streamWriter.Close();
        }

        /// <summary>
        /// クリア時のデータ保存
        /// </summary>
        /// <param name="stageId"> クエストID </param>
        /// <param name="clearTime"> ステージのクリア時間 </param>
        public bool SaveClear(int stageId, float clearTime)
        {
            var dataStages = DataManager.Instance.GetDataStages;

            // 新記録判定
            var newRecord = false;

            // データがなければ
            if (dataStages.GetList.IsEmpty()) { DebugManager.Error("Stage = Null"); }

            // IDがあれば
            if (dataStages.GetDictionary.TryGetValue(stageId, out DataStage dataStage))
            {
                newRecord = AddClearStageId(stageId, clearTime);

                int nexId = dataStage.nextId;
                for (int i = 0, count = dataStages.GetList.Count; i < count; ++i)
                {
                    var data = dataStages.GetList[i];

                    // 次のクエストがあれば
                    if (data.id == nexId)
                    {
                        AddOpenStageId(data.id);

                        AddNewStageId(data.id);

                        break;
                    }
                }

                Save(m_saveData);

                return newRecord;
            }

            return newRecord;
        }

        /// <summary>
        /// クリアしたステージのIDを追加
        /// </summary>
        /// <param name="id">クリアしたステージのID</param>
        /// <param name="clearTime"> ステージのクリア時間 </param>
        private bool AddClearStageId(int id, float clearTime)
        {
            if (!m_saveData.clearStageIds.Contains(id))
            {
                // IDを追加
                m_saveData.clearStageIds.Add(id);
            }

            return m_saveData.score.UpdateScore(id, clearTime);
        }

        /// <summary>
        /// 解放済みステージのIDを追加
        /// </summary>
        /// <param name="id">解放しているステージのID</param>
        private void AddOpenStageId(int id)
        {
            if (!m_saveData.openStageIds.Contains(id))
            {
                m_saveData.openStageIds.Add(id);
            }
        }

        /// <summary>
        /// 新規開放したステージのIDを追加
        /// </summary>
        /// <param name="id">新規開放するステージのID</param>
        private void AddNewStageId(int id)
        {
            // 新規開放クエストのリストにIDがなければ
            if (!m_saveData.newStageIds.Contains(id))
            {
                m_saveData.newStageIds.Add(id);
            }
        }

        #endregion
    }
}