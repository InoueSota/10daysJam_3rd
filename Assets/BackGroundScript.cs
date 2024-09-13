using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScript : MonoBehaviour
{

    GameObject camera;

    [SerializeField] Vector3 cameraBasePos = new Vector3(0f,0.34375f,0f);

    [System.Serializable]
    struct BackGroundData{
        [SerializeField] public Transform transform;
        [SerializeField, Range(0.0f, 1.0f)] public float movePer;
    }

    [SerializeField] BackGroundData[] backGround;

    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraPos = camera.transform.position;
        cameraPos.z = 0f;

        Vector3 cameraPosPer = cameraPos - cameraBasePos;

        for (int i = 0; i < backGround.Length; i++)
        {
            backGround[i].transform.localPosition = cameraPosPer * backGround[i].movePer;
            Debug.Log(cameraPosPer * backGround[i].movePer);
        }

    }
}
