using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Item : MonoBehaviour
{
    //�A�C�e���}�l�[�W���[
    [SerializeField] ItemManager itemeManager;
    //�擾�����A�C�e���i�[
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
        //    //�A�C�e�����

        //}
        
    }

    public void Initialize()
    {
        //���X�g���Ȃ����B
        items.Clear ();
    }
    public void addList()
    {

    }

}
