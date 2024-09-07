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
    [SerializeField, Range(0.0f, 1.0f)] private float grassLengthPer = 0.5f;

    [Header("�����̔䗦�̃����_���͈�")]
    [SerializeField, Range(0.0f, 1.0f)] private float grassLengthPerRandom = 0.1f;

    [Header("�֐ߐ�")]
    [SerializeField] private int jointCount = 5;

    [Header("�x�[�X�̒���")]
    [SerializeField] private float length = 7f;

    [Header("�x�[�X�̒����̃����_���͈�")]
    [SerializeField] private float lengthRandom = 1f;

    [Header("�|���͂̉e���͂̌�����")]
    [SerializeField, Range(0.0f, 1.0f)] private float droopingPer = 0.7f;

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


                // �{���ɂ�鋗����
                float grassDistance = 1.0f / grassCount;

                if (jointNum == 0)
                {
                    growPos.y = offSetY;
                    //�ŏ��𔼕����炵�Ă͂₷

                    shiftPer= Random.Range(-shiftPerMax,shiftPerMax) * grassDistance ;

                    growPos.x = (grassDistance * 0.5f + grassDistance * (grassNum)) - 0.5f - shiftPer;
                }
                else
                {
                    growPos.y = grass.GetLength();

                    float wide = grass.GetWide() * 0.5f;

                    growPos.x = Random.Range(-wide, wide);
                }




                //�傫��
                Vector3 grassScale = Vector3.one;

                float oneDot = 1.0f / 16.0f;

                float trueGrassLengthPer = grassLengthPer + Random.Range(-grassLengthPerRandom, grassLengthPerRandom);
                float trueLength = length + Random.Range(-lengthRandom, lengthRandom);

                grassScale.x = oneDot * 2.0f * Mathf.Pow(trueGrassLengthPer, jointNum);
                grassScale.y = oneDot * trueLength * Mathf.Pow(trueGrassLengthPer, jointNum);


                GameObject parent = this.gameObject;

                if (jointNum != 0)
                {
                    parent = grass.gameObject;
                }

                grass = Instantiate(grassPrefab, Vector3.zero, Quaternion.identity);

                grass.transform.parent = parent.transform;

                grass.Init(growPos, grassScale,Mathf.Pow(droopingPer,jointNum));
            }


        }
    }

}
