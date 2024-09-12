using System.IO;
using UnityEngine;

[System.Serializable]
public class ClearData
{
    public bool[] Stage;

    public ClearData LoadClearData(ClearData _clearData)
    {
        StreamReader reader = new StreamReader("Assets/Resources/ClearData.json");
        string datastr = reader.ReadToEnd();
        reader.Close();
        return JsonUtility.FromJson<ClearData>(datastr);
    }

    public void Save(ClearData _clearData)
    {
        string jsonstr = JsonUtility.ToJson(_clearData);
        StreamWriter writer = new StreamWriter("Assets/Resources/ClearData.json", false);
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

    public bool GetClearFlag(ClearData _clearData, int _stageNum)
    {
        return _clearData.Stage[_stageNum];
    }
}