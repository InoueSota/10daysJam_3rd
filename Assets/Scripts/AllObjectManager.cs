using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllObjectManager : MonoBehaviour
{
    [SerializeField] private bool isActive = true;

    public enum ObjectType
    {
        GROUND,
        BLOCK,
        ITEM
    }
    [SerializeField] private ObjectType objectType;

    void Start()
    {

    }

    void Update()
    {

    }

    public void SetIsActive(bool _isActive)
    {
        isActive = _isActive;
    }

    public bool GetIsActive()
    {
        return isActive;
    }
    public ObjectType GetObjectType()
    {
        return objectType;
    }
}
