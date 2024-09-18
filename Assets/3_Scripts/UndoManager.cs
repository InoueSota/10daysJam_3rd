using System.Collections.Generic;
using UnityEngine;

public class UndoManager : MonoBehaviour
{
    // Undo���Ǘ����郊�X�g
    public List<UndoState> listUndo = new List<UndoState>();
    // �I�u�W�F�N�g�̏�Ԃ��Ǘ����郊�X�g
    public List<AllObjectManager> list_AllObjects = new List<AllObjectManager>();
    public List<int> list_HP = new List<int>();
    public List<Vector3> list_Positions = new List<Vector3>();
    public List<bool> list_IsActive = new List<bool>();
    public List<bool> list_Sprites = new List<bool>();
    public List<BoxCollider2D> list_collider = new List<BoxCollider2D>();
    Vector3 playerPos;
    public List<Vector3> list_Scale;
    public List<bool> uI_Items;
    // Start is called before the first frame update
    void Start()
    {
        // �ŏ��ƃ��Z�b�g���ɃI�u�W�F�N�g�̏��擾
        //�u���b�N�֘A
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
        //�v���C���[�֘A
        playerPos = GameObject.FindWithTag("Player").gameObject.transform.position;
        uI_Items =new List<bool> { false,false,false };
        UndoSave();
    }

    public void UndoReSet()
    {
        listUndo.Clear();
    }

    public void UndoSave()
    {
        for (int i = 0; i < list_AllObjects.Count; i++)
        {
            list_HP[i] = list_AllObjects[i].GetHp();
            list_IsActive[i] = list_AllObjects[i].GetIsActive();
            list_Sprites[i]= list_AllObjects[i].GetComponent<SpriteRenderer>().enabled;
            //list_collider[i] =list_AllObjects[i].GetComponent<BoxCollider2D>().get;
            if (list_IsActive[i])
            {
                list_Scale[i] = list_AllObjects[i].gameObject.transform.localScale;

            }
        };
        // �V������Ԃ��쐬
        UndoState newState = new UndoState
        {
            list_AllObjects = new List<AllObjectManager>(list_AllObjects),
            list_Positions = new List<Vector3>(list_Positions),
            list_IsActive = new List<bool>(list_IsActive),
            list_Sprites = new List<bool>(list_Sprites),
            list_HP = new List<int>(list_HP),
            //list_collider=new List<bool>()
            list_Scale = new List<Vector3>(list_Scale),

            playerPos = GameObject.FindWithTag("Player").gameObject.transform.position,
            uI_Items = GameObject.FindWithTag("items").GetComponent<UI_Item>().items
        };
        listUndo.Add(newState);
    }

    public void UndoLoad()
    {
        if (listUndo.Count > 0)
        {
            // �ۑ�����Ă���l�����o��
            UndoState lastState = listUndo[listUndo.Count - 1];

            for (int i = 0; i < list_AllObjects.Count; i++)
            {
                
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
           //�v���C���[�֘A
            GameObject.FindWithTag("Player").transform.position = lastState.playerPos;
            GameObject.FindWithTag("items").GetComponent<UI_Item>().items = uI_Items;


            listUndo.RemoveAt(listUndo.Count - 1); // List�̍Ō�̗v�f���폜����
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
}

// Undo�̏�Ԃ��Ǘ�����N���X
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

}
