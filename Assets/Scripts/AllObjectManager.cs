using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllObjectManager : MonoBehaviour
{
    public enum ObjectType
    {
        GROUND,
        BLOCK,
        GOAL
    }
    [SerializeField] private ObjectType objectType;

    void Start()
    {

    }

    void Update()
    {

    }

    public ObjectType GetObjectType()
    {
        return objectType;
    }
}
