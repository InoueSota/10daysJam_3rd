using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GrassParentScript : MonoBehaviour
{
    [SerializeField] private GrassScript grassPrefab = null;

    [Header("����������")]
    [SerializeField] private float offSetY = 0.5f;

    [Header("�����{��")]
    [SerializeField] private int grassCount = 1;

    [Header("�������̃Y�����̍ő�(�O�̋���*�Y�����̕��Y����)")]
    [SerializeField, Range(0.0f, 1.0f)] private float shiftPerMax = 0;
    private float shiftPer = 0;

    [Header("�����̔䗦")]
    [SerializeField, Range(0.0f, 1.0f)] private float grassLengthPerMax = 0.5f;
    private float grassLengthPer = 0.5f;

    [Header("�֐ߐ�")]
    [SerializeField] private int jointCount = 5;

    // Start is called before the first frame update
    void Start()
    {
        GrassGrow();
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    void GrassGrow()
    {

        for (int grassNum = 0; grassNum < grassCount; grassNum++)
        {
            
            GrassScript grass = null;

            for (int jointNum = 0; jointNum < jointCount; jointNum++)
            {
                //�����ʒu
                Vector3 growPos = Vector3.zero;
                growPos.y = offSetY;
                // �{���ɂ�鋗����
                float grassDistance = 1.0f / grassCount;

                //�ŏ��𔼕����炵�Ă͂₷
                growPos.x = (grassDistance * 0.5f + grassDistance * (grassNum)) - 0.5f - shiftPer;


                //�傫��
                Vector3 grassScale = Vector3.one;

                float oneDot = 1.0f / 16.0f;

                grassScale.x = oneDot * 1.5f;
                grassScale.y = oneDot * 8.0f;


                GameObject parent = this.gameObject;
                //
                if(grass != null)
                {
                    parent = grass.gameObject; 
                }

                grass = Instantiate(grassPrefab, Vector3.zero, Quaternion.identity);

                grass.transform.parent = parent.transform;

                grass.Init(growPos, grassScale);
            }
        }
    }

}
