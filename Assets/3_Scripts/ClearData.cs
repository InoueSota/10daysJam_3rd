using System.IO;
using UnityEngine;

[System.Serializable]
public class ClearData
{
    public bool Stage1;
    public bool Stage2;
    public bool Stage3;
    public bool Stage4;
    public bool Stage5;
    public bool Stage6;

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
        if (GlobalVariables.selectStageNumber == 0)
        {
            _clearData.Stage1 = true;
        }
        else if (GlobalVariables.selectStageNumber == 1)
        {
            _clearData.Stage2 = true;
        }
        else if (GlobalVariables.selectStageNumber == 2)
        {
            _clearData.Stage3 = true;
        }
        else if (GlobalVariables.selectStageNumber == 3)
        {
            _clearData.Stage4 = true;
        }
        else if (GlobalVariables.selectStageNumber == 4)
        {
            _clearData.Stage5 = true;
        }
        else if (GlobalVariables.selectStageNumber == 5)
        {
            _clearData.Stage6 = true;
        }
    }
}