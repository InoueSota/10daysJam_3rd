using UnityEngine;

public class BackGroundScript : MonoBehaviour
{
    private GameObject mainCamera;

    [SerializeField] Vector3 cameraBasePos = new Vector3(0f,0.34375f,0f);

    [System.Serializable]
    struct BackGroundData{
        [SerializeField] public Transform transform;
        [SerializeField, Range(0.0f, 1.0f)] public float movePer;
    }

    [SerializeField] BackGroundData[] backGround;

    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    void Update()
    {
        Vector3 cameraPos = mainCamera.transform.position;
        cameraPos.z = 0f;

        Vector3 cameraPosPer = cameraPos - cameraBasePos;

        for (int i = 0; i < backGround.Length; i++)
        {
            backGround[i].transform.localPosition = cameraPosPer * backGround[i].movePer;
            
        }
    }
}
