using System.IO;
using UnityEngine;

[System.Serializable]
public class ClearData
{
    public bool[] Stage;

    public ClearData LoadClearData(ClearData _clearData)
    {
        StreamReader reader;

        #if UNITY_EDITOR
            reader = new StreamReader("Assets/Resources/ClearData.json");
        #else
            reader = new StreamReader(Application.dataPath + "/StreamingAssets/ClearData.json");
        #endif

        string datastr = reader.ReadToEnd();
        reader.Close();
        return JsonUtility.FromJson<ClearData>(datastr);
    }

    public void Save(ClearData _clearData)
    {
        string jsonstr = JsonUtility.ToJson(_clearData);
        StreamWriter writer;

        #if UNITY_EDITOR
            writer = new StreamWriter("Assets/Resources/ClearData.json", false);
        #else
            writer = new StreamWriter(Application.dataPath + "/StreamingAssets/ClearData.json", false);
        #endif

        writer.WriteLine(jsonstr);
        writer.Flush();
        writer.Close();
    }

    public void SetClearFlag(ClearData _clearData)
    {
        _clearData.Stage[GlobalVariables.selectStageNumber] = true;
    }

    public void ResetClearFlag(ClearData _clearData)
    {
        for (int i = 0; i < _clearData.Stage.Length; i++)
        {
            _clearData.Stage[i] = false;
        }
    }

    public void SetAllClear(ClearData _clearData)
    {
        for (int i = 0; i < _clearData.Stage.Length; i++)
        {
            _clearData.Stage[i] = true;
        }
    }

    public bool GetClearFlag(ClearData _clearData, int _stageNum)
    {
        return _clearData.Stage[_stageNum];
    }
    public bool GetAllClear(ClearData _clearData)
    {
        bool isAllClear = true;

        for (int i = 0; i < _clearData.Stage.Length; i++)
        {
            if (!_clearData.Stage[i])
            {
                isAllClear = false;
                break;
            }
        }

        return isAllClear;
    }
}