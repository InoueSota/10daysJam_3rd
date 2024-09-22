using System.Collections.Generic;
using UnityEditorInternal.VersionControl;
using UnityEngine;

public class UndoManager : MonoBehaviour
{
    // Undoを管理するリスト
    public List<UndoState> listUndo = new List<UndoState>();
    // オブジェクトの状態を管理するリスト
    public List<AllObjectManager> list_AllObjects = new List<AllObjectManager>();
    public List<int> list_HP = new List<int>();
    public List<Vector3> list_Positions = new List<Vector3>();
    public List<bool> list_IsActive = new List<bool>();
    public List<bool> list_Sprites = new List<bool>();
    public List<BoxCollider2D> list_collider = new List<BoxCollider2D>();
    Vector3 playerPos;
    public List<Vector3> list_Scale;
    UI_Item itemsScript;
    public List<bool> uI_Items;
    public List<bool> uI_Items_isOne;
    // Start is called before the first frame update
    void Start()
    {
        // 最初とリセット時にオブジェクトの情報取得
        //ブロック関連
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Object"))
        {
            AllObjectManager allObjectManager = obj.GetComponent<AllObjectManager>();
            list_AllObjects.Add(allObjectManager);
            list_HP.Add(allObjectManager.GetHp());
            list_Positions.Add(obj.transform.position);
            list_IsActive.Add(allObjectManager.GetIsActive());
            list_Sprites.Add(obj.GetComponent<SpriteRenderer>().enabled);
            list_collider.Add(obj.GetComponent<BoxCollider2D>());
            list_Scale.Add(obj.transform.localScale);
        }
        //プレイヤー関連
        playerPos = GameObject.FindWithTag("Player").gameObject.transform.position;
        itemsScript = GameObject.FindWithTag("items").GetComponent<UI_Item>();
        //uI_Items =new List<bool> {false, false, false };
        uI_Items_isOne = new List<bool> { false, false, false };
        UndoSave();
    }

    public void UndoReSet()
    {
        listUndo.Clear();
    }

    public void UndoSave()
    {
        // 現在のリストの状態を保存
        for (int i = 0; i < list_AllObjects.Count; i++)
        {
            list_HP[i] = list_AllObjects[i].GetHp();
            list_IsActive[i] = list_AllObjects[i].GetIsActive();
            list_Sprites[i] = list_AllObjects[i].GetComponent<SpriteRenderer>().enabled;

            if (list_IsActive[i])
            {
                list_Scale[i] = list_AllObjects[i].gameObject.transform.localScale;
            }
        }

        uI_Items = new List<bool>(itemsScript.items); // リストのコピー
        uI_Items_isOne = new List<bool>(itemsScript.isOne); // リストのコピー

        // 新しい状態を作成 (ディープコピー)
        UndoState newState = new UndoState
        {
            // ブロック関連
            list_AllObjects = new List<AllObjectManager>(list_AllObjects),
            list_IsActive = new List<bool>(list_IsActive),
            list_Sprites = new List<bool>(list_Sprites),
            list_HP = new List<int>(list_HP),
            list_Scale = new List<Vector3>(list_Scale),
            list_Positions = new List<Vector3>(list_Positions),
            // プレイヤー関連
            playerPos = GameObject.FindWithTag("Player").gameObject.transform.position,
            uI_Items = new List<bool>(uI_Items),
            uI_Items_isOne = new List<bool>(uI_Items_isOne),

        };

        Debug.Log("Saving UI_Items: " + string.Join(", ", newState.uI_Items));
        Debug.Log("Saving UI_Items_isOne: " + string.Join(", ", newState.uI_Items_isOne));

        // Undo状態をリストに追加
        listUndo.Add(newState); // これで参照ではなく、リストのコピーを保存
    }

