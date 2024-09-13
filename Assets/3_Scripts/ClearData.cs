using System.IO;
using UnityEngine;

[System.Serializable]
public class ClearData
{
    public bool[] Stage;

    string datapath = "/StreamingAssets/ClearData.json";

    public ClearData LoadClearData(ClearData _clearData)
    {
        StreamReader reader = new StreamReader(Application.dataPath + datapath);
        string datastr = reader.ReadToEnd();
        reader.Close();
        return JsonUtility.FromJson<ClearData>(datastr);
    }

    public void Save(ClearData _clearData)
    {
        string jsonstr = JsonUtility.ToJson(_clearData);
        StreamWriter writer = new StreamWriter(Application.dataPath + datapath, false);
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