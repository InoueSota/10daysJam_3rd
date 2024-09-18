using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CantClearManager : MonoBehaviour
{

    [SerializeField] bool cantClear = false;
    [SerializeField] RectTransform cantClearObj;
    Vector3 originPosition;

    // Start is called before the first frame update
    void Start()
    {
        cantClearObj = GetComponent<RectTransform>();
        originPosition = cantClearObj.localPosition;

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
        Vector3 pos = originPosition;

        if (cantClear == true)
        {
            pos.y += 100;
        }

        cantClearObj.localPosition = pos;
    }

    public void SetCantClear(bool cant)
    {
        cantClear = cant;
    }
}