    public void UndoLoad()
    {
        if (listUndo.Count > 0)
        {
            // 保存されている値を取り出す
            UndoState lastState = listUndo[listUndo.Count - 1];

            for (int i = 0; i < list_AllObjects.Count; i++)
            {
                //ブロック関連

                list_AllObjects[i].gameObject.transform.localScale = lastState.list_Scale[i];
                list_AllObjects[i].gameObject.transform.Rotate(Vector3.zero);
                list_AllObjects[i] = lastState.list_AllObjects[i];
                list_Positions[i] = lastState.list_Positions[i];
                list_IsActive[i] = lastState.list_IsActive[i];
                list_Sprites[i] = lastState.list_Sprites[i];
                list_AllObjects[i].SetHp(lastState.list_HP[i]);
                //list_collider[i] = lastState.list_collider[i];
                list_AllObjects[i].SetIsActive(lastState.list_IsActive[i]);
                list_AllObjects[i].GetComponent<SpriteRenderer>().enabled = lastState.list_Sprites[i];
                //list_AllObjects[i].GetComponent<BoxCollider2D>().enabled = list_collider[i];
            }
            //プレイヤー関連
            GameObject.FindWithTag("Player").transform.position = lastState.playerPos;
            //itemの表示を消す

            // itemsScript.DestoryObjs.Clear();
            itemsScript.items.Clear();
            itemsScript.isOne.Clear();
            itemsScript.items.AddRange(lastState.uI_Items);
            itemsScript.isOne.AddRange(lastState.uI_Items_isOne);


            itemsScript.UndoUIItems();
            listUndo.RemoveAt(listUndo.Count - 1); // Listの最後の要素を削除する
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < list_AllObjects.Count; i++)
        {
            list_IsActive[i] = list_AllObjects[i].GetIsActive();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            UndoLoad();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            UndoSave();
        }
    }

    public void UndoSaveItem(Vector3 pos)
    {
        // 現在のリストの状態を保存
        for (int i = 0; i < list_AllObjects.Count; i++)
        {
            list_HP[i] = list_AllObjects[i].GetHp();
            list_IsActive[i] = list_AllObjects[i].GetIsActive();
            list_Sprites[i] = list_AllObjects[i].GetComponent<SpriteRenderer>().enabled;

            if (list_IsActive[i])
            {
                list_Scale[i] = list_AllObjects[i].gameObject.transform.localScale;
            }
        }

        uI_Items = new List<bool>(itemsScript.items); // リストのコピー
        uI_Items_isOne = new List<bool>(itemsScript.isOne); // リストのコピー

        // 新しい状態を作成 (ディープコピー)
        UndoState newState = new UndoState
        {
            // ブロック関連
            list_AllObjects = new List<AllObjectManager>(list_AllObjects),
            list_IsActive = new List<bool>(list_IsActive),
            list_Sprites = new List<bool>(list_Sprites),
            list_HP = new List<int>(list_HP),
            list_Scale = new List<Vector3>(list_Scale),
            list_Positions = new List<Vector3>(list_Positions),
            // プレイヤー関連
            playerPos = pos,
            uI_Items = new List<bool>(uI_Items),
            uI_Items_isOne = new List<bool>(uI_Items_isOne),

        };

        Debug.Log("Saving UI_Items: " + string.Join(", ", newState.uI_Items));
        Debug.Log("Saving UI_Items_isOne: " + string.Join(", ", newState.uI_Items_isOne));

        // Undo状態をリストに追加
        listUndo.Add(newState); // これで参照ではなく、リストのコピーを保存
    }
}

// Undoの状態を管理するクラス
[System.Serializable]
public class UndoState
{
    public List<AllObjectManager> list_AllObjects;
    public List<Vector3> list_Positions;
    public List<bool> list_IsActive;
    public List<bool> list_Sprites;
    public List<int> list_HP;
    public List<BoxCollider2D> list_collider;
    public Vector3 playerPos;
    public List<Vector3> list_Scale;
    public List<bool> uI_Items;
    public List<bool> uI_Items_isOne;

}
