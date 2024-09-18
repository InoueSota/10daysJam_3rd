using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CantClearManager : MonoBehaviour
{

    [SerializeField] bool cantClear = false;
    [SerializeField] RectTransform cantClearObj;

    // Start is called before the first frame update
    void Start()
    {
        cantClearObj = GetComponent<RectTransform>();

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Object"))
        {
            AllObjectManager allObjectManager = obj.GetComponent<AllObjectManager>();

            if (allObjectManager.GetObjectType() == AllObjectManager.ObjectType.ITEM)
            {
                allObjectManager.GetComponent<ItemManager>().SetCantClearManager(this);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 pos = Vector3.zero;
        pos.x = 1280;
        if (cantClear == false)
        {
            pos.y = -100;
        }

        cantClearObj.position = pos;
    }

    public void SetCantClear(bool cant)
    {
        cantClear = cant;
    }
}
