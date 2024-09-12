using System.IO;
using UnityEngine;

public class StageClearManager : MonoBehaviour
{
    [System.Serializable]
    public class ClearData
    {
        public bool Stage1;
        public bool Stage2;
        public bool Stage3;
        public bool Stage4;
        public bool Stage5;
    }

    string datapath;

    void Awake()
    {
        datapath = Application.dataPath + "/TestJson.json";
    }

    void Start()
    {
        ClearData clearData = new ClearData();
        clearData.Stage1 = false;
        clearData.Stage2 = true;
        Save(clearData);
    }

    public void Save(ClearData _clearData)
    {
        string jsonstr = JsonUtility.ToJson(_clearData);
        StreamWriter writer = new StreamWriter(datapath, false);

        writer.WriteLine(jsonstr);
        writer.Flush();
        writer.Close();
    }
}