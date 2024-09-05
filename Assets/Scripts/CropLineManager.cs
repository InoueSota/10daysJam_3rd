using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropLineManager : MonoBehaviour
{
    void LateUpdate()
    {
        transform.localPosition = new(0f, -1f, 0f);
        transform.position = new(0f, transform.position.y, transform.position.z);
    }
}
