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

    private AllObjectManager allObjectManager;
    private AllObjectManager parentObjectManager;

    [SerializeField] ParticleSystem particle = null;

    [SerializeField] private bool isActive = true;
    private bool isGrew = false;

    private bool isGrewed = false;

    // Start is called before the first frame update
    void Start()
    {
        allObjectManager = GetComponent<AllObjectManager>();
        parentObjectManager = transform.parent.GetComponent<AllObjectManager>();
        GrassGrow();
    }

    // Update is called once per f

    void GrassGrow()
    {
        bool canMake = true;

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Object"))
        {
            if (obj != gameObject && obj.GetComponent<AllObjectManager>().GetIsActive() && obj.GetComponent<AllObjectManager>().GetObjectType() != AllObjectManager.ObjectType.ITEM)
            {
                // X������
                float xBetween = Mathf.Abs(transform.position.x - obj.transform.position.x);

                // Y������
                float yBetween = Mathf.Abs(transform.position.y - obj.transform.position.y);

                if (yBetween <= 0.2f && xBetween <= 0.2f)
                {
                    isGrew = false;
                    break;
                }
            }
        } 

        if (canMake)
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

                        shiftPer = Random.Range(-shiftPerMax, shiftPerMax) * grassDistance;

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

                    grass.Init(growPos, grassScale, Mathf.Pow(droopingPer, jointNum), this);
                }


            }
        }
        else
        {
            NotPlace();
        }

       
    }

    public void Damage()
    {
        if (isGrew == true )
        {
            allObjectManager.Damage();

            if (allObjectManager.GetHp() <= 0)
            {
                SetIsActive(false);
            }
        }
    }

    public void SetIsActive(bool _isActive)
    {
        if (_isActive)
        {
            Initialize();
        }
        else
        {
            Disappear();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(parentObjectManager.GetIsActive() == false && isActive == true)
        {
            Disappear();
            return;
        }


        if (isGrew == false && isGrewed == false)
        {
            bool canMake = true;

            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Object"))
            {
                if (obj != gameObject && obj.GetComponent<AllObjectManager>().GetIsActive() && obj.GetComponent<AllObjectManager>().GetObjectType() != AllObjectManager.ObjectType.ITEM)
                {
                    // X������
                    float xBetween = Mathf.Abs(transform.position.x - obj.transform.position.x);

                    // Y������
                    float yBetween = Mathf.Abs(transform.position.y - obj.transform.position.y);

                    if (yBetween <= 0.2f && xBetween <= 0.2f)
                    {
                        canMake = false;
                        break;
                    }
                }
            }

            if(canMake == true)
            {
                isGrew = true;
                isGrewed = true;
            }
        }
    }
    void Initialize()
    {
        isGrewed = false;
        isGrew = false;
        isActive = true;
        allObjectManager.SetIsActive(isActive);
        allObjectManager.Initialize();
    }

    void Disappear()
    {
        isGrew = false;
        isActive = false;
        allObjectManager.SetIsActive(isActive);
        Instantiate(particle,this.transform.position,Quaternion.identity);
    }

    void NotPlace()
    {
        isGrew = false;
        allObjectManager.SetIsActive(isActive);
    }

        public bool GetActive()
    {
        return isActive;
    }

    public bool GetGrew()
    {
        return isGrew;
    }

}
