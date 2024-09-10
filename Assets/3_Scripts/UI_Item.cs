using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Item : MonoBehaviour
{
    //アイテムマネージャー
    [SerializeField] ItemManager itemeManager;
    //取得したアイテム格納
    [SerializeField] List<GameObject> items;
    [SerializeField] List<GameObject> itemPostions;
    // Start is called before the first frame update
    void Start()
    {
        items.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        //if (items[0] != null)
        //{
        //    //アイテム一個目

        //}
        
    }

    public void Initialize()
    {
        //リストをなくす。
        items.Clear ();
    }
    public void addList()
    {

    }

}
