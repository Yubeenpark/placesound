using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
    [System.Serializable]
    public class SaveData
    {
        [System.Serializable]
        public struct EnemyData
        {
            public string m_Uuid;
            public int m_Health;
        }

        public int m_Score;
        public List<EnemyData> m_EnemyData = new List<EnemyData>();

        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }

        public void LoadFromJson(string a_Json)
        {
            JsonUtility.FromJsonOverwrite(a_Json, this);
        }
    }

    public interface ISaveable
    {
        void PopulateSaveData(SaveData a_SaveData);
        void LoadFromSaveData(SaveData a_SaveData);
    }

    public static void SaveJsonData(IEnumerable<ISaveable> a_Saveables)
    {
        SaveData sd = new SaveData();
        foreach (var saveable in a_Saveables)
        {
            saveable.PopulateSaveData(sd);
        }

        if (ReadFile.WriteToFile("SaveData01.dat", sd.ToJson()))
        {
            Debug.Log("Save successful");
        }
    }

    public static void LoadJsonData(IEnumerable<ISaveable> a_Saveables)
    {
        if (ReadFile.LoadFromFile("SaveData01.dat", out var json))
        {
            SaveData sd = new SaveData();
            sd.LoadFromJson(json);

            foreach (var saveable in a_Saveables)
            {
                saveable.LoadFromSaveData(sd);
            }

            Debug.Log("Load complete");
        }
    }
}
