using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GrassParentScript : MonoBehaviour
{
    [SerializeField] private GrassScript grassPrefab = null;

    [Header("草召喚高さ")]
    [SerializeField] private float offSetY = 0.5f;

    [Header("召喚本数")]
    [SerializeField] private int grassCount = 1;

    [Header("召喚時のズレ率の最大(前の距離*ズレ率の分ズレる)")]
    [SerializeField, Range(0.0f, 1.0f)] private float shiftPerMax = 0;
    private float shiftPer = 0;

    [Header("長さの比率")]
    [SerializeField, Range(0.0f, 1.0f)] private float grassLengthPer = 0.5f;

    [Header("長さの比率のランダム範囲")]
    [SerializeField, Range(0.0f, 1.0f)] private float grassLengthPerRandom = 0.1f;

    [Header("関節数")]
    [SerializeField] private int jointCount = 5;

    [Header("ベースの長さ")]
    [SerializeField] private float length = 7f;

    [Header("ベースの長さのランダム範囲")]
    [SerializeField] private float lengthRandom = 1f;

    [Header("倒れる力の影響力の減少率")]
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
                // X軸判定
                float xBetween = Mathf.Abs(transform.position.x - obj.transform.position.x);

                // Y軸判定
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
                    //召喚位置
                    Vector3 growPos = Vector3.zero;


                    // 本数による距離間
                    float grassDistance = 1.0f / grassCount;

                    if (jointNum == 0)
                    {
                        growPos.y = offSetY;
                        //最初を半分ずらしてはやす

                        shiftPer = Random.Range(-shiftPerMax, shiftPerMax) * grassDistance;

                        growPos.x = (grassDistance * 0.5f + grassDistance * (grassNum)) - 0.5f - shiftPer;
                    }
                    else
                    {
                        growPos.y = grass.GetLength();

                        float wide = grass.GetWide() * 0.5f;

                        growPos.x = Random.Range(-wide, wide);
                    }




                    //大きさ
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
                    // X軸判定
                    float xBetween = Mathf.Abs(transform.position.x - obj.transform.position.x);

                    // Y軸判定
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
